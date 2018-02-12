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
    public class CaseAssessmentLivingConditionRepository : BaseRepository<CaseAssessmentLivingCondition>, ICaseAssessmentLivingConditionRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseAssessmentLivingConditionRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<CaseAssessmentLivingCondition> AllIncluding(int caseassessmentId, params Expression<Func<CaseAssessmentLivingCondition, object>>[] includeProperties)
        {
            IQueryable<CaseAssessmentLivingCondition> query = context.CaseAssessmentLivingCondition;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseAssessmentID == caseassessmentId);
        }

        public IQueryable<CaseAssessmentLivingCondition> FindAllByCaseAssessmentID(int caseassessmentID)
        {
            return context.CaseAssessmentLivingCondition.Where(item => item.CaseAssessmentID == caseassessmentID);
        }

        public IQueryable<CaseAssessmentLivingCondition> FindAllByCaseAssessmentIDAndQualityOfLifeCategoryID(int caseassessmentID, int qualityOfLifeCategoryID)
        {
            return context.CaseAssessmentLivingCondition.Where(item => item.CaseAssessmentID == caseassessmentID && item.QualityOfLife.QualityOfLifeSubCategory.QualityOfLifeCategoryID == qualityOfLifeCategoryID);
        }

        /// <summary>
        /// Add or Update caseassessmentlivingcondition to database
        /// </summary>
        /// <param name="caseassessmentlivingcondition">data to save</param>
        public void InsertOrUpdate(CaseAssessmentLivingCondition caseassessmentlivingcondition)
        {
            caseassessmentlivingcondition.LastUpdateDate = DateTime.Now;
            if (caseassessmentlivingcondition.ID == default(int))
            {
                //set the date when this record was created
                caseassessmentlivingcondition.CreateDate = caseassessmentlivingcondition.LastUpdateDate;
                //set the id of the worker who has created this record
                caseassessmentlivingcondition.CreatedByWorkerID = caseassessmentlivingcondition.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseAssessmentLivingCondition.Add(caseassessmentlivingcondition);
            }
            else
            {
                //update an existing record to database
                context.Entry(caseassessmentlivingcondition).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public CaseAssessmentLivingCondition Find(int caseassessmentId, int qolId)
        {
            return context.CaseAssessmentLivingCondition.SingleOrDefault(item=>item.CaseAssessmentID==caseassessmentId && item.QualityOfLifeID==qolId);
        }

        public List<CaseAssessmentLivingCondition> FindAllByCaseMemberID(int casememberID)
        {
            return context.CaseAssessmentLivingCondition.Where(item => item.CaseAssessment.CaseMemberID == casememberID).AsEnumerable().ToList().
                Select(item => new CaseAssessmentLivingCondition()
                {
                    CaseAssessmentID = item.CaseAssessmentID,
                    AssessmentStartDate=item.CaseAssessment.StartDate,
                    QualityOfLifeName=item.QualityOfLife.Name,
                    QualityOfLifeID=item.QualityOfLifeID,
                    QualityOfLifeSubCategoryName=item.QualityOfLife.QualityOfLifeSubCategory.Name,
                    QualityOfLifeCategoryID=item.QualityOfLife.QualityOfLifeSubCategory.QualityOfLifeCategoryID,
                    Note=item.Note
                }).ToList();
        }
    }

    /// <summary>
    /// interface of CaseAssessmentLivingCondition containing necessary database operations
    /// </summary>
    public interface ICaseAssessmentLivingConditionRepository : IBaseRepository<CaseAssessmentLivingCondition>
    {
        IQueryable<CaseAssessmentLivingCondition> AllIncluding(int caseassessmentId, params Expression<Func<CaseAssessmentLivingCondition, object>>[] includeProperties);
        IQueryable<CaseAssessmentLivingCondition> FindAllByCaseAssessmentID(int caseassessmentID);
        List<CaseAssessmentLivingCondition> FindAllByCaseMemberID(int casememberID);
        IQueryable<CaseAssessmentLivingCondition> FindAllByCaseAssessmentIDAndQualityOfLifeCategoryID(int caseassessmentID, int qualityOfLifeCategoryID);
        void InsertOrUpdate(CaseAssessmentLivingCondition caseassessmentlivingcondition);
        CaseAssessmentLivingCondition Find(int caseassessmentId, int qolId);
    }
}
