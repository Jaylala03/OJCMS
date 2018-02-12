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
using Kendo.Mvc.UI;
using Kendo.Mvc;
using eCMS.Shared;
using Kendo.Mvc.Extensions;
using eCMS.DataLogic.Models.Lookup;
using EasySoft.Helper;
using eCMS.ExceptionLoging;
using System.Web.Mvc;
using System.Collections.Specialized;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseSmartGoalRepository : BaseRepository<CaseSmartGoal>, ICaseSmartGoalRepository
    {
        private readonly ICaseSmartGoalServiceProviderRepository casesmartgoalserviceproviderRepository;
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        private readonly ICaseActionRepository caseactionRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseSmartGoalRepository(RepositoryContext context,
            ICaseSmartGoalServiceProviderRepository casesmartgoalserviceproviderRepository,
            ICaseActionRepository caseactionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            ,IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(context)
        {
            this.casesmartgoalserviceproviderRepository = casesmartgoalserviceproviderRepository;
            this.caseactionRepository = caseactionRepository;
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
        }

        public IQueryable<CaseSmartGoal> AllIncluding(int caseId, params Expression<Func<CaseSmartGoal, object>>[] includeProperties)
        {
            IQueryable<CaseSmartGoal> query = context.CaseSmartGoal;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseGoal.CaseMember.CaseID == caseId);
        }

        public IQueryable<CaseSmartGoal> FindAllByCaseMemberID(int casememberID)
        {
            return context.CaseSmartGoal.Where(item => item.CaseGoal.CaseMemberID == casememberID);
        }

       

        /// <summary>
        /// Add or Update casesmartgoal to database
        /// </summary>
        /// <param name="casesmartgoal">data to save</param>
        public void InsertOrUpdate(CaseSmartGoal casesmartgoal, NameValueCollection data, bool endOnly = false)
        {
            bool isNew = false;
            casesmartgoal.LastUpdateDate = DateTime.Now;
            if (casesmartgoal.ID == default(int))
            {
                //set the date when this record was created
                casesmartgoal.CreateDate = casesmartgoal.LastUpdateDate;
                //set the id of the worker who has created this record
                casesmartgoal.CreatedByWorkerID = casesmartgoal.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseSmartGoal.Add(casesmartgoal);
                isNew = true;
            }
            else
            {
                //update an existing record to database
                context.Entry(casesmartgoal).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            if (endOnly)
            {
                return;
            }
            if (casesmartgoal.ID > 0)
            {
                List<CaseSmartGoalServiceProvider> existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                if (!isNew)
                {
                    existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                    if (existingServiceProviderList == null)
                    {
                        existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                    }
                }
                else
                {
                    //add progress level outcome
                    CaseSmartGoalProgress newCaseSmartGoalProgress = new CaseSmartGoalProgress()
                    {
                        CaseSmartGoalID = casesmartgoal.ID,
                        Comment = "Initial Outcome",
                        CreateDate = DateTime.Now,
                        CreatedByWorkerID = casesmartgoal.CreatedByWorkerID,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = casesmartgoal.LastUpdatedByWorkerID,
                        ProgressDate = DateTime.Now,
                        ServiceLevelOutcomeID = casesmartgoal.ServiceLevelOutcomeID,
                    };
                    context.CaseSmartGoalProgress.Add(newCaseSmartGoalProgress);
                    context.SaveChanges();
                }
                bool isAdded = false;
                string selectedUsedInternalServiceProvider = casesmartgoal.UsedInternalServiceProviderIDs;
                selectedUsedInternalServiceProvider = selectedUsedInternalServiceProvider.ToString(true).Replace("false", string.Empty);
                string[] arrayselectedUsedInternalServiceProvider = selectedUsedInternalServiceProvider.ToStringArray(',', true);
                if (arrayselectedUsedInternalServiceProvider != null && arrayselectedUsedInternalServiceProvider.Length > 0)
                {
                    foreach (string qolID in arrayselectedUsedInternalServiceProvider)
                    {
                        if (existingServiceProviderList.Where(item => item.ServiceProviderID == qolID.ToInteger(true)).Count() == 0)
                        {
                            CaseSmartGoalServiceProvider newCaseSmartGoalInternalService = new CaseSmartGoalServiceProvider()
                            {
                                ServiceProviderID = qolID.ToInteger(true),
                                CaseSmartGoalID = casesmartgoal.ID,
                                LastUpdateDate = DateTime.Now,
                                LastUpdatedByWorkerID = casesmartgoal.LastUpdatedByWorkerID,
                                IsUsed = true
                            };
                            casesmartgoalserviceproviderRepository.InsertOrUpdate(newCaseSmartGoalInternalService);
                            casesmartgoalserviceproviderRepository.Save();
                            isAdded = true;
                        }
                    }
                    if (isAdded)
                    {
                        existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                        if (existingServiceProviderList == null)
                        {
                            existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                        }
                    }
                }

                bool isDeleted = false;
                foreach (CaseSmartGoalServiceProvider existingUsedInternalServiceProvider in existingServiceProviderList.Where(item => item.IsUsed == true && item.ServiceProvider!=null && item.ServiceProvider.IsExternal == false))
                {
                    if (arrayselectedUsedInternalServiceProvider == null || !arrayselectedUsedInternalServiceProvider.Contains(existingUsedInternalServiceProvider.ServiceProviderID.ToString(true)))
                    {

                        string sqlQuery = @"delete from [CaseAction] where CaseSmartGoalServiceProviderID=" + existingUsedInternalServiceProvider.ID + ";";                     
                        context.Database.ExecuteSqlCommand(sqlQuery);
                        casesmartgoalserviceproviderRepository.Delete(existingUsedInternalServiceProvider);
                        casesmartgoalserviceproviderRepository.Save();
                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                    if (existingServiceProviderList == null)
                    {
                        existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                    }
                }
                isAdded = false;
                string selectedProposedInternalServiceProvider = casesmartgoal.ProposedInternalServiceProviderIDs;
                selectedProposedInternalServiceProvider = selectedProposedInternalServiceProvider.ToString(true).Replace("false", string.Empty);
                string[] arrayselectedProposedInternalServiceProvider = selectedProposedInternalServiceProvider.ToStringArray(',', true);
                if (arrayselectedProposedInternalServiceProvider != null && arrayselectedProposedInternalServiceProvider.Length > 0)
                {
                    foreach (string qolID in arrayselectedProposedInternalServiceProvider)
                    {
                        if (existingServiceProviderList.Where(item => item.ServiceProviderID == qolID.ToInteger(true)).Count() == 0)
                        {
                            CaseSmartGoalServiceProvider newCaseSmartGoalInternalService = new CaseSmartGoalServiceProvider()
                            {
                                ServiceProviderID = qolID.ToInteger(true),
                                CaseSmartGoalID = casesmartgoal.ID,
                                LastUpdateDate = DateTime.Now,
                                LastUpdatedByWorkerID = casesmartgoal.LastUpdatedByWorkerID,
                                IsUsed = false
                            };
                            casesmartgoalserviceproviderRepository.InsertOrUpdate(newCaseSmartGoalInternalService);
                            casesmartgoalserviceproviderRepository.Save();
                            isAdded = true;
                        }
                    }
                    if (isAdded)
                    {
                        existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                        if (existingServiceProviderList == null)
                        {
                            existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                        }
                    }
                }
                isDeleted = false;
                foreach (CaseSmartGoalServiceProvider existingProposedInternalServiceProvider in existingServiceProviderList.Where(item => item.IsUsed == false && item.ServiceProvider != null && item.ServiceProvider.IsExternal == false))
                {
                    if (arrayselectedProposedInternalServiceProvider == null || !arrayselectedProposedInternalServiceProvider.Contains(existingProposedInternalServiceProvider.ServiceProviderID.ToString(true)))
                    {
                        casesmartgoalserviceproviderRepository.Delete(existingProposedInternalServiceProvider);
                        casesmartgoalserviceproviderRepository.Save();
                        isDeleted = true;
                    }
                }
                if (isDeleted)
                {
                    existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                    if (existingServiceProviderList == null)
                    {
                        existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                    }
                }
                isAdded = false;
                string selectedUsedExternalServiceProvider = casesmartgoal.UsedExternalServiceProviderIDs;
                selectedUsedExternalServiceProvider = selectedUsedExternalServiceProvider.ToString(true).Replace("false", string.Empty);
                string[] arrayselectedUsedExternalServiceProvider = selectedUsedExternalServiceProvider.ToStringArray(',', true);
                if (arrayselectedUsedExternalServiceProvider != null && arrayselectedUsedExternalServiceProvider.Length > 0)
                {
                    foreach (string qolID in arrayselectedUsedExternalServiceProvider)
                    {
                        if (existingServiceProviderList.Where(item => item.ServiceProviderID == qolID.ToInteger(true)).Count() == 0)
                        {
                            CaseSmartGoalServiceProvider newCaseSmartGoalExternalService = new CaseSmartGoalServiceProvider()
                            {
                                ServiceProviderID = qolID.ToInteger(true),
                                CaseSmartGoalID = casesmartgoal.ID,
                                LastUpdateDate = DateTime.Now,
                                LastUpdatedByWorkerID = casesmartgoal.LastUpdatedByWorkerID,
                                IsUsed = true
                            };
                            casesmartgoalserviceproviderRepository.InsertOrUpdate(newCaseSmartGoalExternalService);
                            casesmartgoalserviceproviderRepository.Save();
                            isAdded = true;
                        }
                    }
                    if (isAdded)
                    {
                        existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                        if (existingServiceProviderList == null)
                        {
                            existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                        }
                    }
                }
                isDeleted = false;
                foreach (CaseSmartGoalServiceProvider existingUsedExternalServiceProvider in existingServiceProviderList.Where(item => item.IsUsed == true && item.ServiceProvider != null && item.ServiceProvider.IsExternal == true))
                {
                    if (arrayselectedUsedExternalServiceProvider == null || !arrayselectedUsedExternalServiceProvider.Contains(existingUsedExternalServiceProvider.ServiceProviderID.ToString(true)))
                    {
                        casesmartgoalserviceproviderRepository.Delete(existingUsedExternalServiceProvider);
                        casesmartgoalserviceproviderRepository.Save();
                        isDeleted = true;
                    }
                }
                if (isDeleted)
                {
                    existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                    if (existingServiceProviderList == null)
                    {
                        existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                    }
                }
                isAdded = false;
                string selectedProposedExternalServiceProvider = casesmartgoal.ProposedExternalServiceProviderIDs;
                selectedProposedExternalServiceProvider = selectedProposedExternalServiceProvider.ToString(true).Replace("false", string.Empty);
                string[] arrayselectedProposedExternalServiceProvider = selectedProposedExternalServiceProvider.ToStringArray(',', true);
                if (arrayselectedProposedExternalServiceProvider != null && arrayselectedProposedExternalServiceProvider.Length > 0)
                {
                    foreach (string qolID in arrayselectedProposedExternalServiceProvider)
                    {
                        if (existingServiceProviderList.Where(item => item.ServiceProviderID == qolID.ToInteger(true)).Count() == 0)
                        {
                            CaseSmartGoalServiceProvider newCaseSmartGoalExternalService = new CaseSmartGoalServiceProvider()
                            {
                                ServiceProviderID = qolID.ToInteger(true),
                                CaseSmartGoalID = casesmartgoal.ID,
                                LastUpdateDate = DateTime.Now,
                                LastUpdatedByWorkerID = casesmartgoal.LastUpdatedByWorkerID,
                                IsUsed = false
                            };
                            casesmartgoalserviceproviderRepository.InsertOrUpdate(newCaseSmartGoalExternalService);
                            casesmartgoalserviceproviderRepository.Save();
                            isAdded = true;
                        }
                    }
                    if (isAdded)
                    {
                        existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                        if (existingServiceProviderList == null)
                        {
                            existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                        }
                    }
                }
                isDeleted = false;
                foreach (CaseSmartGoalServiceProvider existingProposedExternalServiceProvider in existingServiceProviderList.Where(item => item.IsUsed == false && item.ServiceProvider != null && item.ServiceProvider.IsExternal == true))
                {
                    if (arrayselectedProposedExternalServiceProvider == null || !arrayselectedProposedExternalServiceProvider.Contains(existingProposedExternalServiceProvider.ServiceProviderID.ToString(true)))
                    {
                        casesmartgoalserviceproviderRepository.Delete(existingProposedExternalServiceProvider);
                        casesmartgoalserviceproviderRepository.Save();
                        isDeleted = true;
                    }
                }
                if (isDeleted)
                {
                    existingServiceProviderList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoal.ID).ToList();
                    if (existingServiceProviderList == null)
                    {
                        existingServiceProviderList = new List<CaseSmartGoalServiceProvider>();
                    }
                }


                List<CaseSmartGoalAssignment> existingCaseSmartGoalAssignmentList = new List<CaseSmartGoalAssignment>();
                if (!isNew)
                {
                    existingCaseSmartGoalAssignmentList = context.CaseSmartGoalAssignment.Where(item=>item.CaseSmartGoalID==casesmartgoal.ID).ToList();
                    if (existingCaseSmartGoalAssignmentList == null)
                    {
                        existingCaseSmartGoalAssignmentList = new List<CaseSmartGoalAssignment>();
                    }
                }

                string selectedCaseSmartGoalAssignment = casesmartgoal.SmartGoalIDs;
                selectedCaseSmartGoalAssignment = selectedCaseSmartGoalAssignment.ToString(true).Replace("false", string.Empty);
                string[] arrayselectedCaseSmartGoalAssignment = selectedCaseSmartGoalAssignment.ToStringArray(',', true);
                if (arrayselectedCaseSmartGoalAssignment != null && arrayselectedCaseSmartGoalAssignment.Length > 0)
                {
                    foreach (string qolID in arrayselectedCaseSmartGoalAssignment)
                    {
                        //if (existingCaseSmartGoalAssignmentList.Where(item => item.SmartGoalID == qolID.ToInteger(true)).Count() == 0) 


                        var smartGoalOther = data["SmartGoalOther" + qolID] == null ? "" : data["SmartGoalOther" + qolID].ToString();

                        var qo1IDItem = existingCaseSmartGoalAssignmentList.FirstOrDefault(item => item.SmartGoalID == qolID.ToInteger(true));
                        if (qo1IDItem == null) 
                        {
                            CaseSmartGoalAssignment newCaseSmartGoalAssignment = new CaseSmartGoalAssignment()
                            {
                                SmartGoalID = qolID.ToInteger(true),
                                CaseSmartGoalID = casesmartgoal.ID,
                                LastUpdateDate = DateTime.Now,
                                LastUpdatedByWorkerID = casesmartgoal.LastUpdatedByWorkerID,
                                CreateDate = DateTime.Now,
                                CreatedByWorkerID = casesmartgoal.LastUpdatedByWorkerID,
                                IsArchived = false
                            };
                            if (data["SmartGoalStartDate" + qolID] != null && data["SmartGoalStartDate" + qolID].IsNotNullOrEmpty() && data["SmartGoalStartDate" + qolID].ToDateTime().IsValidDate())
                            {
                                newCaseSmartGoalAssignment.StartDate = data["SmartGoalStartDate" + qolID].ToDateTime();
                            }
                            if (data["SmartGoalEndDate" + qolID] != null && data["SmartGoalEndDate" + qolID].IsNotNullOrEmpty() && data["SmartGoalEndDate" + qolID].ToDateTime().IsValidDate())
                            {
                                newCaseSmartGoalAssignment.EndDate = data["SmartGoalEndDate" + qolID].ToDateTime();
                            }
                            if (data["Comment" + qolID] != null && data["Comment" + qolID].IsNotNullOrEmpty())
                            {
                                newCaseSmartGoalAssignment.Comment = data["Comment" + qolID].ToString();
                            }

                            
                            newCaseSmartGoalAssignment.SmartGoalOther = smartGoalOther;
                                                        
                            context.CaseSmartGoalAssignment.Add(newCaseSmartGoalAssignment);
                        }
                        else if(qo1IDItem.SmartGoalOther!=smartGoalOther)
                        {
                            qo1IDItem.SmartGoalOther = smartGoalOther;
                        }
                    }
                }

                foreach (CaseSmartGoalAssignment existingCaseSmartGoalAssignment in existingCaseSmartGoalAssignmentList)
                {
                    if (arrayselectedCaseSmartGoalAssignment == null || !arrayselectedCaseSmartGoalAssignment.Contains(existingCaseSmartGoalAssignment.SmartGoalID.ToString(true)))
                    {
                        context.Entry(existingCaseSmartGoalAssignment).State = System.Data.Entity.EntityState.Deleted;
                        Save();
                    }
                    else
                    {
                        if (data["SmartGoalStartDate" + existingCaseSmartGoalAssignment.SmartGoalID] != null && data["SmartGoalStartDate" + existingCaseSmartGoalAssignment.SmartGoalID].IsNotNullOrEmpty() && data["SmartGoalStartDate" + existingCaseSmartGoalAssignment.SmartGoalID].ToDateTime().IsValidDate())
                        {
                            existingCaseSmartGoalAssignment.StartDate = data["SmartGoalStartDate" + existingCaseSmartGoalAssignment.SmartGoalID].ToDateTime();
                        }
                        if (data["SmartGoalEndDate" + existingCaseSmartGoalAssignment.SmartGoalID] != null && data["SmartGoalEndDate" + existingCaseSmartGoalAssignment.SmartGoalID].IsNotNullOrEmpty() && data["SmartGoalEndDate" + existingCaseSmartGoalAssignment.SmartGoalID].ToDateTime().IsValidDate())
                        {
                            existingCaseSmartGoalAssignment.EndDate = data["SmartGoalEndDate" + existingCaseSmartGoalAssignment.SmartGoalID].ToDateTime();
                        }
                        if (data["Comment" + existingCaseSmartGoalAssignment.SmartGoalID] != null && data["Comment" + existingCaseSmartGoalAssignment.SmartGoalID].IsNotNullOrEmpty())
                        {
                            existingCaseSmartGoalAssignment.Comment = data["Comment" + existingCaseSmartGoalAssignment.SmartGoalID].ToString();
                        }
                        if (data["SmartGoalOther" + existingCaseSmartGoalAssignment.SmartGoalID] != null && data["SmartGoalOther" + existingCaseSmartGoalAssignment.SmartGoalID].IsNotNullOrEmpty())
                        {
                            existingCaseSmartGoalAssignment.Comment = data["SmartGoalOther" + existingCaseSmartGoalAssignment.SmartGoalID].ToString();
                        }
                    }
                }
            }
        }

        public DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId, bool isClosed)
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
            bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoal, Constants.Actions.Read, true);
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoal, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoal, Constants.Actions.Delete, true);
            //bool hasDeletePermission = IsCurrentLoggedInWorkerAdministrator;
            bool hasTrackGoalPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalProgress, Constants.Actions.Index, true);
            bool IsUserAdminWorker = CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1;

            List<CaseSmartGoal> caseSmartGoalList = context.CaseSmartGoal
                .Join(context.CaseGoal, left => left.CaseGoalID, right => right.ID, (left, right) => new { left, right })
                //.Join(context.CaseWorkerMemberAssignment, secondleft => secondleft.right.CaseMemberID, secondright => secondright.CaseMemberID, (secondleft, secondright) => new { secondleft, secondright })
                //.Where(item => item.secondleft.right.CaseMember.CaseID == caseId && item.secondright.CaseWorker.WorkerID == workerId)
                .Where(item => item.right.CaseMember.CaseID == caseId)
                //<JL:Comment:06/18/2017>
                //.Where(item => context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(item.right.CaseMemberID) || IsUserAdminWorker)
                .OrderBy(item => item.left.StartDate)
                .AsEnumerable()
                .ToList()
                .Select(
                casesmartgoal => new CaseSmartGoal()
                {
                    ID = casesmartgoal.left.ID,
                    CreateDate = casesmartgoal.left.CreateDate,
                    CaseMemberID = casesmartgoal.right.CaseMemberID,
                    CaseMemberName = casesmartgoal.right.CaseMember.FirstName + " " + casesmartgoal.right.CaseMember.LastName,
                    ServiceLevelOutcomeID = casesmartgoal.left.ServiceLevelOutcomeID,
                    ServiceLevelOutcomeName = casesmartgoal.left.ServiceLevelOutcome != null ? casesmartgoal.left.ServiceLevelOutcome.Name : "",
                    QualityOfLifeCategoryName = casesmartgoal.left.QualityOfLifeCategory != null ? casesmartgoal.left.QualityOfLifeCategory.Name : "",
                    StartDate = casesmartgoal.left.StartDate,
                    EndDate = casesmartgoal.left.EndDate,
                    CaseID = caseId,
                    IsCompleted = casesmartgoal.left.IsCompleted,
                    HasPermissionToRead = hasReadPermission ? "" : "display:none;",
                    HasPermissionToEdit = hasEditPermission ? "" : "display:none;",
                    HasPermissionToDelete = hasDeletePermission ? "" : "display:none;",
                    HasPermissionToTrackGoal = hasTrackGoalPermission ? "" : "display:none;"
                }
                ).ToList();
            if (caseSmartGoalList != null)
            {
                foreach (CaseSmartGoal caseSmartGoal in caseSmartGoalList)
                {
                    caseSmartGoal.TotalActionCount = context.CaseAction.Where(item => item.CaseSmartGoalID == caseSmartGoal.ID && item.CaseSmartGoalServiceProviderID==null).Count();
                    caseSmartGoal.OpenActionCount = context.CaseAction.Where(item => (item.CaseSmartGoalID == caseSmartGoal.ID && item.CaseSmartGoalServiceProviderID == null) && item.IsCompleted == false).Count();
                    caseSmartGoal.CloseActionCount = context.CaseAction.Where(item => (item.CaseSmartGoalID == caseSmartGoal.ID && item.CaseSmartGoalServiceProviderID == null) && item.IsCompleted == true).Count();
                    List<CaseSmartGoalAssignment> goalAssignmentList = context.CaseSmartGoalAssignment.Where(item => item.CaseSmartGoalID == caseSmartGoal.ID).ToList();
                    if (goalAssignmentList != null)
                    {
                        foreach (CaseSmartGoalAssignment goalAssignment in goalAssignmentList)
                        {
                            caseSmartGoal.SmartGoalName = caseSmartGoal.SmartGoalName.Concate(",", goalAssignment.SmartGoal.Name);
                        }
                    }
                }
            }
            if (isClosed)
            {
                if (dsRequest.Filters == null || (dsRequest.Filters != null && dsRequest.Filters.Count == 0))
                {
                    if (dsRequest.Filters == null)
                    {
                        dsRequest.Filters = new List<IFilterDescriptor>();
                    }
                }
                FilterDescriptor newFilterDescriptor = new FilterDescriptor("IsCompleted", FilterOperator.IsEqualTo, true);
                dsRequest.Filters.Add(newFilterDescriptor);
            }
            else
            {
                if (dsRequest.Filters == null || (dsRequest.Filters != null && dsRequest.Filters.Count == 0))
                {
                    if (dsRequest.Filters == null)
                    {
                        dsRequest.Filters = new List<IFilterDescriptor>();
                    }
                }
                FilterDescriptor newFilterDescriptor = new FilterDescriptor("IsCompleted", FilterOperator.IsEqualTo, false);
                dsRequest.Filters.Add(newFilterDescriptor);
            }
            DataSourceResult dsResult = caseSmartGoalList.ToDataSourceResult(dsRequest);
            return dsResult;
        }

        public DataSourceResult AssignedServiceProvider(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            FilterDescriptor newFilterDescriptor = new FilterDescriptor("IsCompleted", FilterOperator.IsEqualTo, false);
            dsRequest.Filters.Add(newFilterDescriptor);
            if (caseMemberId.HasValue && caseMemberId > 0)
            {
                FilterDescriptor filterDescriptor = new FilterDescriptor("CaseMemberID", FilterOperator.IsEqualTo, caseMemberId.Value);
                dsRequest.Filters.Add(filterDescriptor);
            }
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Index, true);
            DataSourceResult dsResult = context.CaseSmartGoal
                .Join(context.CaseGoal, left => left.CaseGoalID, right => right.ID, (left, right) => new { left, right })
                //.Join(context.CaseWorkerMemberAssignment, secondleft => secondleft.right.CaseMemberID, secondright => secondright.CaseMemberID, (secondleft, secondright) => new { secondleft, secondright })
                //.Where(item => item.secondleft.right.CaseMember.CaseID == caseId && item.secondright.CaseWorker.WorkerID == workerId)
                .Where(item => item.right.CaseMember.CaseID == caseId && (context.CaseSmartGoalServiceProvider.Where(c=>c.CaseSmartGoalID==item.left.ID).Count()>0) )
                .OrderBy(item => item.left.StartDate).ToList()
                .Select(
                casesmartgoal => new CaseSmartGoal()
                {
                    ID = casesmartgoal.left.ID,
                    CreateDate = casesmartgoal.left.CreateDate,
                    CaseMemberID = casesmartgoal.right.CaseMemberID,
                    CaseMemberName = casesmartgoal.right.CaseMember.FirstName + " " + casesmartgoal.right.CaseMember.LastName,
                    ServiceLevelOutcomeID = casesmartgoal.left.ServiceLevelOutcomeID,
                    ServiceLevelOutcomeName = casesmartgoal.left.ServiceLevelOutcome != null ? casesmartgoal.left.ServiceLevelOutcome.Name : "",
                    QualityOfLifeCategoryName = casesmartgoal.left.QualityOfLifeCategory != null ? casesmartgoal.left.QualityOfLifeCategory.Name : "",
                    StartDate = casesmartgoal.left.StartDate,
                    EndDate = casesmartgoal.left.EndDate,
                    CaseID = caseId,
                    IsCompleted = casesmartgoal.left.IsCompleted,
                    HasPermissionToEdit = hasEditPermission ? "" : "display:none;"
                }
                ).ToDataSourceResult(dsRequest);
            if (dsResult.Data != null)
            {
                foreach (CaseSmartGoal dataItem in dsResult.Data)
                {
                    List<CaseSmartGoalServiceProvider> serviceProviderList = casesmartgoalserviceproviderRepository.All.Where(item => item.CaseSmartGoalID == dataItem.ID).ToList();
                    if (serviceProviderList != null)
                    {
                        foreach (CaseSmartGoalServiceProvider serviceProvider in serviceProviderList.Where(item=>item.ServiceProvider.IsExternal==false))
                        {
                            if (serviceProvider.ServiceProvider != null)
                            {
                                dataItem.InternalServiceProvider = dataItem.InternalServiceProvider.Concate(',', serviceProvider.ServiceProvider.Name);
                            }
                        }

                        foreach (CaseSmartGoalServiceProvider serviceProvider in serviceProviderList.Where(item => item.ServiceProvider.IsExternal == true))
                        {
                            if (serviceProvider.ServiceProvider != null)
                            {
                                dataItem.ExternalServiceProvider = dataItem.ExternalServiceProvider.Concate(',', serviceProvider.ServiceProvider.Name);
                            }
                        }
                    }

                    List<CaseSmartGoalAssignment> goalAssignmentList = context.CaseSmartGoalAssignment.Where(item => item.CaseSmartGoalID == dataItem.ID).ToList();
                    if (goalAssignmentList != null)
                    {
                        foreach (CaseSmartGoalAssignment goalAssignment in goalAssignmentList)
                        {
                            dataItem.SmartGoalName = dataItem.SmartGoalName.Concate(",", goalAssignment.SmartGoal.Name);
                        }
                    }
                }
            }
            return dsResult;
        }

        public DataSourceResult UnAssignedServiceProvider(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            FilterDescriptor newFilterDescriptor = new FilterDescriptor("IsCompleted", FilterOperator.IsEqualTo, false);
            dsRequest.Filters.Add(newFilterDescriptor);
            if (caseMemberId.HasValue && caseMemberId > 0)
            {
                FilterDescriptor filterDescriptor = new FilterDescriptor("CaseMemberID", FilterOperator.IsEqualTo, caseMemberId.Value);
                dsRequest.Filters.Add(filterDescriptor);
            }
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Index, true);
            DataSourceResult dsResult = context.CaseSmartGoal
                .Join(context.CaseGoal, left => left.CaseGoalID, right => right.ID, (left, right) => new { left, right })
                //.Join(context.CaseWorkerMemberAssignment, secondleft => secondleft.right.CaseMemberID, secondright => secondright.CaseMemberID, (secondleft, secondright) => new { secondleft, secondright })
                //.Where(item => item.secondleft.right.CaseMember.CaseID == caseId && item.secondright.CaseWorker.WorkerID == workerId)
                .Where(item => item.right.CaseMember.CaseID == caseId && (context.CaseSmartGoalServiceProvider.Where(c => c.CaseSmartGoalID == item.left.ID).Count() == 0))
                .OrderBy(item => item.left.StartDate).ToList()
                .Select(
                casesmartgoal => new CaseSmartGoal()
                {
                    ID = casesmartgoal.left.ID,
                    CreateDate = casesmartgoal.left.CreateDate,
                    CaseMemberID = casesmartgoal.right.CaseMemberID,
                    CaseMemberName = casesmartgoal.right.CaseMember.FirstName + " " + casesmartgoal.right.CaseMember.LastName,
                    ServiceLevelOutcomeID = casesmartgoal.left.ServiceLevelOutcomeID,
                    ServiceLevelOutcomeName = casesmartgoal.left.ServiceLevelOutcome != null ? casesmartgoal.left.ServiceLevelOutcome.Name : "",
                    QualityOfLifeCategoryName = casesmartgoal.left.QualityOfLifeCategory != null ? casesmartgoal.left.QualityOfLifeCategory.Name : "",
                    StartDate = casesmartgoal.left.StartDate,
                    EndDate = casesmartgoal.left.EndDate,
                    CaseID = caseId,
                    IsCompleted = casesmartgoal.left.IsCompleted,
                    HasPermissionToEdit = hasEditPermission ? "" : "display:none;"
                }
                ).ToDataSourceResult(dsRequest);
            if (dsResult.Data != null)
            {
                foreach (CaseSmartGoal dataItem in dsResult.Data)
                {
                    List<CaseSmartGoalServiceProvider> serviceProviderList = casesmartgoalserviceproviderRepository.All.Where(item => item.CaseSmartGoalID == dataItem.ID).ToList();
                    if (serviceProviderList != null)
                    {
                        foreach (CaseSmartGoalServiceProvider serviceProvider in serviceProviderList.Where(item => item.ServiceProvider.IsExternal == false))
                        {
                            if (serviceProvider.ServiceProvider != null)
                            {
                                dataItem.InternalServiceProvider = dataItem.InternalServiceProvider.Concate(',', serviceProvider.ServiceProvider.Name);
                            }
                        }

                        foreach (CaseSmartGoalServiceProvider serviceProvider in serviceProviderList.Where(item => item.ServiceProvider.IsExternal == true))
                        {
                            if (serviceProvider.ServiceProvider != null)
                            {
                                dataItem.ExternalServiceProvider = dataItem.ExternalServiceProvider.Concate(',', serviceProvider.ServiceProvider.Name);
                            }
                        }
                    }

                    List<CaseSmartGoalAssignment> goalAssignmentList = context.CaseSmartGoalAssignment.Where(item => item.CaseSmartGoalID == dataItem.ID).ToList();
                    if (goalAssignmentList != null)
                    {
                        foreach (CaseSmartGoalAssignment goalAssignment in goalAssignmentList)
                        {
                            dataItem.SmartGoalName = dataItem.SmartGoalName.Concate(",", goalAssignment.SmartGoal.Name);
                        }
                    }
                }
            }
            return dsResult;
        }

        public List<CaseSmartGoalAssignment> FindAllCaseSmartGoalAssignmentByCaseSmargGoalID(int id)
        {
            return context.CaseSmartGoalAssignment.Where(item => item.CaseSmartGoalID == id).ToList();
        }

        public List<SelectListItem> FindAllCaseSmartGoalAssignmentForDropDownListByCaseSmargGoalID(int id)
        {
            return context.CaseSmartGoalAssignment.Where(item => item.CaseSmartGoalID == id).AsEnumerable().ToList().Select(item => new SelectListItem() { Value=item.SmartGoalID.ToString(), Text=item.SmartGoal.Name }).ToList();
        }

        public bool HasOpenSmartGoal(int caseMemberId)
        {
            int openGoalsCount = context.CaseSmartGoal
                .Join(context.CaseGoal, left => left.CaseGoalID, right => right.ID, (left, right) => new { left, right })
                .Where(item => item.right.CaseMemberID == caseMemberId && item.left.IsCompleted==false).Count();
            if (openGoalsCount > 0)
            {
                return true;
            }
            return false;
        }

        public void EndGoal(int id)
        {
            int count=caseactionRepository.FindPendingActionCount(id);
            if (count > 0)
            {
                throw new CustomException("You can't end this goal because it has open actions.");
            }
            CaseSmartGoal casesmartGoal = Find(id);
            if (casesmartGoal != null)
            {
                casesmartGoal.EndDate = DateTime.Now;
                InsertOrUpdate(casesmartGoal, null, true);
                Save();
            }
        }
    }

    /// <summary>
    /// interface of CaseSmartGoal containing necessary database operations
    /// </summary>
    public interface ICaseSmartGoalRepository : IBaseRepository<CaseSmartGoal>
    {
        IQueryable<CaseSmartGoal> AllIncluding(int caseId, params Expression<Func<CaseSmartGoal, object>>[] includeProperties);
        IQueryable<CaseSmartGoal> FindAllByCaseMemberID(int casememberID);
        void InsertOrUpdate(CaseSmartGoal casesmartgoal, NameValueCollection data, bool endOnly);
        void EndGoal(int id);
        DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId, bool isClosed);
        DataSourceResult AssignedServiceProvider(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId);
        DataSourceResult UnAssignedServiceProvider(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId);
        List<CaseSmartGoalAssignment> FindAllCaseSmartGoalAssignmentByCaseSmargGoalID(int id);
        List<SelectListItem> FindAllCaseSmartGoalAssignmentForDropDownListByCaseSmargGoalID(int id);
        bool HasOpenSmartGoal(int caseMemberId);
    }
}
