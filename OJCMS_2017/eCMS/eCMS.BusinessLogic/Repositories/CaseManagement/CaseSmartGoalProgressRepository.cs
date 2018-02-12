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
    public class CaseSmartGoalProgressRepository : BaseRepository<CaseSmartGoalProgress>, ICaseSmartGoalProgressRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseSmartGoalProgressRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<CaseSmartGoalProgress> AllIncluding(int casesmartgoalId, params Expression<Func<CaseSmartGoalProgress, object>>[] includeProperties)
        {
            IQueryable<CaseSmartGoalProgress> query = context.CaseSmartGoalProgress;
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
        /// Add or Update casesmartgoalprogress to database
        /// </summary>
        /// <param name="casesmartgoalprogress">data to save</param>
        public void InsertOrUpdate(CaseSmartGoalProgress casesmartgoalprogress)
        {
            casesmartgoalprogress.LastUpdateDate = DateTime.Now;
            if (casesmartgoalprogress.ID == default(int))
            {
                //set the date when this record was created
                casesmartgoalprogress.CreateDate = casesmartgoalprogress.LastUpdateDate;
                //set the id of the worker who has created this record
                casesmartgoalprogress.CreatedByWorkerID = casesmartgoalprogress.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseSmartGoalProgress.Add(casesmartgoalprogress);
            }
            else
            {
                //update an existing record to database
                context.Entry(casesmartgoalprogress).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            if (casesmartgoalprogress.ID > 0)
            {
                CaseSmartGoal caseSmartGoal = context.CaseSmartGoal.SingleOrDefault(item => item.ID == casesmartgoalprogress.CaseSmartGoalID);
                if (caseSmartGoal != null && caseSmartGoal.ServiceLevelOutcomeID != casesmartgoalprogress.ServiceLevelOutcomeID)
                {
                    caseSmartGoal.ServiceLevelOutcomeID = casesmartgoalprogress.ServiceLevelOutcomeID;
                    caseSmartGoal.LastUpdateDate = DateTime.Today;
                    caseSmartGoal.LastUpdatedByWorkerID = casesmartgoalprogress.LastUpdatedByWorkerID;
                    context.Entry(caseSmartGoal).State = System.Data.Entity.EntityState.Modified;
                    Save();
                }
            }
        }

        public CaseSmartGoalAssignment FindCaseSmartGoalAssignment(int SmartGoalID, int CaseSmartGoalID)
        {
            return context.CaseSmartGoalAssignment.FirstOrDefault(item=>item.SmartGoalID==SmartGoalID && item.CaseSmartGoalID==CaseSmartGoalID);
        }

    }

    /// <summary>
    /// interface of CaseSmartGoalProgress containing necessary database operations
    /// </summary>
    public interface ICaseSmartGoalProgressRepository : IBaseRepository<CaseSmartGoalProgress>
    {
        IQueryable<CaseSmartGoalProgress> AllIncluding(int casesmartgoalId, params Expression<Func<CaseSmartGoalProgress, object>>[] includeProperties);
        void InsertOrUpdate(CaseSmartGoalProgress casesmartgoalprogress);
        CaseSmartGoalAssignment FindCaseSmartGoalAssignment(int SmartGoalID, int CaseSmartGoalID);
    }
}
