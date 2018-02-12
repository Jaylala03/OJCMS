using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System;
using System.Linq;
using Kendo.Mvc.UI;
using System.Text;
using Kendo.Mvc.Extensions;
using eCMS.DataLogic.ViewModels;
using eCMS.Shared;

namespace eCMS.BusinessLogic.Repositories
{
    public class WorkerRolePermissionNewRepository : BaseRepository<WorkerRolePermissionNew>, IWorkerRolePermissionNewRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public WorkerRolePermissionNewRepository(RepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Add or Update Worker Role Permission New to database
        /// </summary>
        /// <param name="role">data to save</param>
        public void InsertOrUpdate(WorkerRolePermissionNew workerrolepermission)
        {
            workerrolepermission.LastUpdateDate = DateTime.Now;
            if (workerrolepermission.ID == default(int))
            {
                //set the date when this record was created
                workerrolepermission.CreateDate = workerrolepermission.LastUpdateDate;
                //add a new record to database
                context.WorkerRolePermissionNew.Add(workerrolepermission);
            }
            else
            {
                //update an existing record to database
                context.Entry(workerrolepermission).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public DataSourceResult GetIndexList(DataSourceRequest paramDSRequest)
        {
            StringBuilder sqlQuery = new StringBuilder(@"SELECT wrp.ID,
                            wrp.CreateDate,
                            wrp.LastUpdateDate,
                            wrp.WorkerRoleID,
                            wrp.PermissionID,
                            wrp.IsActive,
                            p.Name AS PermissionName,
                            w.Name AS WorkerRoleName
                            FROM WorkerRolePermissionNew AS wrp
                            INNER JOIN permission AS p on wrp.PermissionID = p.ID
                            INNER JOIN WorkerRole AS w on wrp.WorkerRoleID = w.ID 
                            ORDER BY wrp.CreateDate");

            DataSourceResult dataSourceResult = context.Database.SqlQuery<WorkRolePermissionNewViewModel>(sqlQuery.ToString()).ToDataSourceResult(paramDSRequest);
            return dataSourceResult;
        }

        public WorkerRolePermissionNew FindByID(int WorkerRolePermissionNewId)
        {
            WorkerRolePermissionNew rolepermission = new WorkerRolePermissionNew();
            StringBuilder sqlQuery = new StringBuilder(@"SELECT wrp.ID,
                            wrp.CreateDate,
                            wrp.LastUpdateDate,
                            wrp.WorkerRoleID,
                            wrp.PermissionID,
                            wrp.IsActive,
                             wrp.CreateDate,
                             wrp.LastUpdateDate
                            FROM WorkerRolePermissionNew AS wrp ");
            sqlQuery.Append(" WHERE wrp.ID=" + WorkerRolePermissionNewId + "");

            var rolepermissionModel = context.Database.SqlQuery<WorkRolePermissionNewViewModel>(sqlQuery.ToString()).FirstOrDefault();
            if (rolepermissionModel != null)
            {
                rolepermission.ID = rolepermissionModel.ID;
                rolepermission.WorkerRoleID = rolepermissionModel.WorkerRoleID;
                rolepermission.PermissionID = rolepermissionModel.PermissionID;
                rolepermission.IsActive = rolepermissionModel.IsActive;
                rolepermission.CreateDate = rolepermissionModel.CreateDate;
                rolepermission.LastUpdateDate = rolepermissionModel.LastUpdateDate;
            }
            return rolepermission;

        }

        public void FindVisiblity(int workerID,
           ref VisibilityStatus regionVisiblity,
           ref VisibilityStatus programVisiblity,
           ref VisibilityStatus subProgramVisiblity,
           ref VisibilityStatus caseVisiblity)
        {
            //List<WorkerRolePermissionNew> permissionList = context.WorkerInRoleNew.Join(context.WorkerRolePermissionNew, left => left.WorkerRoleID, right => right.WorkerRoleID, (left, right) => new { left, right }).
            //    Where(item => item.left.WorkerID == workerID).Select(item => item.right).ToList();
          
            //int count = permissionList.Where(item => item.Permission == 1 || item.Permission == 2 || item.Permission == 3 || item.Permission == 4 || item.Permission == 5 || item.Permission == 6 || item.Permission == 7 || item.Permission == 8).Count();
            //if (count > 0 && (regionVisiblity == VisibilityStatus.None || regionVisiblity == VisibilityStatus.UnDefined))
            //{
                //regionVisiblity = VisibilityStatus.Assigned;
            //}

            //count = permissionList.Where(item => item.Permission == 1 || item.Permission == 2 || item.Permission == 5 || item.Permission == 6 || item.Permission == 3 || item.Permission == 4 || item.Permission == 7 || item.Permission == 8).Count();
            //if (count > 0 && (programVisiblity == VisibilityStatus.None || programVisiblity == VisibilityStatus.UnDefined))
            //{
                //programVisiblity = VisibilityStatus.Assigned;
                //subProgramVisiblity = VisibilityStatus.Assigned;
            //}

            ////Case Visibility
            //count = permissionList.Where(item => item.Permission == 1 || item.Permission == 3 || item.Permission == 5 || item.Permission == 7).Count();
            //if (count > 0)
            //{
            //    caseVisiblity = VisibilityStatus.All;
            //}
            //count = permissionList.Where(item => item.Permission == 2 || item.Permission == 4 || item.Permission == 6 || item.Permission == 8).Count();
            //if (count > 0 && (caseVisiblity == VisibilityStatus.None || caseVisiblity == VisibilityStatus.UnDefined))
            //{
                //caseVisiblity = VisibilityStatus.Assigned;

                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
                {
                    regionVisiblity = VisibilityStatus.All;
                    programVisiblity = VisibilityStatus.All;
                    subProgramVisiblity = VisibilityStatus.All;
                    caseVisiblity = VisibilityStatus.All;
                }
                else
                {
                    regionVisiblity = VisibilityStatus.Assigned;
                    programVisiblity = VisibilityStatus.Assigned;
                    subProgramVisiblity = VisibilityStatus.Assigned;
                    caseVisiblity = VisibilityStatus.Assigned;
                }
            //}
        }
    }

    /// <summary>
    /// interface of Work Role Permission containing necessary database operations
    /// </summary>
    public interface IWorkerRolePermissionNewRepository : IBaseRepository<WorkerRolePermissionNew>
    {
        void InsertOrUpdate(WorkerRolePermissionNew workerrolepermission);
        DataSourceResult GetIndexList(DataSourceRequest paramDSRequest);
        WorkerRolePermissionNew FindByID(int WorkerRolePermissionNewId);

        void FindVisiblity(int workerID,
            ref VisibilityStatus regionVisiblity,
            ref VisibilityStatus programVisiblity,
            ref VisibilityStatus subProgramVisiblity,
            ref VisibilityStatus caseVisiblity);
    }
}
