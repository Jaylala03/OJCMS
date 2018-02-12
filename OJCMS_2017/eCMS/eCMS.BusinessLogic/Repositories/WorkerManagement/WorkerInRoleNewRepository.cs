//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using eCMS.DataLogic.Models;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.ExceptionLoging;
using eCMS.DataLogic.ViewModels;
using System.Text;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class WorkerInRoleNewRepository : BaseRepository<WorkerInRoleNew>, IWorkerInRoleNewRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public WorkerInRoleNewRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<WorkerInRoleNew> AllIncluding(int workerId, params Expression<Func<WorkerInRoleNew, object>>[] includeProperties)
        {
            IQueryable<WorkerInRoleNew> query = context.WorkerInRoleNew;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.WorkerID == workerId).GroupBy(m => new { m.WorkerRoleID }).Select(m => m.FirstOrDefault());
        }

        public List<WorkerInRoleNew> FindAllActiveByWorkerID(int workerID)
        {
            return context.WorkerInRoleNew.Where(item => item.WorkerID == workerID && item.EffectiveFrom <= DateTime.Now && item.EffectiveTo >= DateTime.Now).GroupBy(m => new { m.WorkerRoleID }).Select(m => m.FirstOrDefault()).ToList();
        }

        public List<WorkerInRoleNew> FindAllByWorkerID(int workerID)
        {
            return context.WorkerInRoleNew.Where(item => item.WorkerID == workerID).GroupBy(m => new { m.WorkerRoleID }).Select(m => m.FirstOrDefault()).ToList();
        }
        public List<WorkerInRoleNewLVM> IndexGetAllPermission(int workerroleID)
        {
            List<WorkerInRoleNewLVM> result = null;

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT ");
            sqlQuery.Append("ISNULL(pm.Name,'') AS PermissionName,ISNULL(r.Name,'') AS RegionName,ISNULL(p.Name,'') AS ProgramName, ");
            sqlQuery.Append("(SELECT ISNULL(STUFF( (SELECT ',' + Name ");
            sqlQuery.Append("FROM SubProgram AS sp ");
            sqlQuery.Append("INNER JOIN PermissionSubProgram AS psp ON sp.ID = psp.SubProgramID ");
            sqlQuery.Append("WHERE psp.PermissionRegionID = pr.ID ");
            sqlQuery.Append("ORDER BY sp.Name ");
            sqlQuery.Append("FOR XML PATH('')), ");
            sqlQuery.Append("1, 1, ''),'')) AS SubProgramNames, ");
            sqlQuery.Append("(SELECT ISNULL(STUFF( (SELECT ',' + Name ");
            sqlQuery.Append("FROM Jamatkhana AS j ");
            sqlQuery.Append("INNER JOIN PermissionJamatkhana AS pj ON j.ID = pj.JamatkhanaID ");
            sqlQuery.Append("WHERE pj.PermissionRegionID = pr.ID ");
            sqlQuery.Append("ORDER BY j.Name ");
            sqlQuery.Append("FOR XML PATH('')), ");
            sqlQuery.Append("1, 1, ''),'')) AS JamatkhanaNames ");
            sqlQuery.Append("FROM WorkerRolePermissionNew AS wrp ");
            sqlQuery.Append("LEFT JOIN Permission AS pm ON wrp.PermissionID = pm.ID ");
            sqlQuery.Append("LEFT JOIN PermissionRegion AS pr ON wrp.PermissionID = pr.PermissionID ");
            sqlQuery.Append("LEFT JOIN Program AS p ON pr.ProgramID = p.ID ");
            sqlQuery.Append("LEFT JOIN Region AS r ON pr.RegionID = r.ID ");
            sqlQuery.Append("WHERE wrp.WorkerRoleID = " + workerroleID + " ");
            sqlQuery.Append("ORDER BY pm.ID ");

            result = context.Database.SqlQuery<WorkerInRoleNewLVM>(sqlQuery.ToString()).ToList();

            return result;

            //return context.WorkerInRoleNew.Where(item => item.WorkerID == workerID).GroupBy(m => new { m.WorkerRoleID }).Select(m => m.FirstOrDefault()).ToList();
        }
        public IQueryable<WorkerInRoleNew> FindAllByWorkerRoleID(int workerroleID)
        {
            return context.WorkerInRoleNew.Where(item => item.WorkerRoleID == workerroleID).GroupBy(m => new { m.WorkerRoleID }).Select(m => m.FirstOrDefault());
        }

        /// <summary>
        /// Add or Update workerinrole to database
        /// </summary>s
        /// <param name="workerinrole">data to save</param>
        public void InsertOrUpdate(WorkerInRoleNew workerinrole)
        {
            var existingWorkerInRole = context.WorkerInRoleNew.SingleOrDefault(item => item.WorkerID == workerinrole.WorkerID && item.WorkerRoleID == workerinrole.WorkerRoleID);
            if (existingWorkerInRole != null && existingWorkerInRole.ID != workerinrole.ID)
            {
                workerinrole.ID = existingWorkerInRole.ID;
                workerinrole.CreateDate = existingWorkerInRole.CreateDate;
                workerinrole.CreatedByWorkerID = existingWorkerInRole.CreatedByWorkerID;
                Remove(existingWorkerInRole);
            }
            workerinrole.LastUpdateDate = DateTime.Now;
            if (workerinrole.ID == default(int))
            {
                //set the date when this record was created
                workerinrole.CreateDate = workerinrole.LastUpdateDate;
                //set the id of the worker who has created this record
                workerinrole.CreatedByWorkerID = workerinrole.LastUpdatedByWorkerID;
                //add a new record to database
                context.WorkerInRoleNew.Add(workerinrole);
            }
            else
            {
                //update an existing record to database
                context.Entry(workerinrole).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
        }

        public override WorkerInRoleNew Find(int id)
        {
            return context.WorkerInRoleNew.SingleOrDefault(item => item.ID == id);
        }

        public override void Delete(int id)
        {
            string sqlQuery = "DELETE FROM WorkerInRoleNew WHERE ID = " + id.ToString() + ";";
            context.Database.ExecuteSqlCommand(sqlQuery);
        }
        public List<int> FindAllActiveWorkerInRoleByWorkerID()
        {
            List<int> result = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append(" SELECT WIR.WorkerRoleID ");
            sqlQuery.Append(" FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append(" WHERE WIR.WorkerID = " + CurrentLoggedInWorker.ID);
            sqlQuery.Append(" AND WIR.EffectiveFrom <= '" + DateTime.Now + "' AND WIR.EffectiveTo >= '" + DateTime.Now + "' ");
            sqlQuery.Append(" GROUP BY WIR.WorkerRoleID ");

            result = context.Database.SqlQuery<int>(sqlQuery.ToString()).ToList();

            return result;
        }

        public List<int> FindAllActiveRegionByWorkerID()
        {
            List<int> result = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append(" SELECT PR.RegionID ");
            sqlQuery.Append(" FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append(" INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append(" INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append(" INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append(" INNER JOIN Region AS R ON PR.RegionID = R.ID AND R.IsActive = 1 ");
            sqlQuery.Append(" WHERE WIR.WorkerID = " + CurrentLoggedInWorker.ID);
            sqlQuery.Append(" AND WIR.EffectiveFrom <= '" + DateTime.Now + "' AND WIR.EffectiveTo >= '" + DateTime.Now + "' ");
            sqlQuery.Append(" GROUP BY PR.RegionID ");

            result = context.Database.SqlQuery<int>(sqlQuery.ToString()).ToList();

            return result;
        }

        public string FindAllActiveProgramByWorkerID(int WorkerID)
        {
            List<string> result = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append(" SELECT PRG.Name ");
            sqlQuery.Append(" FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append(" INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append(" INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append(" INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append(" INNER JOIN Program AS PRG ON PR.ProgramID = PRG.ID AND PRG.IsActive = 1 ");
            sqlQuery.Append(" WHERE WIR.WorkerID = " + WorkerID);
            sqlQuery.Append(" AND WIR.EffectiveFrom <= '" + DateTime.Now + "' AND WIR.EffectiveTo >= '" + DateTime.Now + "' ");
            sqlQuery.Append(" GROUP BY PRG.Name ");

            result = context.Database.SqlQuery<string>(sqlQuery.ToString()).ToList();

            return String.Join(",", result);
        }
    }

    /// <summary>
    /// interface of WorkerInRole containing necessary database operations
    /// </summary>
    public interface IWorkerInRoleNewRepository : IBaseRepository<WorkerInRoleNew>
    {
        IQueryable<WorkerInRoleNew> AllIncluding(int workerId, params Expression<Func<WorkerInRoleNew, object>>[] includeProperties);
        List<WorkerInRoleNew> FindAllActiveByWorkerID(int workerID);
        List<WorkerInRoleNew> FindAllByWorkerID(int workerID);
        List<WorkerInRoleNewLVM> IndexGetAllPermission(int workerroleID);
        IQueryable<WorkerInRoleNew> FindAllByWorkerRoleID(int workerroleID);
        void InsertOrUpdate(WorkerInRoleNew workerinrolenew);

        List<int> FindAllActiveRegionByWorkerID();
        List<int> FindAllActiveWorkerInRoleByWorkerID();

        string FindAllActiveProgramByWorkerID(int workerID);
    }
}
