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
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseMemberContactRepository : BaseRepository<CaseMemberContact>, ICaseMemberContactRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseMemberContactRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<CaseMemberContact> AllIncluding(int casememberId, params Expression<Func<CaseMemberContact, object>>[] includeProperties)
        {
            IQueryable<CaseMemberContact> query = context.CaseMemberContact;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseMemberID == casememberId);
        }

        public IQueryable<CaseMemberContact> FindAllByCaseMemberID(int casememberID)
        {
            return context.CaseMemberContact.Where(item => item.CaseMemberID == casememberID);
        }

        /// <summary>
        /// Add or Update casemembercontact to database
        /// </summary>
        /// <param name="casemembercontact">data to save</param>
        public void InsertOrUpdate(CaseMemberContact casemembercontact)
        {
            casemembercontact.LastUpdateDate = DateTime.Now;
            if (casemembercontact.ID == default(int))
            {
                //set the date when this record was created
                casemembercontact.CreateDate = casemembercontact.LastUpdateDate;
                //set the id of the worker who has created this record
                casemembercontact.CreatedByWorkerID = casemembercontact.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseMemberContact.Add(casemembercontact);
            }
            else
            {
                //update an existing record to database
                context.Entry(casemembercontact).State = System.Data.Entity.EntityState.Modified;
            }
        }

    }

    /// <summary>
    /// interface of CaseMemberContact containing necessary database operations
    /// </summary>
    public interface ICaseMemberContactRepository : IBaseRepository<CaseMemberContact>
    {
        IQueryable<CaseMemberContact> AllIncluding(int casememberId, params Expression<Func<CaseMemberContact, object>>[] includeProperties);
        IQueryable<CaseMemberContact> FindAllByCaseMemberID(int casememberID);
        void InsertOrUpdate(CaseMemberContact casemembercontact);
    }
}
