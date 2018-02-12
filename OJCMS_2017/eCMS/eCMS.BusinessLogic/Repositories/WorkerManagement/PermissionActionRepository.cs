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
    public class PermissionActionRepository : BaseRepository<PermissionAction>, IPermissionActionRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public PermissionActionRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<PermissionAction> AllIncluding(int permissionID, params Expression<Func<PermissionAction, object>>[] includeProperties)
        {
            IQueryable<PermissionAction> query = context.PermissionAction;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.PermissionID == permissionID);
        }

        public List<PermissionAction> FindAllByPermissionID(int permissionID)
        {
            return context.PermissionAction.Where(item => item.PermissionID == permissionID).ToList();
        }

        /// <summary>
        /// Add or Update permissionSubProgram to database
        /// </summary>
        /// <param name="permissionSubProgram">data to save</param>
        public void InsertOrUpdate(int permissionID, string[] arraySelectedActionMethods)
        {
            List<PermissionAction> assignment = context.PermissionAction.Where(item => item.PermissionID == permissionID).ToList();
            if (arraySelectedActionMethods != null && arraySelectedActionMethods.Length > 0)
            {
                foreach (string actionMethodID in arraySelectedActionMethods)
                {
                    if (assignment.Where(item => item.ActionMethodID == actionMethodID.ToInteger(true)).Count() == 0)
                    {
                        PermissionAction newActionPermission = new PermissionAction()
                        {
                            PermissionID = permissionID,
                            ActionMethodID = actionMethodID.ToInteger(true),
                            LastUpdateDate = DateTime.Now,
                            LastUpdatedByWorkerID = CurrentLoggedInWorker.ID,
                            CreatedByWorkerID = CurrentLoggedInWorker.ID
                        };
                        InsertOrUpdate(newActionPermission);
                        Save();
                    }
                }
            }

            foreach (PermissionAction existingMember in assignment)
            {
                if (arraySelectedActionMethods == null || !arraySelectedActionMethods.Contains(existingMember.ActionMethodID.ToString(true)))
                {
                    Delete(existingMember);
                    Save();
                }
            }
        }

        public override PermissionAction Find(int id)
        {
            return context.PermissionAction.SingleOrDefault(item => item.ID == id);
        }

    }

    /// <summary>
    /// interface of PermissionAction containing necessary database operations
    /// </summary>
    public interface IPermissionActionRepository : IBaseRepository<PermissionAction>
    {
        IQueryable<PermissionAction> AllIncluding(int permissionId, params Expression<Func<PermissionAction, object>>[] includeProperties);
        List<PermissionAction> FindAllByPermissionID(int permissionID);
        void InsertOrUpdate(int permissionID, string[] arraySelectedActionMethods);
    }
}
