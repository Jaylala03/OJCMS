//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseGoalRepository : BaseRepository<CaseGoal>, ICaseGoalRepository
    {
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        private readonly ICaseGoalLivingConditionRepository casegoallivingconditionRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseGoalRepository(RepositoryContext context,
            ICaseGoalLivingConditionRepository casegoallivingconditionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            ,IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(context)
        {
            this.casegoallivingconditionRepository = casegoallivingconditionRepository;
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
        }

        public IQueryable<CaseGoal> AllIncluding(int caseId, params Expression<Func<CaseGoal, object>>[] includeProperties)
        {
            IQueryable<CaseGoal> query = context.CaseGoal;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseMember.CaseID == caseId);
        }

        public IQueryable<CaseGoal> FindAllByCaseMemberID(int casememberID)
        {
            return context.CaseGoal.Where(item => item.CaseMemberID == casememberID);
        }

        /// <summary>
        /// Add or Update caseGoal to database
        /// </summary>
        /// <param name="caseGoal">data to save</param>
        public void InsertOrUpdate(CaseGoal caseGoal, NameValueCollection data)
        {
            bool isNew = false;
            caseGoal.LastUpdateDate = DateTime.Now;
            if (caseGoal.ID == default(int))
            {
                //set the date when this record was created
                caseGoal.CreateDate = caseGoal.LastUpdateDate;
                //set the id of the worker who has created this record
                caseGoal.CreatedByWorkerID = caseGoal.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseGoal.Add(caseGoal);
                isNew = true;
            }
            else
            {
                //update an existing record to database
                context.Entry(caseGoal).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            if (caseGoal.ID > 0)
            {
                List<CaseGoalLivingCondition> existingQOLList = new List<CaseGoalLivingCondition>();
                if (!isNew)
                {
                    existingQOLList = casegoallivingconditionRepository.AllIncluding(caseGoal.ID).ToList();
                    if (existingQOLList == null)
                    {
                        existingQOLList = new List<CaseGoalLivingCondition>();
                    }
                }

                string selectedQOL = caseGoal.QualityOfLifeCategoryIDs;
                selectedQOL = selectedQOL.Replace("false", string.Empty);
                string[] arraySelectedQOL = selectedQOL.ToStringArray(',', true);
                if (arraySelectedQOL != null && arraySelectedQOL.Length > 0)
                {
                    foreach (string qolID in arraySelectedQOL)
                    {
                        if (existingQOLList.Where(item => item.QualityOfLifeCategoryID == qolID.ToInteger(true)).Count() == 0)
                        {
                            CaseGoalLivingCondition newCaseGoalLivingCondition = new CaseGoalLivingCondition()
                            {
                                QualityOfLifeCategoryID = qolID.ToInteger(true),
                                CaseGoalID = caseGoal.ID,
                                LastUpdateDate = DateTime.Now,
                                LastUpdatedByWorkerID = caseGoal.LastUpdatedByWorkerID,
                            };
                            newCaseGoalLivingCondition.Note = data["txtQualityOfLifeCategoryIDs_" + qolID].ToString(true);
                            if (newCaseGoalLivingCondition.Note.IsNullOrEmpty())
                            {
                                newCaseGoalLivingCondition.Note = "N/A";
                            }
                            casegoallivingconditionRepository.InsertOrUpdate(newCaseGoalLivingCondition);
                            casegoallivingconditionRepository.Save();
                        }
                        else
                        {
                            CaseGoalLivingCondition caseAssessmentLivingCondition = casegoallivingconditionRepository.Find(caseGoal.ID, qolID.ToInteger(true));
                            if (caseAssessmentLivingCondition != null)
                            {
                                caseAssessmentLivingCondition.Note = data["txtQualityOfLifeCategoryIDs_" + qolID].ToString(true);
                                if (caseAssessmentLivingCondition.Note.IsNullOrEmpty())
                                {
                                    caseAssessmentLivingCondition.Note = "N/A";
                                }
                                caseAssessmentLivingCondition.LastUpdateDate = DateTime.Now;
                                casegoallivingconditionRepository.InsertOrUpdate(caseAssessmentLivingCondition);
                                casegoallivingconditionRepository.Save();
                            }
                        }
                    }
                }

                foreach (CaseGoalLivingCondition existingQOL in existingQOLList)
                {
                    if (arraySelectedQOL == null || !arraySelectedQOL.Contains(existingQOL.QualityOfLifeCategoryID.ToString(true)))
                    {
                        casegoallivingconditionRepository.Delete(existingQOL);
                        casegoallivingconditionRepository.Save();
                    }
                }
            }
        }

        public DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            if (caseMemberId.HasValue && caseMemberId > 0)
            {
                FilterDescriptor filterDescriptor = new FilterDescriptor("CaseMemberID", FilterOperator.IsEqualTo, caseMemberId.Value);
                dsRequest.Filters.Add(filterDescriptor);
            }
            bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Read, true);
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Delete, true);
            bool hasSetGoalPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoal, Constants.Actions.Create, true);
            bool IsUserAdminWorker = CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1;
            //DataSourceResult dsResult = context.CaseGoal
            //    //.Join(context.CaseWorkerMemberAssignment, left => left.CaseMemberID, right => right.CaseMemberID, (left, right) => new { left, right })
            //    //.Where(item => item.left.CaseMember.CaseID == caseId && item.right.CaseWorker.WorkerID == workerId)
            //    .Where(item => item.CaseMember.CaseID == caseId)
            //    .Where(item => context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(item.CaseMemberID) || workerRoleIDs.Contains("1"))
            //    .OrderByDescending(item => item.CreateDate).ToList()
            //    .Select(
            //    caseGoal => new
            //    {
            //        caseGoal.ID,
            //        caseGoal.CaseMemberID,
            //        CaseMemberName = caseGoal.CaseMember.FirstName + " " + caseGoal.CaseMember.LastName,
            //        caseGoal.StartDate,
            //        caseGoal.EndDate,
            //        caseGoal.WishInLife,
            //        CaseID = caseId,
            //        HasPermissionToEdit = CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1 || hasEditPermission ? "" : "display:none;",
            //        HasPermissionToDelete = hasDeletePermission ? "" : "display:none;",
            //        HasPermissionToCreateSmartGoal = hasSetGoalPermission ? "" : "display:none;"
            //    }
            //    ).ToDataSourceResult(dsRequest);

            List<CaseGoal> caseGoalList = context.CaseGoal
                .Where(item => item.CaseMember.CaseID == caseId)
                //<JL:Comment:06/18/2017>
                //.Where(item => context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(item.CaseMemberID) || IsUserAdminWorker)
                .OrderByDescending(item => item.CreateDate).AsEnumerable().ToList()
                .Select(
                caseGoal => new CaseGoal()
                {
                    ID = caseGoal.ID,
                    CaseMemberID = caseGoal.CaseMemberID,
                    CaseMemberName = caseGoal.CaseMember.FirstName + " " + caseGoal.CaseMember.LastName,
                    StartDate = caseGoal.StartDate,
                    EndDate = caseGoal.EndDate,
                    WishInLife = caseGoal.WishInLife,
                    CaseID = caseId,
                    HasPermissionToRead = IsUserAdminWorker || hasReadPermission ? "" : "display:none;",
                    HasPermissionToEdit = IsUserAdminWorker || hasEditPermission ? "" : "display:none;",
                    HasPermissionToDelete = hasDeletePermission ? "" : "display:none;",
                    HasPermissionToCreateSmartGoal = hasSetGoalPermission ? "" : "display:none;"
                }
                ).ToList();
            if (caseGoalList != null)
            {
                foreach (CaseGoal caseGoal in caseGoalList)
                {
                    List<CaseGoalLivingCondition> qolList = context.CaseGoalLivingCondition.Where(item => item.CaseGoalID == caseGoal.ID).ToList();
                    if (qolList != null)
                    {
                        foreach (CaseGoalLivingCondition qol in qolList)
                        {
                            caseGoal.QualityOfLifeCategoryNames = caseGoal.QualityOfLifeCategoryNames.Concate(",", qol.QualityOfLifeCategory.Name);
                        }
                    }
                }
            }

            return caseGoalList.ToDataSourceResult(dsRequest);
        }

        public override void Delete(int id)
        {
            var entity = Find(id);
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            int smartGoalCount = context.CaseSmartGoal.Where(item => item.CaseGoalID == id).Count();
            if (smartGoalCount > 0)
            {
                throw new CustomException("You can't delete a goal for which Measurable goals are identified.");
            }
            string sqlQuery = @"delete from [CaseGoalLivingCondition] where CaseGoalID=@CaseGoalID;

                                --delete from [CaseAction] where CaseSmartGoalID in (select id from [CaseSmartGoal] where CaseGoalID=@CaseGoalID);
                                DELETE CA FROM [CaseAction] AS CA INNER JOIN [CaseSmartGoal]  CSG ON CA.CaseSmartGoalID = CSG.ID where CaseGoalID=@CaseGoalID;

                                --delete from [CaseAction] where CaseSmartGoalServiceProviderID in (select id from [CaseSmartGoalServiceProvider] where CaseSmartGoalID in (select id from [CaseSmartGoal] where CaseGoalID=@CaseGoalID));
                                DELETE CA FROM [CaseAction] AS CA INNER JOIN [CaseSmartGoalServiceProvider] CSGSP ON CSGSP.ID = CA.CaseSmartGoalServiceProviderID INNER JOIN [CaseSmartGoal] CSG ON CSG.ID = CSGSP.CaseSmartGoalID WHERE CSG.CaseGoalID=@CaseGoalID;
                                
                                --delete from [CaseSmartGoalAssignment] where CaseSmartGoalID in (select id from [CaseSmartGoal] where CaseGoalID=@CaseGoalID);
                                DELETE CSGA FROM [CaseSmartGoalAssignment] AS CSGA INNER JOIN [CaseSmartGoal] CSG ON CSG.ID = CSGA.CaseSmartGoalID WHERE CSG.CaseGoalID=@CaseGoalID;

                                --delete from [CaseSmartGoalProgress] where CaseSmartGoalID in (select id from [CaseSmartGoal] where CaseGoalID=@CaseGoalID);
                                DELETE CSGP FROM [CaseSmartGoalProgress] AS CSGP INNER JOIN [CaseSmartGoal] CSG ON CSG.ID =  CSGP.CaseSmartGoalID WHERE CSG.CaseGoalID = @CaseGoalID;

                                --delete from [CaseSmartGoalServiceLevelOutcome] where CaseSmartGoalID in (select id from [CaseSmartGoal] where CaseGoalID=@CaseGoalID);
                                DELETE CSGPO FROM [CaseSmartGoalServiceLevelOutcome] CSGPO INNER JOIN [CaseSmartGoal] CSG ON CSG.ID = CSGPO.CaseSmartGoalID WHERE CSG.CaseGoalID=@CaseGoalID;

                                --delete from [CaseSmartGoalServiceProvider] where CaseSmartGoalID in (select id from [CaseSmartGoal] where CaseGoalID=@CaseGoalID);
                                DELETE CSGP FROM [CaseSmartGoalServiceProvider] AS CSGP  INNER JOIN [CaseSmartGoal] CSG ON CSG.ID = CSGP.CaseSmartGoalID WHERE CSG.CaseGoalID=@CaseGoalID;

                                delete from [CaseSmartGoal] where CaseGoalID=@CaseGoalID;
                                delete from [CaseGoal] where id=@CaseGoalID;";
            sqlQuery = sqlQuery.Replace("@CaseGoalID", id.ToString());
            context.Database.ExecuteSqlCommand(sqlQuery);

        }
    }

    /// <summary>
    /// interface of CaseGoal containing necessary database operations
    /// </summary>
    public interface ICaseGoalRepository : IBaseRepository<CaseGoal>
    {
        IQueryable<CaseGoal> AllIncluding(int caseId, params Expression<Func<CaseGoal, object>>[] includeProperties);
        IQueryable<CaseGoal> FindAllByCaseMemberID(int casememberID);
        void InsertOrUpdate(CaseGoal caseGoal, NameValueCollection data);
        DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId);
    }
}
