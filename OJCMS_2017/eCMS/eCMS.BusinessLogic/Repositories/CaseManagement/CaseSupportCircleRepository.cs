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

namespace eCMS.BusinessLogic.Repositories
{ 
	/// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseSupportCircleRepository : BaseRepository<CaseSupportCircle>, ICaseSupportCircleRepository
    {
		/// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
		public CaseSupportCircleRepository(RepositoryContext context)
			:base(context)
        {
        }

                public IQueryable<CaseSupportCircle> AllIncluding(int caseId,params Expression<Func<CaseSupportCircle, object>>[] includeProperties)
        {
            IQueryable<CaseSupportCircle> query = context.CaseSupportCircle;
            if(includeProperties!=null)
			{
				foreach (var includeProperty in includeProperties) 
				{
					query = query.Include(includeProperty);
				}
			}
            return query.Where(item=>item.CaseID==caseId);
        }
		
				
		public IQueryable<CaseSupportCircle> FindAllByCreatedByWorkerID(int createdbyworkerID )
        {
            return context.CaseSupportCircle.Where(item => item.CreatedByWorkerID == createdbyworkerID);
        }
				
		public IQueryable<CaseSupportCircle> FindAllByLastUpdatedByWorkerID(int lastupdatedbyworkerID )
        {
            return context.CaseSupportCircle.Where(item => item.LastUpdatedByWorkerID == lastupdatedbyworkerID);
        }
				
		public IQueryable<CaseSupportCircle> FindAllByCaseID(int caseID )
        {
            return context.CaseSupportCircle.Where(item => item.CaseID == caseID);
        }
				
				public IQueryable<CaseSupportCircle> FindAllByCreatedByWorkerIDAndLastUpdatedByWorkerID(int createdbyworkerID, int lastupdatedbyworkerID)
		{
		return context.CaseSupportCircle.Where( item =>  item.CreatedByWorkerID == createdbyworkerID && item.LastUpdatedByWorkerID == lastupdatedbyworkerID);
		}
				public IQueryable<CaseSupportCircle> FindAllByCreatedByWorkerIDAndLastUpdatedByWorkerIDAndCaseID(int createdbyworkerID, int lastupdatedbyworkerID, int caseID)
		{
		return context.CaseSupportCircle.Where( item =>  item.CreatedByWorkerID == createdbyworkerID && item.LastUpdatedByWorkerID == lastupdatedbyworkerID && item.CaseID == caseID);
		}
			

		/// <summary>
        /// Add or Update casesupportcircle to database
        /// </summary>
        /// <param name="casesupportcircle">data to save</param>
        public void InsertOrUpdate(CaseSupportCircle casesupportcircle)
        {
            casesupportcircle.LastUpdateDate = DateTime.Now;
            if (casesupportcircle.ID == default(int)) {
				//set the date when this record was created
				casesupportcircle.CreateDate = casesupportcircle.LastUpdateDate;
				//set the id of the worker who has created this record
                casesupportcircle.CreatedByWorkerID = casesupportcircle.LastUpdatedByWorkerID;
				//add a new record to database
                context.CaseSupportCircle.Add(casesupportcircle);
            } else {
				//update an existing record to database
                context.Entry(casesupportcircle).State = System.Data.Entity.EntityState.Modified;
            }
        }

    }

	/// <summary>
    /// interface of CaseSupportCircle containing necessary database operations
    /// </summary>
    public interface ICaseSupportCircleRepository : IBaseRepository<CaseSupportCircle>
    {
                IQueryable<CaseSupportCircle> AllIncluding(int caseId,params Expression<Func<CaseSupportCircle, object>>[] includeProperties);
                		IQueryable<CaseSupportCircle> FindAllByCreatedByWorkerID(int createdbyworkerID );
				IQueryable<CaseSupportCircle> FindAllByLastUpdatedByWorkerID(int lastupdatedbyworkerID );
				IQueryable<CaseSupportCircle> FindAllByCaseID(int caseID );
						IQueryable<CaseSupportCircle> FindAllByCreatedByWorkerIDAndLastUpdatedByWorkerID(int createdbyworkerID, int lastupdatedbyworkerID);
				IQueryable<CaseSupportCircle> FindAllByCreatedByWorkerIDAndLastUpdatedByWorkerIDAndCaseID(int createdbyworkerID, int lastupdatedbyworkerID, int caseID);
				void InsertOrUpdate(CaseSupportCircle casesupportcircle);
    }
}
