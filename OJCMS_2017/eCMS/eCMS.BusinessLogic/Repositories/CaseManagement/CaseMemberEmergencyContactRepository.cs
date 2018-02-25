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
    public class CaseMemberEmergencyContactRepository : BaseRepository<CaseMemberEmergencyContact>, ICaseMemberEmergencyContactRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseMemberEmergencyContactRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<CaseMemberEmergencyContact> AllIncluding(int CaseMemberEmergencyId, params Expression<Func<CaseMemberEmergencyContact, object>>[] includeProperties)
        {
            IQueryable<CaseMemberEmergencyContact> query = context.CaseMemberEmergencyContact;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            //return query.Where(item => item.ID == CaseMemberEmergencyId);
            return query;
        }

        public IQueryable<CaseMemberEmergencyContact> FindAllByCaseMemberID(int CaseMemberID)
        {
            return context.CaseMemberEmergencyContact.Where(item => item.CaseMemberID == CaseMemberID);
        }

        /// <summary>
        /// Add or Update CaseMemberEmergencyContact to database
        /// </summary>
        /// <param name="CaseMemberEmergencycontact">data to save</param>
        public void InsertOrUpdate(CaseMemberEmergencyContact CaseMemberEmergencycontact)
        {
            CaseMemberEmergencycontact.LastUpdateDate = DateTime.Now;
            if (CaseMemberEmergencycontact.ID == default(int))
            {
                //set the date when this record was created
                CaseMemberEmergencycontact.CreateDate = CaseMemberEmergencycontact.LastUpdateDate;
                //set the id of the worker who has created this record
                CaseMemberEmergencycontact.CreatedByWorkerID = CaseMemberEmergencycontact.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseMemberEmergencyContact.Add(CaseMemberEmergencycontact);
            }
            else
            {
                //update an existing record to database
                context.Entry(CaseMemberEmergencycontact).State = System.Data.Entity.EntityState.Modified;
            }
        }

    }

    /// <summary>
    /// interface of CaseMemberEmergencyContact containing necessary database operations
    /// </summary>
    public interface ICaseMemberEmergencyContactRepository : IBaseRepository<CaseMemberEmergencyContact>
    {
        IQueryable<CaseMemberEmergencyContact> AllIncluding(int CaseMemberEmergencyId, params Expression<Func<CaseMemberEmergencyContact, object>>[] includeProperties);
        IQueryable<CaseMemberEmergencyContact> FindAllByCaseMemberID(int CaseMemberID);
        void InsertOrUpdate(CaseMemberEmergencyContact CaseMemberEmergencycontact);
    }
}
