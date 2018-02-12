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
    public class PermissionJamatkhanaRepository : BaseRepository<PermissionJamatkhana>, IPermissionJamatkhanaRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public PermissionJamatkhanaRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<PermissionJamatkhana> AllIncluding(int permissionID, params Expression<Func<PermissionJamatkhana, object>>[] includeProperties)
        {
            IQueryable<PermissionJamatkhana> query = context.PermissionJamatkhana;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.PermissionRegion.PermissionID == permissionID);
        }

        public List<PermissionJamatkhana> FindAllByPermissionID(int permissionID)
        {
            return context.PermissionJamatkhana.Where(item => item.PermissionRegion.PermissionID == permissionID).ToList();
        }

        /// <summary>
        /// Add or Update permissionJamatkhana to database
        /// </summary>
        /// <param name="permissionJamatkhana">data to save</param>
        public void InsertOrUpdate(int permissionRegionID, List<int> arraySelectedJamatkhanas)
        {
            List<PermissionJamatkhana> assignment = context.PermissionJamatkhana.Where(item => item.PermissionRegionID == permissionRegionID).ToList();
            if (arraySelectedJamatkhanas != null && arraySelectedJamatkhanas.Count > 0)
            {
                foreach (int JamatkhanaID in arraySelectedJamatkhanas)
                {
                    if (assignment.Where(item => item.JamatkhanaID == JamatkhanaID.ToInteger(true)).Count() == 0)
                    {
                        PermissionJamatkhana newRegionJamatkhana = new PermissionJamatkhana()
                        {
                            PermissionRegionID = permissionRegionID,
                            JamatkhanaID = JamatkhanaID,
                            LastUpdateDate = DateTime.Now,
                            LastUpdatedByWorkerID = CurrentLoggedInWorker.ID,
                            CreatedByWorkerID = CurrentLoggedInWorker.ID
                        };
                        InsertOrUpdate(newRegionJamatkhana);
                        Save();
                    }
                }
            }

            foreach (PermissionJamatkhana existingMember in assignment)
            {
                if (arraySelectedJamatkhanas == null || arraySelectedJamatkhanas.IndexOf(existingMember.JamatkhanaID) == -1)
                {
                    Delete(existingMember);
                    Save();
                }
            }
        }

        public override PermissionJamatkhana Find(int id)
        {
            return context.PermissionJamatkhana.SingleOrDefault(item => item.ID == id);
        }

        public List<PermissionJamatkhana> FindAllByPermissionRegionID(int permissionRegionID)
        {
            return context.PermissionJamatkhana.Where(item => item.PermissionRegionID == permissionRegionID).ToList();
        }

        public List<Jamatkhana> FindAllJamatkhanaByWordkerID(int workerID, int regionID)
        {
            if (regionID > 0 && workerID > 0)
            {
                var data = context.PermissionJamatkhana.Join(context.Jamatkhana, left => left.JamatkhanaID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.PermissionRegion.CreatedByWorkerID == workerID && item.right.RegionID == regionID)
                    .GroupBy(item => new { item.right })
                    .OrderBy(item => item.Key.right.Name).Select(item => item.Key.right).ToList();
                return data;
            }
            else if (workerID > 0)
            {
                var data = context.PermissionJamatkhana.Join(context.Jamatkhana, left => left.JamatkhanaID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.PermissionRegion.CreatedByWorkerID == workerID)
                    .GroupBy(item => new { item.right })
                    .OrderBy(item => item.Key.right.Name).Select(item => item.Key.right).ToList();
                return data;
            }
            return null;
        }
    }

    /// <summary>
    /// interface of PermissionJamatkhana containing necessary database operations
    /// </summary>
    public interface IPermissionJamatkhanaRepository : IBaseRepository<PermissionJamatkhana>
    {
        IQueryable<PermissionJamatkhana> AllIncluding(int permissionId, params Expression<Func<PermissionJamatkhana, object>>[] includeProperties);
        List<PermissionJamatkhana> FindAllByPermissionID(int permissionID);
        List<PermissionJamatkhana> FindAllByPermissionRegionID(int permissionRegionID);
        void InsertOrUpdate(int permissionRegionID, List<int> arraySelectedJamatkhanas);
        List<Jamatkhana> FindAllJamatkhanaByWordkerID(int workerID, int regionID);
    }
}
