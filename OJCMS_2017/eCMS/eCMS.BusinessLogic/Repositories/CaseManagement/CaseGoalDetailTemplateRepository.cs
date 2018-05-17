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
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseGoalDetailTemplateRepository : BaseRepository<CaseGoalDetailTemplate>, ICaseGoalDetailTemplateRepository
    {

        public CaseGoalDetailTemplateRepository(RepositoryContext context)
            : base(context)
        {
        }

        public void InsertOrUpdate(CaseGoalDetailTemplate template)
        {
            template.LastUpdateDate = DateTime.Now;
            if (template.ID == default(int))
            {
                //set the date when this record was created
                template.CreatedByWorkerID = template.LastUpdatedByWorkerID;
                template.CreateDate = template.LastUpdateDate;
                //add a new record to database
                context.CaseGoalDetailTemplate.Add(template);
            }
            else
            {
                template.CreatedByWorkerID = template.LastUpdatedByWorkerID;
                template.CreateDate = template.LastUpdateDate;
                //update an existing record to database
                context.Entry(template).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<CaseGoalDetailTemplate> GetByIndicatorType()
        {
            //return context.CaseGoalDetailTemplate.Where(a => !a.IsArchived && a.IndicatorTypeID == IndicatorTypeID).ToList();
            return context.CaseGoalDetailTemplate.Join(context.IndicatorType, left => left.IndicatorTypeID, right => right.ID, (left, right) => new { left, right }).Where(item => !item.left.IsArchived).AsEnumerable().Select(item => new CaseGoalDetailTemplate
            {
                ID = item.left.ID,
                Name = item.left.Name,
                Description = item.left.Description,
                IndicatorTypeID = item.left.IndicatorTypeID,
                IndicatorTypeName = item.right.Name
            }).ToList();
            //return context.CaseMember.Join(context.CaseWorkerMemberAssignment, left => left.ID, right => right.CaseMemberID, (left, right) => new { left, right }).Where(item => item.left.CaseID == caseID && item.right.CaseWorker.WorkerID == workerID).AsEnumerable().Select(item => new SelectListItem { Value = item.left.ID.ToString(), Text = item.left.FirstName + " " + item.left.LastName }).ToList();
        }
    }

    public interface ICaseGoalDetailTemplateRepository : IBaseRepository<CaseGoalDetailTemplate>
    {
        void InsertOrUpdate(CaseGoalDetailTemplate template);
        List<CaseGoalDetailTemplate> GetByIndicatorType();
    }

}
