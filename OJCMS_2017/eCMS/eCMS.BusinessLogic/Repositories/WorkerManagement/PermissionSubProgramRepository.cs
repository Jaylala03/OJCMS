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
using eCMS.DataLogic.Models.Lookup;
namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class PermissionSubProgramRepository : BaseRepository<PermissionSubProgram>, IPermissionSubProgramRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public PermissionSubProgramRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<PermissionSubProgram> AllIncluding(int permissionID, params Expression<Func<PermissionSubProgram, object>>[] includeProperties)
        {
            IQueryable<PermissionSubProgram> query = context.PermissionSubProgram;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.PermissionRegion.PermissionID == permissionID);
        }

        public List<PermissionSubProgram> FindAllByPermissionID(int permissionID)
        {
            return context.PermissionSubProgram.Where(item => item.PermissionRegion.PermissionID == permissionID).ToList();
        }

        /// <summary>
        /// Add or Update permissionSubProgram to database
        /// </summary>
        /// <param name="permissionSubProgram">data to save</param>
        public void InsertOrUpdate(int permissionRegionID, string[] arraySelectedSubPrograms)
        {
            List<PermissionSubProgram> assignment = context.PermissionSubProgram.Where(item => item.PermissionRegionID == permissionRegionID).ToList();
            if (arraySelectedSubPrograms != null && arraySelectedSubPrograms.Length > 0)
            {
                foreach (string subProgramID in arraySelectedSubPrograms)
                {
                    if (assignment.Where(item => item.SubProgramID == subProgramID.ToInteger(true)).Count() == 0)
                    {
                        PermissionSubProgram newRegionSubProgram = new PermissionSubProgram()
                        {
                            PermissionRegionID = permissionRegionID,
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

            foreach (PermissionSubProgram existingMember in assignment)
            {
                if (arraySelectedSubPrograms == null || !arraySelectedSubPrograms.Contains(existingMember.SubProgramID.ToString(true)))
                {
                    Delete(existingMember);
                    Save();
                }
            }
        }

        public override PermissionSubProgram Find(int id)
        {
            return context.PermissionSubProgram.SingleOrDefault(item => item.ID == id);
        }

        public List<PermissionSubProgram> FindAllByPermissionRegionID(int permissionRegionID)
        {
            return context.PermissionSubProgram.Where(item => item.PermissionRegionID == permissionRegionID).ToList();
        }
        public List<SubProgram> FindAllSubProgramByWordkerID(int workerID, int programID)
        {
            if (programID > 0 && workerID > 0)
            {
                var data = context.PermissionSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.PermissionRegion.CreatedByWorkerID == workerID && item.right.ProgramID == programID)
                    .GroupBy(item => new { item.right })
                    .OrderBy(item => item.Key.right.Name).Select(item => item.Key.right).ToList();
                return data;
            }
            else if (workerID > 0)
            {
                var data = context.PermissionSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.PermissionRegion.CreatedByWorkerID == workerID)
                    .GroupBy(item => new { item.right })
                    .OrderBy(item => item.Key.right.Name).Select(item => item.Key.right).ToList();
                return data;
            }
            return null;
        }
    }

    /// <summary>
    /// interface of PermissionSubProgram containing necessary database operations
    /// </summary>
    public interface IPermissionSubProgramRepository : IBaseRepository<PermissionSubProgram>
    {
        IQueryable<PermissionSubProgram> AllIncluding(int permissionId, params Expression<Func<PermissionSubProgram, object>>[] includeProperties);
        List<PermissionSubProgram> FindAllByPermissionID(int permissionID);
        List<PermissionSubProgram> FindAllByPermissionRegionID(int permissionRegionID);
        void InsertOrUpdate(int permissionRegionID, string[] arraySelectedSubPrograms);
        List<SubProgram> FindAllSubProgramByWordkerID(int workerID, int programID);
    }
}
