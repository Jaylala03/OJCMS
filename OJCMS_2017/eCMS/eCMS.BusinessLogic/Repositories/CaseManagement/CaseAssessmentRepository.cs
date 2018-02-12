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
using eCMS.DataLogic.Models.Lookup;
using EasySoft.Helper;
using System.Collections.Specialized;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using eCMS.Shared;
using Kendo.Mvc.Extensions;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseAssessmentRepository : BaseRepository<CaseAssessment>, ICaseAssessmentRepository
    {
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        private readonly ICaseAssessmentLivingConditionRepository caseassessmentlivingconditionRepository;
        private readonly ICaseWorkerRepository caseworkerRepository;
        private readonly IWorkerNotificationRepository workernotificationRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseAssessmentRepository(RepositoryContext context,
            ICaseAssessmentLivingConditionRepository caseassessmentlivingconditionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseWorkerRepository caseworkerRepository,
            IWorkerNotificationRepository workernotificationRepository)
            : base(context)
        {
            this.caseassessmentlivingconditionRepository = caseassessmentlivingconditionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.workernotificationRepository = workernotificationRepository;
        }

        public IQueryable<CaseAssessment> AllIncluding(int caseId, params Expression<Func<CaseAssessment, object>>[] includeProperties)
        {
            IQueryable<CaseAssessment> query = context.CaseAssessment;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseMember.CaseID == caseId);
        }

        /// <summary>
        /// Add or Update caseassessment to database
        /// </summary>
        /// <param name="caseassessment">data to save</param>
        public void InsertOrUpdate(CaseAssessment caseassessment, NameValueCollection data)
        {
            bool isNew = false;
            caseassessment.LastUpdateDate = DateTime.Now;
            if (caseassessment.ID == default(int))
            {
                //set the date when this record was created
                caseassessment.CreateDate = caseassessment.LastUpdateDate;
                //set the id of the worker who has created this record
                caseassessment.CreatedByWorkerID = caseassessment.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseAssessment.Add(caseassessment);
                isNew = true;
            }
            else
            {
                //update an existing record to database
                context.Entry(caseassessment).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            if (caseassessment.ID > 0)
            {
                CaseMember existingCaseMember = context.CaseMember.SingleOrDefault(item => item.ID == caseassessment.CaseMemberID);
                if (existingCaseMember != null)
                {
                    try
                    {
                        existingCaseMember.MemberStatusID = caseassessment.MemberStatusID;
                        existingCaseMember.LastUpdateDate = DateTime.Now;
                        existingCaseMember.LastUpdatedByWorkerID = caseassessment.LastUpdatedByWorkerID;
                        context.Entry(existingCaseMember).State = System.Data.Entity.EntityState.Modified;
                        Save();
                    }
                    catch
                    {
                        context.Entry(existingCaseMember).State = System.Data.Entity.EntityState.Detached;
                    }
                }
                List<CaseAssessmentLivingCondition> existingQOLList = new List<CaseAssessmentLivingCondition>();
                if (!isNew)
                {
                    existingQOLList = caseassessmentlivingconditionRepository.FindAllByCaseAssessmentID(caseassessment.ID).ToList();
                    if (existingQOLList == null)
                    {
                        existingQOLList = new List<CaseAssessmentLivingCondition>();
                    }
                }

                string selectedQOL = caseassessment.QualityOfLifeIDs;
                selectedQOL = selectedQOL.Replace("false", string.Empty);
                string[] arraySelectedQOL = selectedQOL.ToStringArray(',', true);
                if (arraySelectedQOL != null && arraySelectedQOL.Length > 0)
                {
                    foreach (string qolID in arraySelectedQOL)
                    {
                        if (existingQOLList.Where(item => item.QualityOfLifeID == qolID.ToInteger(true)).Count() == 0)
                        {
                            CaseAssessmentLivingCondition newCaseAssessmentLivingCondition = new CaseAssessmentLivingCondition()
                            {
                                QualityOfLifeID = qolID.ToInteger(true),
                                CaseAssessmentID = caseassessment.ID,
                                LastUpdateDate = DateTime.Now,
                                LastUpdatedByWorkerID = caseassessment.LastUpdatedByWorkerID,
                            };
                            newCaseAssessmentLivingCondition.Note = data["txtQualityOfLifeIDs" + qolID].ToString(true);
                            caseassessmentlivingconditionRepository.InsertOrUpdate(newCaseAssessmentLivingCondition);
                            caseassessmentlivingconditionRepository.Save();
                        }
                        else
                        {
                            CaseAssessmentLivingCondition caseAssessmentLivingCondition = caseassessmentlivingconditionRepository.Find(caseassessment.ID, qolID.ToInteger(true));
                            if (caseAssessmentLivingCondition != null)
                            {
                                caseAssessmentLivingCondition.Note = data["txtQualityOfLifeIDs" + qolID].ToString(true);
                                caseAssessmentLivingCondition.LastUpdateDate = DateTime.Now;
                                caseassessmentlivingconditionRepository.InsertOrUpdate(caseAssessmentLivingCondition);
                                caseassessmentlivingconditionRepository.Save();
                            }
                        }
                    }
                }

                foreach (CaseAssessmentLivingCondition existingQOL in existingQOLList)
                {
                    if (arraySelectedQOL == null || !arraySelectedQOL.Contains(existingQOL.QualityOfLifeID.ToString(true)))
                    {
                        caseassessmentlivingconditionRepository.Delete(existingQOL);
                        caseassessmentlivingconditionRepository.Save();
                    }
                }

                CaseWorker primaryWorker = caseworkerRepository.FindPrimary(caseassessment.CaseID);
                if (primaryWorker != null)
                {
                    string caseLink = "/CaseManagement/CaseAssessment?CaseID=" + caseassessment.CaseID + "&CaseMemberID=" + caseassessment.CaseMemberID;
                    WorkerNotification workerNotification = new WorkerNotification()
                    {
                        IsRead = false,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = caseassessment.LastUpdatedByWorkerID,
                        ReferenceLink = caseLink,
                        WorkerID = primaryWorker.WorkerID
                    };
                    if (isNew)
                    {
                        workerNotification.Notification = "A new assessment has been added to a case. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the assessment detail.";
                    }
                    else
                    {
                        workerNotification.Notification = "An assessment has been updated. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the assessment detail.";
                    }
                    workernotificationRepository.InsertOrUpdate(workerNotification);
                    workernotificationRepository.Save();
                }
            }
        }

        public List<CaseAssessment> FindAllByCaseMemberID(int caseMemberID)
        {
            return context.CaseAssessment.Where(item => item.CaseMemberID == caseMemberID).OrderBy(item=>item.StartDate).ToList();
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
            bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Read, true);
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Delete, true);
            bool IsUserAdminWorker = CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1;
            bool IsUserRegionalManager = CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1;

            List<CaseAssessment> caseAssessmentList = context.CaseAssessment
                //.Join(context.CaseWorkerMemberAssignment, left => left.CaseMemberID, right => right.CaseMemberID, (left, right) => new { left, right })
                //.Where(item => item.left.CaseMember.CaseID == caseId && item.right.CaseWorker.WorkerID == workerId)
                .Where(item => item.CaseMember.CaseID == caseId)
                //<JL:Comment:06/18/2017>
                //.Where(item => context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(item.CaseMemberID) || IsUserAdminWorker)
                .OrderByDescending(item => item.CreateDate).AsEnumerable().ToList()
                .Select(
                caseassessment => new CaseAssessment()
                {
                    ID = caseassessment.ID,
                    CaseMemberID = caseassessment.CaseMemberID,
                    AssessmentTypeID = caseassessment.AssessmentTypeID,
                    AssessmentTypeName = caseassessment.AssessmentType != null ? caseassessment.AssessmentType.Name : "",
                    MemberStatusID = caseassessment.MemberStatusID,
                    DocumentedByID = caseassessment.DocumentedByID,
                    DocumentedByName = caseassessment.DocumentedBy != null && caseassessment.DocumentedBy.Worker != null ? caseassessment.DocumentedBy.Worker.FirstName + " " + caseassessment.DocumentedBy.Worker.LastName : "",
                    CaseMemberName = caseassessment.CaseMember != null ? caseassessment.CaseMember.FirstName + " " + caseassessment.CaseMember.LastName : string.Empty,
                    StartDate = caseassessment.StartDate,
                    EndDate = caseassessment.EndDate,
                    ReasonsForDischargeName = caseassessment.ReasonsForDischarge != null ? caseassessment.ReasonsForDischarge.Name : "",
                    CaseID = caseId,
                    HasPermissionToRead = hasReadPermission ? "" : "display:none;",
                    //HasPermissionToEdit = IsUserAdminWorker || (caseassessment.AssessmentTypeID != 2 && hasEditPermission) || IsUserRegionalManager ? "" : "display:none;",
                    HasPermissionToEdit = IsUserAdminWorker || hasEditPermission || IsUserRegionalManager ? "" : "display:none;",
                    HasPermissionToDelete = hasDeletePermission ? "" : "display:none;"
                }
                ).ToList();
            if (caseAssessmentList != null)
            {
                foreach (CaseAssessment caseAssessment in caseAssessmentList)
                {
                    caseAssessment.QualityOfLifeNames = string.Join(",", context.CaseAssessmentLivingCondition.Where(item => item.CaseAssessmentID == caseAssessment.ID).Select(item => item.QualityOfLife.Name));
                }
            }
            DataSourceResult dsResult = caseAssessmentList.ToDataSourceResult(dsRequest);
            return dsResult;
        }
    }

    /// <summary>
    /// interface of CaseAssessment containing necessary database operations
    /// </summary>
    public interface ICaseAssessmentRepository : IBaseRepository<CaseAssessment>
    {
        IQueryable<CaseAssessment> AllIncluding(int caseId, params Expression<Func<CaseAssessment, object>>[] includeProperties);
        void InsertOrUpdate(CaseAssessment caseassessment,NameValueCollection data);
        List<CaseAssessment> FindAllByCaseMemberID(int caseMemberID);
        DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId);
    }
}
