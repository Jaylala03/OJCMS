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
using EasySoft.Helper;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseSmartGoalServiceLevelOutcomeRepository : BaseRepository<CaseSmartGoalServiceLevelOutcome>, ICaseSmartGoalServiceLevelOutcomeRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseSmartGoalServiceLevelOutcomeRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<CaseSmartGoalServiceLevelOutcome> AllIncluding(int casesmartgoalId, params Expression<Func<CaseSmartGoalServiceLevelOutcome, object>>[] includeProperties)
        {
            IQueryable<CaseSmartGoalServiceLevelOutcome> query = context.CaseSmartGoalServiceLevelOutcome;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseSmartGoalID == casesmartgoalId);
        }

        /// <summary>
        /// Add or Update casesmartgoalserviceleveloutcome to database
        /// </summary>
        /// <param name="casesmartgoalserviceleveloutcome">data to save</param>
        public void InsertOrUpdate(CaseSmartGoalServiceLevelOutcome casesmartgoalserviceleveloutcome)
        {
            casesmartgoalserviceleveloutcome.LastUpdateDate = DateTime.Now;
            if (casesmartgoalserviceleveloutcome.ID == default(int))
            {
                //set the date when this record was created
                casesmartgoalserviceleveloutcome.CreateDate = casesmartgoalserviceleveloutcome.LastUpdateDate;
                //set the id of the worker who has created this record
                casesmartgoalserviceleveloutcome.CreatedByWorkerID = casesmartgoalserviceleveloutcome.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseSmartGoalServiceLevelOutcome.Add(casesmartgoalserviceleveloutcome);
            }
            else
            {
                //update an existing record to database
                context.Entry(casesmartgoalserviceleveloutcome).State = System.Data.Entity.EntityState.Modified;
            }
        }

    }

    /// <summary>
    /// interface of CaseSmartGoalServiceLevelOutcome containing necessary database operations
    /// </summary>
    public interface ICaseSmartGoalServiceLevelOutcomeRepository : IBaseRepository<CaseSmartGoalServiceLevelOutcome>
    {
        IQueryable<CaseSmartGoalServiceLevelOutcome> AllIncluding(int casesmartgoalId, params Expression<Func<CaseSmartGoalServiceLevelOutcome, object>>[] includeProperties);
        void InsertOrUpdate(CaseSmartGoalServiceLevelOutcome casesmartgoalserviceleveloutcome);
    }
}
