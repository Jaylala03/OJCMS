using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web.Mvc;
using eCMS.ExceptionLoging;
using System.Text;
using eCMS.DataLogic.ViewModels;

namespace eCMS.BusinessLogic.Repositories
{
    public class WorkerRoleRepository : BaseLookupRepository<WorkerRole>, IWorkerRoleRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public WorkerRoleRepository(RepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Add or Update workerrole to database
        /// </summary>
        /// <param name="role">data to save</param>
        public void InsertOrUpdate(WorkerRole workerrole)
        {
            WorkerRole existingworkerrole = context.WorkerRole.SingleOrDefault(u => u.Name == workerrole.Name && u.IsActive);
            if (existingworkerrole != null && existingworkerrole.ID != workerrole.ID)
            {
                throw new CustomException(CustomExceptionType.CommonDuplicacy, "Worker role name is duplicate.");
            }
            else if (existingworkerrole != null)
            {
                Remove(existingworkerrole);
            }
            workerrole.LastUpdateDate = DateTime.Now;
            if (workerrole.ID == default(int))
            {
                //set the date when this record was created
                workerrole.CreateDate = workerrole.LastUpdateDate;
                //add a new record to database
                context.WorkerRole.Add(workerrole);
            }
            else
            {
                //update an existing record to database
                context.Entry(workerrole).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<SelectListItem> GetAll()
        {
            return context.WorkerRole.Where(item => item.IsActive == true).AsEnumerable().Select(item => new SelectListItem { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }
        public List<SelectListItem> GetWorkerRoleByWorkerID()
        {
            List<SelectListItem> result;
            result = (from wr in context.WorkerInRoleNew
                     join w in context.WorkerRole on wr.WorkerRoleID equals w.ID
                     where wr.WorkerID == CurrentLoggedInWorker.ID && wr.EffectiveFrom <= DateTime.Now && wr.EffectiveTo >= DateTime.Now
                     select new SelectListItem { Text = w.Name, Value = wr.WorkerRoleID.ToString() })
                    .ToList();

            return result;
        }

        public List<SelectListItem> GetWorkerRoleByProgramAndRegionID(int programID, int regionID, int subProgramID, int? jamatKhanaID)
        {
            List<DropDownViewModel> worker = null;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT WR.ID,WR.Name ");
            sqlQuery.Append("FROM WorkerRole AS WR ");
            sqlQuery.Append("INNER JOIN WorkerInRoleNew AS WIR ON WR.ID = WIR.WorkerRoleID ");

            if (programID > 0 || regionID > 0)
            {
                sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WR.ID = WRP.WorkerRoleID AND WIR.WorkerRoleID = WRP.WorkerRoleID ");
                sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
                sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");

                if (programID > 0)
                    sqlQuery.Append(" AND PR.ProgramID = " + programID + " ");

                if (regionID > 0)
                    sqlQuery.Append(" AND PR.RegionID  = " + regionID + " ");

                if (subProgramID > 0)
                {
                    sqlQuery.Append("INNER JOIN PermissionSubProgram PSPRG ON PR.ID = PSPRG.PermissionRegionID ");
                    sqlQuery.Append(" AND PSPRG.SubProgramID = " + subProgramID + " ");
                }
                if (jamatKhanaID.HasValue && jamatKhanaID.Value > 0)
                {
                    sqlQuery.Append("INNER JOIN PermissionJamatkhana PJK ON PR.ID = PJK.PermissionRegionID ");
                    sqlQuery.Append(" AND PJK.JamatkhanaID = " + jamatKhanaID.Value + " ");
                }
            }

            sqlQuery.Append(" WHERE WIR.EffectiveFrom <= '" + DateTime.Now + "' AND WIR.EffectiveTo >= '" + DateTime.Now + "' ");
            sqlQuery.Append("GROUP BY WR.ID,WR.Name ");
            sqlQuery.Append("ORDER BY WR.Name ");

            worker = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();
            return worker.AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();

        }
        public int IsWorkerRegionalAdmin()
        {
            int result = 0;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("IF EXISTS( ");
            sqlQuery.Append("SELECT TOP 1 WR.ID ");
            sqlQuery.Append("FROM ");
            sqlQuery.Append("WorkerRole AS WR ");
            sqlQuery.Append("WHERE ");
            sqlQuery.Append("WR.IsRegionalAdmin = 1 AND WR.ID IN (" + loggedinworkers + ") ");
            sqlQuery.Append(") ");
            sqlQuery.Append("BEGIN ");
            sqlQuery.Append("SELECT 1 AS result; ");
            sqlQuery.Append("END ");
            sqlQuery.Append("ELSE ");
            sqlQuery.Append("BEGIN ");
            sqlQuery.Append("SELECT 0 AS result; ");
            sqlQuery.Append("END ");

            result = context.Database.SqlQuery<int>(sqlQuery.ToString()).FirstOrDefault();

            return result;
        }
    }

    /// <summary>
    /// interface of Role containing necessary database operations
    /// </summary>
    public interface IWorkerRoleRepository : IBaseLookupRepository<WorkerRole>
    {
        void InsertOrUpdate(WorkerRole workerrole);
        List<SelectListItem> GetAll();

        int IsWorkerRegionalAdmin();
        List<SelectListItem> GetWorkerRoleByWorkerID();

        List<SelectListItem> GetWorkerRoleByProgramAndRegionID(int programID, int regionID, int subProgramID, int? jamatKhanaID);
    }
}
