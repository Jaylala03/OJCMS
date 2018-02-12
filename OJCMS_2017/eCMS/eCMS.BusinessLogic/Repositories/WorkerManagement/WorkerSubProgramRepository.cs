//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EasySoft.Helper;
namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class WorkerSubProgramRepository : BaseRepository<WorkerSubProgram>, IWorkerSubProgramRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public WorkerSubProgramRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<WorkerSubProgram> AllIncluding(int workerId, params Expression<Func<WorkerSubProgram, object>>[] includeProperties)
        {
            IQueryable<WorkerSubProgram> query = context.WorkerSubProgram;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.WorkerInRole.WorkerID == workerId);
        }

        public List<WorkerSubProgram> FindAllByWorkerID(int workerID)
        {
            return context.WorkerSubProgram.Where(item => item.WorkerInRole.WorkerID == workerID).ToList();
        }

        /// <summary>
        /// Add or Update workerSubProgram to database
        /// </summary>
        /// <param name="workerSubProgram">data to save</param>
        public void InsertOrUpdate(int workerInRoleID, string[] arraySelectedSubPrograms)
        {
            List<WorkerSubProgram> assignment = context.WorkerSubProgram.Where(item => item.WorkerInRoleID == workerInRoleID).ToList();
            if (arraySelectedSubPrograms != null && arraySelectedSubPrograms.Length > 0)
            {
                foreach (string subProgramID in arraySelectedSubPrograms)
                {
                    if (assignment.Where(item => item.SubProgramID == subProgramID.ToInteger(true)).Count() == 0)
                    {
                        WorkerSubProgram newRegionSubProgram = new WorkerSubProgram()
                        {
                            WorkerInRoleID = workerInRoleID,
                            SubProgramID = subProgramID.ToInteger(true),
                            LastUpdateDate = DateTime.Now,
                            LastUpdatedByWorkerID = CurrentLoggedInWorker.ID,
                            CreatedByWorkerID = CurrentLoggedInWorker.ID
                        };
                        InsertOrUpdate(newRegionSubProgram);
                        Save();
                    }
                }
            }

            foreach (WorkerSubProgram existingMember in assignment)
            {
                if (arraySelectedSubPrograms == null || !arraySelectedSubPrograms.Contains(existingMember.SubProgramID.ToString(true)))
                {
                    Delete(existingMember);
                    Save();
                }
            }
        }

        public override WorkerSubProgram Find(int id)
        {
            return context.WorkerSubProgram.SingleOrDefault(item => item.ID == id);
        }

        public List<WorkerSubProgram> FindAllByWorkerInRoleID(int workerInRoleID)
        {
            return context.WorkerSubProgram.Where(item => item.WorkerInRoleID == workerInRoleID).ToList();
        }
    }

    /// <summary>
    /// interface of WorkerSubProgram containing necessary database operations
    /// </summary>
    public interface IWorkerSubProgramRepository : IBaseRepository<WorkerSubProgram>
    {
        IQueryable<WorkerSubProgram> AllIncluding(int workerId, params Expression<Func<WorkerSubProgram, object>>[] includeProperties);
        List<WorkerSubProgram> FindAllByWorkerID(int workerID);
        List<WorkerSubProgram> FindAllByWorkerInRoleID(int workerInRoleID);
        void InsertOrUpdate(int workerInRoleID, string[] arraySelectedSubPrograms);
    }
}
