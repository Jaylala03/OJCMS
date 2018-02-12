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
    public class CaseGoalLivingConditionRepository : BaseRepository<CaseGoalLivingCondition>, ICaseGoalLivingConditionRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseGoalLivingConditionRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<CaseGoalLivingCondition> AllIncluding(int caseGoalId, params Expression<Func<CaseGoalLivingCondition, object>>[] includeProperties)
        {
            IQueryable<CaseGoalLivingCondition> query = context.CaseGoalLivingCondition;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseGoalID == caseGoalId);
        }
        
        /// <summary>
        /// Add or Update casegoallivingcondition to database
        /// </summary>
        /// <param name="casegoallivingcondition">data to save</param>
        public void InsertOrUpdate(CaseGoalLivingCondition casegoallivingcondition)
        {
            casegoallivingcondition.LastUpdateDate = DateTime.Now;
            if (casegoallivingcondition.ID == default(int))
            {
                //set the date when this record was created
                casegoallivingcondition.CreateDate = casegoallivingcondition.LastUpdateDate;
                //set the id of the worker who has created this record
                casegoallivingcondition.CreatedByWorkerID = casegoallivingcondition.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseGoalLivingCondition.Add(casegoallivingcondition);
            }
            else
            {
                //update an existing record to database
                context.Entry(casegoallivingcondition).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public CaseGoalLivingCondition Find(int caseGoalId, int qolID)
        {
            return context.CaseGoalLivingCondition.SingleOrDefault(item => item.CaseGoalID == caseGoalId && item.QualityOfLifeCategoryID == qolID);
        }
    }

    /// <summary>
    /// interface of CaseGoalLivingCondition containing necessary database operations
    /// </summary>
    public interface ICaseGoalLivingConditionRepository : IBaseRepository<CaseGoalLivingCondition>
    {
        IQueryable<CaseGoalLivingCondition> AllIncluding(int caseGoalId, params Expression<Func<CaseGoalLivingCondition, object>>[] includeProperties);
        void InsertOrUpdate(CaseGoalLivingCondition casegoallivingcondition);
        CaseGoalLivingCondition Find(int caseGoalId, int qolID);
    }
}
