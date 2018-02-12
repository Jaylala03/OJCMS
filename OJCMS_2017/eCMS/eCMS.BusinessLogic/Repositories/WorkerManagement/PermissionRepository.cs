using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System;
using System.Linq;
using eCMS.DataLogic.Models;
using System.Text;
using System.Web.Mvc;
using eCMS.ExceptionLoging;

namespace eCMS.BusinessLogic.Repositories
{
    public class PermissionRepository : BaseLookupRepository<Permission>, IPermissionRepository
    {
        protected IPermissionActionRepository permissionactionRepository;

        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public PermissionRepository(RepositoryContext context, IPermissionActionRepository permissionactionRepository)
            : base(context)
        {
            this.permissionactionRepository = permissionactionRepository;
        }

        /// <summary>
        /// Add or Update Permission to database
        /// </summary>
        /// <param name="role">data to save</param>
        public void InsertOrUpdate(Permission permission)
        {
            Permission existingPermission = context.Permission.SingleOrDefault(u => u.Name == permission.Name && u.IsActive);
            if (existingPermission != null && existingPermission.ID != permission.ID)
            {
                throw new CustomException(CustomExceptionType.CommonDuplicacy, "Permission name is duplicate.");
            }
            else if (existingPermission != null)
            {
                Remove(existingPermission);
            }
            permission.LastUpdateDate = DateTime.Now;

            if (permission.ID == default(int))
            {
                //set the date when this record was created
                permission.CreateDate = permission.LastUpdateDate;
                //add a new record to database
                context.Permission.Add(permission);
            }
            else
            {
                //update an existing record to database
                context.Entry(permission).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            permissionactionRepository.InsertOrUpdate(permission.ID, permission.ActionMethodIDs);
        }

       
        public override void Delete(int id)
        {
            StringBuilder sqlQuery = new StringBuilder("DELETE PSP FROM PermissionSubProgram PSP ");
            sqlQuery.Append("INNER JOIN PermissionRegion PR ON PSP.PermissionRegionID = PR.ID WHERE PR.PermissionID = " + id.ToString());
            sqlQuery.Append(";");
            sqlQuery.Append("DELETE PJK FROM PermissionJamatkhana PJK ");
            sqlQuery.Append("INNER JOIN PermissionRegion PR ON PJK.PermissionRegionID = PR.ID WHERE PR.PermissionID = " + id.ToString());
            sqlQuery.Append(";");
            sqlQuery.Append("DELETE FROM PermissionRegion WHERE PermissionID = " + id.ToString());
            sqlQuery.Append(";");
            sqlQuery.Append("DELETE FROM PermissionAction WHERE PermissionID = " + id.ToString());
            sqlQuery.Append(";");
            sqlQuery.Append("DELETE FROM Permission WHERE ID = " + id.ToString());
            context.Database.ExecuteSqlCommand(sqlQuery.ToString());
        }

        public List<SelectListItem> GetAll()
        {
            return context.Permission.Where(item => item.IsActive == true).AsEnumerable().Select(item => new SelectListItem { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }
    }

    /// <summary>
    /// interface of Role containing necessary database operations
    /// </summary>
    public interface IPermissionRepository : IBaseLookupRepository<Permission>
    {
        void InsertOrUpdate(Permission permission);
        List<SelectListItem> GetAll();
    }
}
