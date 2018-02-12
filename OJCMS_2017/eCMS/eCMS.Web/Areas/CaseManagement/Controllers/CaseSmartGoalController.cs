//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories;
using eCMS.DataLogic.Models;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseSmartGoalController : CaseBaseController
    {
        private readonly ISmartGoalRepository smartgoalRepository;
        private readonly IServiceLevelOutcomeRepository serviceleveloutcomeRepository;
        private readonly ICaseSmartGoalRepository casesmartgoalRepository;
        private readonly IServiceProviderRepository serviceproviderRepository;
        private readonly ICaseSmartGoalServiceProviderRepository casesmartgoalserviceproviderRepository;
        private readonly ICaseGoalLivingConditionRepository casegoallivingconditionRepository;
        private readonly ICaseAssessmentLivingConditionRepository caseassessmentlivingconditionRepository;
        private readonly ICaseGoalRepository casegoalRepository;
        public CaseSmartGoalController(IWorkerRepository workerRepository, 
            ICaseMemberRepository casememberRepository, 
            IServiceLevelOutcomeRepository serviceleveloutcomeRepository, 
            IQualityOfLifeCategoryRepository qualityoflifecategoryRepository,
            ICaseSmartGoalRepository casesmartgoalRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseRepository caseRepository,
            ICaseSmartGoalServiceProviderRepository casesmartgoalserviceproviderRepository,
            ICaseGoalLivingConditionRepository casegoallivingconditionRepository,
            ICaseAssessmentLivingConditionRepository caseassessmentlivingconditionRepository,
            ICaseGoalRepository casegoalRepository,
            ICaseWorkerRepository caseworkerRepository,
            ISmartGoalRepository smartgoalRepository,
            IServiceProviderRepository serviceproviderRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.casememberRepository = casememberRepository;
            this.serviceleveloutcomeRepository = serviceleveloutcomeRepository;
            this.qualityoflifecategoryRepository = qualityoflifecategoryRepository;
            this.casesmartgoalRepository = casesmartgoalRepository;
            this.casesmartgoalserviceproviderRepository = casesmartgoalserviceproviderRepository;
            this.casegoallivingconditionRepository = casegoallivingconditionRepository;
            this.caseassessmentlivingconditionRepository = caseassessmentlivingconditionRepository;
            this.casegoalRepository = casegoalRepository;
            this.smartgoalRepository = smartgoalRepository;
            this.serviceproviderRepository = serviceproviderRepository;
        }

        /// <summary>
        /// This action creates new casesmartgoal
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int caseGoalId, int caseId, int? caseMemberId, int? qualityOfLifeCategoryID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoal, Constants.Actions.Create, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            
            //create a new instance of casesmartgoal
            CaseSmartGoal casesmartgoal = new CaseSmartGoal();
            casesmartgoal.CaseGoalID = caseGoalId;
            casesmartgoal.CaseID = caseId;
            
            casesmartgoal.ServiceLevelOutcomeID = (int)ServiceLevelOutcomeType.InProcess;
            if (qualityOfLifeCategoryID.HasValue && qualityOfLifeCategoryID.Value > 0)
            {
                casesmartgoal.QualityOfLifeCategoryID = qualityOfLifeCategoryID.Value;
                LoadSmartGoal(casesmartgoal.QualityOfLifeCategoryID);
            }
            CaseGoal caseGoal = casegoalRepository.Find(caseGoalId);
            if (caseGoal != null)
            {
                casesmartgoal.CaseMemberID = caseGoal.CaseMemberID;
                casesmartgoal.StartDate = caseGoal.StartDate;
                casesmartgoal.EndDate = caseGoal.EndDate;
            }
            ViewBag.UsedInternalService = serviceproviderRepository.All.Where(item=>item.IsExternal==false && item.Name!="Other").OrderBy(item=>item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
            var externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
            if (externalServiceList != null && externalServiceList.Count > 0)
            {
                var externalProvidersCount = externalServiceList.Count;
                var index = externalServiceList.FindIndex(x => x.ServiceProviderName == "Other");
                if (index >= 0)
                {
                    var otherServiceaProvider = externalServiceList[index];
                    externalServiceList[index] = externalServiceList[externalProvidersCount - 1];
                    externalServiceList[externalProvidersCount - 1] = otherServiceaProvider;
                }
                else
                {
                    ServiceProvider serviceProvider = new ServiceProvider();
                    serviceProvider.Name = "Other";
                    serviceproviderRepository.InsertOrUpdate(serviceProvider);
                    serviceproviderRepository.Save();
                    externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
                    var index1 = externalServiceList.FindIndex(x => x.ServiceProviderName == "Other");
                    if (index1 >= 0)
                    {
                        var otherServiceaProvider = externalServiceList[index1];
                        externalServiceList[index1] = externalServiceList[externalProvidersCount - 1];
                        externalServiceList[externalProvidersCount - 1] = otherServiceaProvider;
                    }
                }
                
            }
            ViewBag.UsedExternalService = externalServiceList;
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseGoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseGoal.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return view result
            return View(casesmartgoal);
        }

        /// <summary>
        /// This action saves new casesmartgoal to database
        /// </summary>
        /// <param name="casesmartgoal">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseSmartGoal casesmartgoal, int caseGoalId, int caseId, int? caseMemberId, int? qualityOfLifeCategoryID)
        {
            casesmartgoal.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                if (casesmartgoal.StartDate > casesmartgoal.EndDate)
                {
                    throw new CustomException("Start date can't be greater than end date.");
                }
                casesmartgoal.UsedInternalServiceProviderIDs = Request.Form["UsedInternalServiceProviderIDs"].ToString(true);
                casesmartgoal.ProposedInternalServiceProviderIDs = Request.Form["ProposedInternalServiceProviderIDs"].ToString(true);
                if (casesmartgoal.UsedInternalServiceProviderIDs.IsNotNullOrEmpty() && casesmartgoal.ProposedInternalServiceProviderIDs.IsNotNullOrEmpty())
                {
                    string[] arrayUsedInternalServiceProviderIDs = casesmartgoal.UsedInternalServiceProviderIDs.ToStringArray(',');
                    string[] arrayProposedInternalServiceProviderIDs = casesmartgoal.ProposedInternalServiceProviderIDs.ToStringArray(',');
                    foreach (string usedInternalServiceProviderID in arrayUsedInternalServiceProviderIDs)
                    {
                        foreach (string proposedInternalServiceProviderID in arrayProposedInternalServiceProviderIDs)
                        {
                            if (usedInternalServiceProviderID == proposedInternalServiceProviderID)
                            {
                                throw new CustomException("You can't select the same internal service provider for both used and proposed.");
                            }
                        }
                    }
                }
                casesmartgoal.UsedExternalServiceProviderIDs = Request.Form["UsedExternalServiceProviderIDs"].ToString(true);
                casesmartgoal.ProposedExternalServiceProviderIDs = Request.Form["ProposedExternalServiceProviderIDs"].ToString(true);
                if (casesmartgoal.UsedExternalServiceProviderIDs.IsNotNullOrEmpty() && casesmartgoal.ProposedExternalServiceProviderIDs.IsNotNullOrEmpty())
                {
                    string[] arrayUsedExternalServiceProviderIDs = casesmartgoal.UsedExternalServiceProviderIDs.ToStringArray(',');
                    string[] arrayProposedExternalServiceProviderIDs = casesmartgoal.ProposedExternalServiceProviderIDs.ToStringArray(',');
                    foreach (string usedExternalServiceProviderID in arrayUsedExternalServiceProviderIDs)
                    {
                        foreach (string proposedExternalServiceProviderID in arrayProposedExternalServiceProviderIDs)
                        {
                            if (usedExternalServiceProviderID == proposedExternalServiceProviderID)
                            {
                                throw new CustomException("You can't select the same external service provider for both used and proposed.");
                            }
                        }
                    }
                }
                casesmartgoal.SmartGoalIDs = Request.Form["SmartGoalIDs"].ToString(true);
                Session["SmartGoalIDs"] = casesmartgoal.SmartGoalIDs;
                if (casesmartgoal.SmartGoalIDs.IsNullOrEmpty())
                {
                    throw new CustomException("You have to select at least one measurable goal.");
                }

                string[] arraySmargGoalIDs = casesmartgoal.SmartGoalIDs.ToStringArray(',');
                foreach (string smargGoalID in arraySmargGoalIDs)
                {
                    if (Request.Form["SmartGoalStartDate" + smargGoalID] != null && Request.Form["SmartGoalStartDate" + smargGoalID].IsNotNullOrEmpty())
                    {
                        DateTime startDate = Request.Form["SmartGoalStartDate" + smargGoalID].ToDateTime();
                        if (startDate < casesmartgoal.StartDate)
                        {
                            throw new CustomException("Start date of measurable goal can't be less than the overall start date.");
                        }
                        if (startDate > casesmartgoal.EndDate)
                        {
                            throw new CustomException("Start date of measurable goal can't be greater than the overall end date.");
                        }
                    }
                    if (Request.Form["SmartGoalEndDate" + smargGoalID] != null && Request.Form["SmartGoalEndDate" + smargGoalID].IsNotNullOrEmpty())
                    {
                        DateTime endDate = Request.Form["SmartGoalEndDate" + smargGoalID].ToDateTime();
                        if (endDate < casesmartgoal.StartDate)
                        {
                            throw new CustomException("End date of measurable goal can't be less than the overall start date.");
                        }
                        if (endDate > casesmartgoal.EndDate)
                        {
                            throw new CustomException("End date of measurable goal can't be greater than the overall end date.");
                        }
                    }
                }
                CaseGoal caseGoal = casegoalRepository.Find(casesmartgoal.CaseGoalID);
                if (caseGoal != null && casesmartgoal.StartDate < caseGoal.StartDate)
                {
                    throw new CustomException("The target effective date of measurable goal can't be before the start date of Identified Goal");
                }
                if (caseGoal != null && casesmartgoal.EndDate > caseGoal.EndDate)
                {
                    throw new CustomException("The target end date of measurable goal can't be after the end date of Identified Goal");
                }
                
                //validate data
                if (ModelState.IsValid)
                {                    
                    //call the repository function to save in database
                    casesmartgoalRepository.InsertOrUpdate(casesmartgoal, Request.Form, false);
                    casesmartgoalRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Actions.ServiceProvider, Constants.Controllers.CaseSmartGoal, new { caseId = caseId, caseMemberId = casesmartgoal.CaseMemberID });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            casesmartgoal.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (casesmartgoal.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                casesmartgoal.ErrorMessage = "Measurable Goal already exist";
            }
            catch (CustomException ex)
            {
                casesmartgoal.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                casesmartgoal.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            List<CaseSmartGoalServiceProvider> internalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == false && item.Name!="Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
            List<Int32> existingUsedInternalServiceList = new List<Int32>();
            if (casesmartgoal.UsedInternalServiceProviderIDs.IsNotNullOrEmpty())
            {
                existingUsedInternalServiceList = casesmartgoal.UsedInternalServiceProviderIDs.ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            List<Int32> existingProposedInternalServiceList = new List<Int32>();
            if (casesmartgoal.ProposedInternalServiceProviderIDs.IsNotNullOrEmpty())
            {
                existingProposedInternalServiceList = casesmartgoal.ProposedInternalServiceProviderIDs.ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            foreach (CaseSmartGoalServiceProvider internalService in internalServiceList)
            {
                if (existingUsedInternalServiceList != null)
                {
                    foreach (int existingInternalServiceID in existingUsedInternalServiceList)
                    {
                        if (internalService.ServiceProviderID == existingInternalServiceID)
                        {
                            internalService.ID = existingInternalServiceID;
                            internalService.IsUsed = true;
                            break;
                        }
                    }
                }

                if (existingProposedInternalServiceList != null)
                {
                    foreach (int existingInternalServiceID in existingProposedInternalServiceList)
                    {
                        if (internalService.ServiceProviderID == existingInternalServiceID)
                        {
                            internalService.ID = existingInternalServiceID;
                            internalService.IsUsed = false;
                            break;
                        }
                    }
                }
            }
            ViewBag.UsedInternalService = internalServiceList;
           

            List<CaseSmartGoalServiceProvider> externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
            if (externalServiceList != null && externalServiceList.Count > 0)
            {
                var externalProvidersCount = externalServiceList.Count;
                var index = externalServiceList.FindIndex(x => x.ServiceProviderName == "Other");
                if (index >= 0)
                {
                    var otherServiceaProvider = externalServiceList[index];
                    externalServiceList[index] = externalServiceList[externalProvidersCount - 1];
                    externalServiceList[externalProvidersCount - 1] = otherServiceaProvider;
                }
                else
                {
                    ServiceProvider serviceProvider = new ServiceProvider();
                    serviceProvider.Name = "Other";
                    serviceproviderRepository.InsertOrUpdate(serviceProvider);
                    serviceproviderRepository.Save();
                    externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
                    var index1 = externalServiceList.FindIndex(x => x.ServiceProviderName == "Other");
                    if (index1 >= 0)
                    {
                        var otherServiceaProvider = externalServiceList[index1];
                        externalServiceList[index1] = externalServiceList[externalProvidersCount - 1];
                        externalServiceList[externalProvidersCount - 1] = otherServiceaProvider;
                    }
                }
            }
            List<Int32> existingUsedExternalServiceList = new List<Int32>();
            if (casesmartgoal.UsedExternalServiceProviderIDs.IsNotNullOrEmpty())
            {
                existingUsedExternalServiceList = casesmartgoal.UsedExternalServiceProviderIDs.ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            List<Int32> existingProposedExternalServiceList = new List<Int32>();
            if (casesmartgoal.ProposedExternalServiceProviderIDs.IsNotNullOrEmpty())
            {
                existingProposedExternalServiceList = casesmartgoal.ProposedExternalServiceProviderIDs.ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            foreach (CaseSmartGoalServiceProvider externalService in externalServiceList)
            {
                if (existingUsedExternalServiceList != null)
                {
                    foreach (int existingExternalServiceID in existingUsedExternalServiceList)
                    {
                        if (externalService.ServiceProviderID == existingExternalServiceID)
                        {
                            externalService.ID = existingExternalServiceID;
                            externalService.IsUsed = true;
                            break;
                        }
                    }
                }

                if (existingProposedExternalServiceList != null)
                {
                    foreach (int existingExternalServiceID in existingProposedExternalServiceList)
                    {
                        if (externalService.ServiceProviderID == existingExternalServiceID)
                        {
                            externalService.ID = existingExternalServiceID;
                            externalService.IsUsed = false;
                            break;
                        }
                    }
                }
            }
            ViewBag.UsedExternalService = externalServiceList;

            if (casesmartgoal.QualityOfLifeCategoryID > 0)
            {
                LoadSmartGoal(casesmartgoal.QualityOfLifeCategoryID);
            }
            //return view with error message if operation is failed
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                casesmartgoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(casesmartgoal.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            return View(casesmartgoal);
        }

        /// <summary>
        /// This action edits an existing casesmartgoal
        /// </summary>
        /// <param name="id">casesmartgoal id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int casesmartgoalId, int caseId, int? caseMemberId, int? qualityOfLifeCategoryID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the existing casesmartgoal from database
            CaseSmartGoal casesmartgoal = casesmartgoalRepository.Find(casesmartgoalId);
            casesmartgoal.CaseID = caseId;
            if (qualityOfLifeCategoryID.HasValue && qualityOfLifeCategoryID.Value > 0)
            {
                casesmartgoal.QualityOfLifeCategoryID = qualityOfLifeCategoryID.Value;
            }
            if (caseMemberId.HasValue)
            {
                casesmartgoal.CaseMemberID = caseMemberId.Value;
            }
            CaseGoal caseGoal = casegoalRepository.Find(casesmartgoal.CaseGoalID);
            if (caseGoal != null)
            {
                casesmartgoal.CaseMemberID = caseGoal.CaseMemberID;
            }
            //LoadSmartGoal(casesmartgoal.QualityOfLifeCategoryID);
            List<CaseSmartGoalServiceProvider> internalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == false && item.Name != "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID, IsUsed = false }).ToList();
            List<CaseSmartGoalServiceProvider> existingInternalServiceList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoalId).Where(item => item.ServiceProvider.IsExternal == false && item.ServiceProvider.Name != "Other").ToList();
            foreach (CaseSmartGoalServiceProvider internalService in internalServiceList)
            {
                if (existingInternalServiceList != null)
                {
                    foreach (CaseSmartGoalServiceProvider existingInternalService in existingInternalServiceList)
                    {
                        if (internalService.ServiceProviderID == existingInternalService.ServiceProviderID)
                        {
                            internalService.ID = existingInternalService.ID;
                            internalService.IsUsed = existingInternalService.IsUsed;
                            break;
                        }
                    }
                }
            }
            ViewBag.UsedInternalService = internalServiceList;


            List<CaseSmartGoalServiceProvider> externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
            List<CaseSmartGoalServiceProvider> existingExternalServiceList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoalId).Where(item => item.ServiceProvider.IsExternal == true || item.ServiceProvider.Name == "Other").ToList();
            foreach (CaseSmartGoalServiceProvider externalService in externalServiceList)
            {
                if (existingExternalServiceList != null)
                {
                    foreach (CaseSmartGoalServiceProvider existingExternalService in existingExternalServiceList)
                    {
                        if (externalService.ServiceProviderID == existingExternalService.ServiceProviderID)
                        {
                            externalService.ID = existingExternalService.ID;
                            externalService.IsUsed = existingExternalService.IsUsed;
                            break;
                        }
                    }
                }
            }
            if (externalServiceList != null && externalServiceList.Count > 0)
            {
                var externalProvidersCount = externalServiceList.Count;
                var index = externalServiceList.FindIndex(x => x.ServiceProviderName == "Other");
                if (index >= 0)
                {
                    var otherServiceaProvider = externalServiceList[index];
                    externalServiceList[index] = externalServiceList[externalProvidersCount - 1];
                    externalServiceList[externalProvidersCount - 1] = otherServiceaProvider;
                }
                else
                {
                    ServiceProvider serviceProvider = new ServiceProvider();
                    serviceProvider.Name = "Other";
                    serviceproviderRepository.InsertOrUpdate(serviceProvider);
                    serviceproviderRepository.Save();
                    externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
                    var index1 = externalServiceList.FindIndex(x => x.ServiceProviderName == "Other");
                    if (index1 >= 0)
                    {
                        var otherServiceaProvider = externalServiceList[index1];
                        externalServiceList[index1] = externalServiceList[externalProvidersCount - 1];
                        externalServiceList[externalProvidersCount - 1] = otherServiceaProvider;
                    }
                }
            }
            ViewBag.UsedExternalService = externalServiceList;
            List<CaseSmartGoalAssignment> smartGoalList = smartgoalRepository.FindAllByCategoryID(casesmartgoal.QualityOfLifeCategoryID);
            List<CaseSmartGoalAssignment> existingSmartGoalList = casesmartgoalRepository.FindAllCaseSmartGoalAssignmentByCaseSmargGoalID(casesmartgoal.ID);
            foreach (CaseSmartGoalAssignment smartGoal in smartGoalList)
            {
                foreach (CaseSmartGoalAssignment existingSmartGoal in existingSmartGoalList)
                {
                    if (smartGoal.SmartGoalID == existingSmartGoal.SmartGoalID)
                    {
                        smartGoal.StartDate = existingSmartGoal.StartDate;
                        smartGoal.EndDate = existingSmartGoal.EndDate;
                        smartGoal.Comment = existingSmartGoal.Comment;
                        smartGoal.SmartGoalOther = existingSmartGoal.SmartGoalOther;
                        smartGoal.Checked = "checked";
                    }
                }
            }
            ViewBag.SmartGoalList = smartGoalList;
            //Session["SmartGoalIDs"] = String.Join(",", casesmartgoalRepository.FindAllCaseSmartGoalAssignmentByCaseSmargGoalID(casesmartgoal.ID).Select(item => item.SmartGoalID));
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                casesmartgoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(casesmartgoal.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(casesmartgoal);
        }

        [WorkerAuthorize]
        public ActionResult Read(int casesmartgoalId, int caseId, int? caseMemberId, int? qualityOfLifeCategoryID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoal, Constants.Actions.Read, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the existing casesmartgoal from database
            CaseSmartGoal casesmartgoal = casesmartgoalRepository.Find(casesmartgoalId);
            casesmartgoal.CaseID = caseId;
            if (qualityOfLifeCategoryID.HasValue && qualityOfLifeCategoryID.Value > 0)
            {
                casesmartgoal.QualityOfLifeCategoryID = qualityOfLifeCategoryID.Value;
            }
            if (caseMemberId.HasValue)
            {
                casesmartgoal.CaseMemberID = caseMemberId.Value;
            }
            CaseGoal caseGoal = casegoalRepository.Find(casesmartgoal.CaseGoalID);
            if (caseGoal != null)
            {
                casesmartgoal.CaseMemberID = caseGoal.CaseMemberID;
            }
            //LoadSmartGoal(casesmartgoal.QualityOfLifeCategoryID);
            List<CaseSmartGoalServiceProvider> internalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == false && item.Name != "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID, IsUsed = false }).ToList();
            List<CaseSmartGoalServiceProvider> existingInternalServiceList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoalId).Where(item => item.ServiceProvider.IsExternal == false && item.ServiceProvider.Name != "Other").ToList();
            foreach (CaseSmartGoalServiceProvider internalService in internalServiceList)
            {
                if (existingInternalServiceList != null)
                {
                    foreach (CaseSmartGoalServiceProvider existingInternalService in existingInternalServiceList)
                    {
                        if (internalService.ServiceProviderID == existingInternalService.ServiceProviderID)
                        {
                            internalService.ID = existingInternalService.ID;
                            internalService.IsUsed = existingInternalService.IsUsed;
                            break;
                        }
                    }
                }
            }
            ViewBag.UsedInternalService = internalServiceList;


            List<CaseSmartGoalServiceProvider> externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
            List<CaseSmartGoalServiceProvider> existingExternalServiceList = casesmartgoalserviceproviderRepository.AllIncluding(casesmartgoalId).Where(item => item.ServiceProvider.IsExternal == true || item.ServiceProvider.Name == "Other").ToList();
            foreach (CaseSmartGoalServiceProvider externalService in externalServiceList)
            {
                if (existingExternalServiceList != null)
                {
                    foreach (CaseSmartGoalServiceProvider existingExternalService in existingExternalServiceList)
                    {
                        if (externalService.ServiceProviderID == existingExternalService.ServiceProviderID)
                        {
                            externalService.ID = existingExternalService.ID;
                            externalService.IsUsed = existingExternalService.IsUsed;
                            break;
                        }
                    }
                }
            }
            if (externalServiceList != null && externalServiceList.Count > 0)
            {
                var externalProvidersCount = externalServiceList.Count;
                var index = externalServiceList.FindIndex(x => x.ServiceProviderName == "Other");
                if (index >= 0)
                {
                    var otherServiceaProvider = externalServiceList[index];
                    externalServiceList[index] = externalServiceList[externalProvidersCount - 1];
                    externalServiceList[externalProvidersCount - 1] = otherServiceaProvider;
                }
                else
                {
                    ServiceProvider serviceProvider = new ServiceProvider();
                    serviceProvider.Name = "Other";
                    serviceproviderRepository.InsertOrUpdate(serviceProvider);
                    serviceproviderRepository.Save();
                    externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true || item.Name == "Other").OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
                    var index1 = externalServiceList.FindIndex(x => x.ServiceProviderName == "Other");
                    if (index1 >= 0)
                    {
                        var otherServiceaProvider = externalServiceList[index1];
                        externalServiceList[index1] = externalServiceList[externalProvidersCount - 1];
                        externalServiceList[externalProvidersCount - 1] = otherServiceaProvider;
                    }
                }
            }
            ViewBag.UsedExternalService = externalServiceList;
            List<CaseSmartGoalAssignment> smartGoalList = smartgoalRepository.FindAllByCategoryID(casesmartgoal.QualityOfLifeCategoryID);
            List<CaseSmartGoalAssignment> existingSmartGoalList = casesmartgoalRepository.FindAllCaseSmartGoalAssignmentByCaseSmargGoalID(casesmartgoal.ID);
            foreach (CaseSmartGoalAssignment smartGoal in smartGoalList)
            {
                foreach (CaseSmartGoalAssignment existingSmartGoal in existingSmartGoalList)
                {
                    if (smartGoal.SmartGoalID == existingSmartGoal.SmartGoalID)
                    {
                        smartGoal.StartDate = existingSmartGoal.StartDate;
                        smartGoal.EndDate = existingSmartGoal.EndDate;
                        smartGoal.Comment = existingSmartGoal.Comment;
                        smartGoal.SmartGoalOther = existingSmartGoal.SmartGoalOther;
                        smartGoal.Checked = "checked";
                    }
                }
            }
            ViewBag.SmartGoalList = smartGoalList;
            //Session["SmartGoalIDs"] = String.Join(",", casesmartgoalRepository.FindAllCaseSmartGoalAssignmentByCaseSmargGoalID(casesmartgoal.ID).Select(item => item.SmartGoalID));
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                casesmartgoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(casesmartgoal.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(casesmartgoal);
        }

        /// <summary>
        /// This action saves an existing casesmartgoal to database
        /// </summary>
        /// <param name="casesmartgoal">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseSmartGoal casesmartgoal, int caseGoalId, int caseId, int? caseMemberId, int? qualityOfLifeCategoryID)
        {
            casesmartgoal.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            if (caseMemberId.HasValue)
            {
                casesmartgoal.CaseMemberID = caseMemberId.Value;
            }
            try
            {
                //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                //var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoal.CaseID);
                //List<CaseWorker> caseworker = caseworkerRepository.FindAllByCaseID(caseId).Where(x => x.WorkerID == CurrentLoggedInWorker.ID).ToList();
                //if ((caseworker == null || caseworker.Count() == 0) && casesmartgoal.ID > 0 && casesmartgoal.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                //{
                //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                //    //return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                //}
                //</JL:Comment:07/08/2017>
                if (casesmartgoal.StartDate > casesmartgoal.EndDate)
                {
                    throw new CustomException("Start date can't be greater than end date.");
                }
              
                CaseGoal caseGoal = casegoalRepository.Find(casesmartgoal.CaseGoalID);
                if (caseGoal != null && casesmartgoal.StartDate < caseGoal.StartDate)
                {
                    throw new CustomException("The target effective date of measurable goal can't be before the start date of Identified Goal");
                }
                if (caseGoal != null && casesmartgoal.EndDate > caseGoal.EndDate)
                {
                    throw new CustomException("The target end date of measurable goal can't be after the end date of Identified Goal");
                }
                casesmartgoal.UsedInternalServiceProviderIDs = Request.Form["UsedInternalServiceProviderIDs"].ToString(true);
                casesmartgoal.ProposedInternalServiceProviderIDs = Request.Form["ProposedInternalServiceProviderIDs"].ToString(true);
                if (casesmartgoal.UsedInternalServiceProviderIDs.IsNotNullOrEmpty() && casesmartgoal.ProposedInternalServiceProviderIDs.IsNotNullOrEmpty())
                {
                    string[] arrayUsedInternalServiceProviderIDs = casesmartgoal.UsedInternalServiceProviderIDs.ToStringArray(',');
                    string[] arrayProposedInternalServiceProviderIDs = casesmartgoal.ProposedInternalServiceProviderIDs.ToStringArray(',');
                    foreach (string usedInternalServiceProviderID in arrayUsedInternalServiceProviderIDs)
                    {
                        foreach (string proposedInternalServiceProviderID in arrayProposedInternalServiceProviderIDs)
                        {
                            if (usedInternalServiceProviderID == proposedInternalServiceProviderID)
                            {
                                throw new CustomException("You can't select the same internal service provider for both used and proposed.");
                            }
                        }
                    }
                }
                casesmartgoal.UsedExternalServiceProviderIDs = Request.Form["UsedExternalServiceProviderIDs"].ToString(true);
                casesmartgoal.ProposedExternalServiceProviderIDs = Request.Form["ProposedExternalServiceProviderIDs"].ToString(true);
                if (casesmartgoal.UsedExternalServiceProviderIDs.IsNotNullOrEmpty() && casesmartgoal.ProposedExternalServiceProviderIDs.IsNotNullOrEmpty())
                {
                    string[] arrayUsedExternalServiceProviderIDs = casesmartgoal.UsedExternalServiceProviderIDs.ToStringArray(',');
                    string[] arrayProposedExternalServiceProviderIDs = casesmartgoal.ProposedExternalServiceProviderIDs.ToStringArray(',');
                    foreach (string usedExternalServiceProviderID in arrayUsedExternalServiceProviderIDs)
                    {
                        foreach (string proposedExternalServiceProviderID in arrayProposedExternalServiceProviderIDs)
                        {
                            if (usedExternalServiceProviderID == proposedExternalServiceProviderID)
                            {
                                throw new CustomException("You can't select the same external service provider for both used and proposed.");
                            }
                        }
                    }
                }

                casesmartgoal.SmartGoalIDs = Request.Form["SmartGoalIDs"].ToString(true);
                Session["SmartGoalIDs"] = casesmartgoal.SmartGoalIDs;
                string[] blankArray={};
                string[] arraySmargGoalIDs = (!string.IsNullOrEmpty(casesmartgoal.SmartGoalIDs) ? casesmartgoal.SmartGoalIDs.ToStringArray(',') : blankArray);
                foreach (string smargGoalID in arraySmargGoalIDs)
                {
                    if (Request.Form["SmartGoalStartDate" + smargGoalID] != null && Request.Form["SmartGoalStartDate" + smargGoalID].IsNotNullOrEmpty())
                    {
                        DateTime startDate = Request.Form["SmartGoalStartDate" + smargGoalID].ToDateTime();
                        if (startDate < casesmartgoal.StartDate)
                        {
                            throw new CustomException("Start date of measurable goal can't be less than the overall start date.");
                        }
                        if (startDate > casesmartgoal.EndDate)
                        {
                            throw new CustomException("Start date of measurable goal can't be greater than the overall end date.");
                        }
                    }
                    if (Request.Form["SmartGoalEndDate" + smargGoalID] != null && Request.Form["SmartGoalEndDate" + smargGoalID].IsNotNullOrEmpty())
                    {
                        DateTime endDate = Request.Form["SmartGoalEndDate" + smargGoalID].ToDateTime();
                        if (endDate < casesmartgoal.StartDate)
                        {
                            throw new CustomException("End date of measurable goal can't be less than the overall start date.");
                        }
                        if (endDate > casesmartgoal.EndDate)
                        {
                            throw new CustomException("End date of measurable goal can't be greater than the overall end date.");
                        }
                    }
                }
                //validate data
                if (ModelState.IsValid)
                {   
                    //call the repository function to save in database
                    casesmartgoalRepository.InsertOrUpdate(casesmartgoal, Request.Form, false);
                    casesmartgoalRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Actions.ServiceProvider, Constants.Controllers.CaseSmartGoal, new { caseId = caseId, caseMemberId = casesmartgoal.CaseMemberID });
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            casesmartgoal.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (casesmartgoal.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                casesmartgoal.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                casesmartgoal.ErrorMessage = Constants.Messages.UnhandelledError;
            }

            List<CaseSmartGoalServiceProvider> internalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == false).OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
            List<Int32> existingUsedInternalServiceList = new List<Int32>();
            if (casesmartgoal.UsedInternalServiceProviderIDs.IsNotNullOrEmpty())
            {
                existingUsedInternalServiceList = casesmartgoal.UsedInternalServiceProviderIDs.ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            List<Int32> existingProposedInternalServiceList = new List<Int32>();
            if (casesmartgoal.ProposedInternalServiceProviderIDs.IsNotNullOrEmpty())
            {
                existingProposedInternalServiceList = casesmartgoal.ProposedInternalServiceProviderIDs.ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            foreach (CaseSmartGoalServiceProvider internalService in internalServiceList)
            {
                if (existingUsedInternalServiceList != null)
                {
                    foreach (int existingInternalServiceID in existingUsedInternalServiceList)
                    {
                        if (internalService.ServiceProviderID == existingInternalServiceID)
                        {
                            internalService.ID = existingInternalServiceID;
                            internalService.IsUsed = true;
                            break;
                        }
                    }
                }

                if (existingProposedInternalServiceList != null)
                {
                    foreach (int existingInternalServiceID in existingProposedInternalServiceList)
                    {
                        if (internalService.ServiceProviderID == existingInternalServiceID)
                        {
                            internalService.ID = existingInternalServiceID;
                            internalService.IsUsed = false;
                            break;
                        }
                    }
                }
            }
            ViewBag.UsedInternalService = internalServiceList;


            List<CaseSmartGoalServiceProvider> externalServiceList = serviceproviderRepository.All.Where(item => item.IsExternal == true).OrderBy(item => item.Name).AsEnumerable().Select(item => new CaseSmartGoalServiceProvider() { ServiceProviderName = item.Name, ServiceProviderID = item.ID }).ToList();
            List<Int32> existingUsedExternalServiceList = new List<Int32>();
            if (casesmartgoal.UsedExternalServiceProviderIDs.IsNotNullOrEmpty())
            {
                existingUsedExternalServiceList = casesmartgoal.UsedExternalServiceProviderIDs.ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            List<Int32> existingProposedExternalServiceList = new List<Int32>();
            if (casesmartgoal.ProposedExternalServiceProviderIDs.IsNotNullOrEmpty())
            {
                existingProposedExternalServiceList = casesmartgoal.ProposedExternalServiceProviderIDs.ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            foreach (CaseSmartGoalServiceProvider externalService in externalServiceList)
            {
                if (existingUsedExternalServiceList != null)
                {
                    foreach (int existingExternalServiceID in existingUsedExternalServiceList)
                    {
                        if (externalService.ServiceProviderID == existingExternalServiceID)
                        {
                            externalService.ID = existingExternalServiceID;
                            externalService.IsUsed = true;
                            break;
                        }
                    }
                }

                if (existingProposedExternalServiceList != null)
                {
                    foreach (int existingExternalServiceID in existingProposedExternalServiceList)
                    {
                        if (externalService.ServiceProviderID == existingExternalServiceID)
                        {
                            externalService.ID = existingExternalServiceID;
                            externalService.IsUsed = false;
                            break;
                        }
                    }
                }
            }
            ViewBag.UsedExternalService = externalServiceList;
            LoadSmartGoal(casesmartgoal.QualityOfLifeCategoryID);
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                casesmartgoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(casesmartgoal.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return view with error message if the operation is failed
            return View(casesmartgoal);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId, int? caseMemberId,bool isClosed)
        {
            DataSourceResult result = casesmartgoalRepository.Search(dsRequest, caseId, CurrentLoggedInWorker.ID, caseMemberId, isClosed);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens casesmartgoal editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">casesmartgoal id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string caseId)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseSmartGoal casesmartgoal = null;
            if (id > 0)
            {
                //find an existing casesmartgoal from database
                casesmartgoal = casesmartgoalRepository.Find(id);
                if (casesmartgoal == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Smart Goal not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                casesmartgoal = new CaseSmartGoal();
            }

            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleCaseMembers = casememberRepository.All;
            ViewBag.PossibleServiceLevelOutcomes = serviceleveloutcomeRepository.All;
            ViewBag.PossibleQualityOfLifeCategories = qualityoflifecategoryRepository.All;
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.EditorPopUp, casesmartgoal));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="casesmartgoal">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseSmartGoal casesmartgoal)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = casesmartgoal.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (!isNew)
                    //{
                    //    var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoal.CaseID);
                    //    if (casesmartgoal.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //    {
                    //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //    }
                    //}
                    //</JL:Comment:07/08/2017>

                    //set the id of the worker who has added/updated this record
                    casesmartgoal.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    //call repository function to save the data in database
                    casesmartgoalRepository.InsertOrUpdate(casesmartgoal);
                    casesmartgoalRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        casesmartgoal.SuccessMessage = "Case Smart Goal has been added successfully";
                    }
                    else
                    {
                        casesmartgoal.SuccessMessage = "Case Smart Goal has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    casesmartgoal.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    casesmartgoal.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        casesmartgoal.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (casesmartgoal.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (casesmartgoal.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoal) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoal) });
            }
        }

        /// <summary>
        /// delete casesmartgoal from database usign ajax operation
        /// </summary>
        /// <param name="id">casesmartgoal id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the casesmartgoal in database
            CaseSmartGoal casesmartgoal = casesmartgoalRepository.Find(id);
            if (casesmartgoal == null)
            {
                //set error message if it does not exist in database
                casesmartgoal = new CaseSmartGoal();
                casesmartgoal.ErrorMessage = "CaseSmartGoal not found";
            }
            else
            {
                try
                {

                    bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoal, Constants.Actions.Delete, true);
                    if (!hasAccess)
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    }

                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoal.CaseGoal.CaseMember.CaseID);
                    //if (casesmartgoal.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //delete casesmartgoal from database
                    casesmartgoalRepository.Delete(casesmartgoal);
                    casesmartgoalRepository.Save();
                    //set success message
                    casesmartgoal.SuccessMessage = "Case Smart Goal has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    casesmartgoal.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        casesmartgoal.SuccessMessage = "Case Smart Goal has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        casesmartgoal.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (casesmartgoal.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoal) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoal) });
            }
        }

        public JsonResult LoadQualityOfLifeCategoryListForGoalAjax(int caseGoalId)
        {
            return Json(casegoallivingconditionRepository.AllIncluding(caseGoalId).ToList().AsEnumerable().Select(item => new SelectListItem() { Text = item.QualityOfLifeCategory.Name, Value = item.QualityOfLifeCategoryID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadServiceLevelOutcomeListAjax()
        {
            return Json(serviceleveloutcomeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public void LoadSmartGoal(int? categoryId)
        {
            if (categoryId > 0)
            {
                DataSourceRequest dsRequest = new DataSourceRequest();
                List<CaseSmartGoalAssignment> smartGoalList = smartgoalRepository.FindAllByCategoryID(categoryId.Value);
                string selectedSmartGoal = Session["SmartGoalIDs"].ToString(true);
                if (selectedSmartGoal.IsNotNullOrEmpty())
                {
                    List<Int32> assignedSmartGoalList = selectedSmartGoal.ToStringArray(',', true).Select(Int32.Parse).ToList();
                    foreach (CaseSmartGoalAssignment smargGoal in smartGoalList)
                    {
                        smargGoal.Checked = string.Empty;
                        if (Request.Form["SmartGoalStartDate" + smargGoal.SmartGoalID] != null && Request.Form["SmartGoalStartDate" + smargGoal.SmartGoalID].IsNotNullOrEmpty() && Request.Form["SmartGoalStartDate" + smargGoal.SmartGoalID].ToDateTime().IsValidDate())
                        {
                            smargGoal.Checked = "checked";
                            smargGoal.StartDate = Request.Form["SmartGoalStartDate" + smargGoal.SmartGoalID].ToDateTime();
                        }
                        if (Request.Form["SmartGoalEndDate" + smargGoal.SmartGoalID] != null && Request.Form["SmartGoalEndDate" + smargGoal.SmartGoalID].IsNotNullOrEmpty() && Request.Form["SmartGoalEndDate" + smargGoal.SmartGoalID].ToDateTime().IsValidDate())
                        {
                            smargGoal.Checked = "checked";
                            smargGoal.EndDate = Request.Form["SmartGoalEndDate" + smargGoal.SmartGoalID].ToDateTime();
                        }
                        if (Request.Form["Comment" + smargGoal.SmartGoalID] != null && Request.Form["Comment" + smargGoal.SmartGoalID].IsNotNullOrEmpty())
                        {
                            smargGoal.Checked = "checked";
                            smargGoal.Comment = Request.Form["Comment" + smargGoal.SmartGoalID].ToString();
                        }
                        if (assignedSmartGoalList != null)
                        {
                            foreach (Int32 existingSmartGoalID in assignedSmartGoalList)
                            {
                                if (smargGoal.ID == existingSmartGoalID)
                                {
                                    smargGoal.Checked = "checked";
                                    break;
                                }
                            }
                        }
                    }
                }
                ViewBag.SmartGoalList = smartGoalList;
            }
            else
            {
                List<CaseSmartGoalAssignment> item = new List<CaseSmartGoalAssignment>();
                ViewBag.SmartGoalList = item;
            }
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult CaseAssessmentLivingConditionIndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int? CaseMemberID, int? QualityOfLifeCategoryID)
        {
            if (CaseMemberID > 0 && QualityOfLifeCategoryID > 0)
            {
                if (dsRequest.Filters == null)
                {
                    dsRequest.Filters = new List<IFilterDescriptor>();
                }
                DataSourceResult result = caseassessmentlivingconditionRepository
                    .All
                    .Where(item => item.CaseAssessment.CaseMemberID == CaseMemberID && item.QualityOfLife.QualityOfLifeSubCategory.QualityOfLifeCategoryID == QualityOfLifeCategoryID && item.Note!=null && item.Note!="")
                    .Select(item => new
                    {
                        item.ID,
                        item.Note,
                        item.LastUpdateDate
                    }).ToDataSourceResult(dsRequest);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DataSourceResult result = new DataSourceResult();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult AssignedServiceProviderAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId, int? caseMemberId)
        {
            DataSourceResult result = casesmartgoalRepository.AssignedServiceProvider(dsRequest, caseId, CurrentLoggedInWorker.ID, caseMemberId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult UnAssignedServiceProviderAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId, int? caseMemberId)
        {
            DataSourceResult result = casesmartgoalRepository.UnAssignedServiceProvider(dsRequest, caseId, CurrentLoggedInWorker.ID, caseMemberId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        public ActionResult ServiceProvider(int caseId, int? caseMemberId)
        {
            var varCase = caseRepository.Find(caseId);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseSmartGoal newCaseSmartGoal = new CaseSmartGoal();
            newCaseSmartGoal.CaseID = caseId;
            if (caseMemberId.HasValue)
            {
                newCaseSmartGoal.CaseMemberID = caseMemberId.Value;
            }

            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                CaseMember caseMember = casememberRepository.Find(caseMemberId.Value);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            return View(newCaseSmartGoal);
        }

    }
}

