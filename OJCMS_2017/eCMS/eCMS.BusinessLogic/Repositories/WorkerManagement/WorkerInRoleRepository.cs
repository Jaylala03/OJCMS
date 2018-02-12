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

namespace eCMS.BusinessLogic.Repositories
{ 
	/// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class WorkerInRoleRepository : BaseRepository<WorkerInRole>, IWorkerInRoleRepository
    {
        protected IWorkerSubProgramRepository workersubprogramRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public WorkerInRoleRepository(RepositoryContext context, IWorkerSubProgramRepository workersubprogramRepository)
            : base(context)
        {
            this.workersubprogramRepository = workersubprogramRepository;
        }

        public IQueryable<WorkerInRole> AllIncluding(int workerId, params Expression<Func<WorkerInRole, object>>[] includeProperties)
        {
            IQueryable<WorkerInRole> query = context.WorkerInRole;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.WorkerID == workerId).GroupBy(m => new { m.ProgramID, m.RegionID, m.WorkerRoleID }).Select(m => m.FirstOrDefault());
        }

        public List<WorkerInRole> FindAllActiveByWorkerID(int workerID)
        {
            return context.WorkerInRole.Where(item => item.WorkerID == workerID && item.EffectiveFrom <= DateTime.Now && item.EffectiveTo >= DateTime.Now).GroupBy(m => new { m.ProgramID, m.RegionID, m.WorkerRoleID }).Select(m => m.FirstOrDefault()).ToList();
        }

        public List<WorkerInRole> FindAllByWorkerID(int workerID)
        {
            return context.WorkerInRole.Where(item => item.WorkerID == workerID).GroupBy(m => new { m.ProgramID,m.RegionID,m.WorkerRoleID}).Select(m=>m.FirstOrDefault()).ToList();
        }

        public IQueryable<WorkerInRole> FindAllByWorkerRoleID(int workerroleID)
        {
            return context.WorkerInRole.Where(item => item.WorkerRoleID == workerroleID).GroupBy(m => new { m.ProgramID, m.RegionID, m.WorkerRoleID }).Select(m => m.FirstOrDefault());
        }

        /// <summary>
        /// Add or Update workerinrole to database
        /// </summary>
        /// <param name="workerinrole">data to save</param>
        public void InsertOrUpdate(WorkerInRole workerinrole)
        {
            var existingWorkerInRole = context.WorkerInRole.SingleOrDefault(item => item.WorkerID == workerinrole.WorkerID && item.WorkerRoleID == workerinrole.WorkerRoleID && item.ProgramID==workerinrole.ProgramID && item.RegionID==workerinrole.RegionID);
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
                context.WorkerInRole.Add(workerinrole);
            }
            else
            {
                //update an existing record to database
                context.Entry(workerinrole).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            workersubprogramRepository.InsertOrUpdate(workerinrole.ID, workerinrole.SubProgramIDs);
        }

        public override WorkerInRole Find(int id)
        {
            return context.WorkerInRole.SingleOrDefault(item => item.ID == id);
        }

        public override void Delete(int id)
        {
            string sqlQuery = "DELETE FROM WorkerSubProgram WHERE WorkerInRoleID=" + id.ToString() + ";DELETE FROM WorkerInRole WHERE ID=" + id.ToString()+";";
            context.Database.ExecuteSqlCommand(sqlQuery);
        }
    }

	/// <summary>
    /// interface of WorkerInRole containing necessary database operations
    /// </summary>
    public interface IWorkerInRoleRepository : IBaseRepository<WorkerInRole>
    {
        IQueryable<WorkerInRole> AllIncluding(int workerId, params Expression<Func<WorkerInRole, object>>[] includeProperties);
        List<WorkerInRole> FindAllActiveByWorkerID(int workerID);
        List<WorkerInRole> FindAllByWorkerID(int workerID);
        IQueryable<WorkerInRole> FindAllByWorkerRoleID(int workerroleID);
        void InsertOrUpdate(WorkerInRole workerinrole);
    }
}
