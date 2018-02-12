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
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseSmartGoalServiceProviderController : CaseBaseController
    {
        private readonly IServiceLevelOutcomeRepository serviceleveloutcomeRepository;
        private readonly IServiceProviderRepository serviceproviderRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly ICaseSmartGoalRepository casesmartgoalRepository;
        private readonly IFinancialAssistanceSubCategoryRepository financialassistancesubcategoryRepository;
        private readonly IFinancialAssistanceCategoryRepository financialassistancecategoryRepository;
        private readonly ICaseSmartGoalServiceProviderRepository casesmartgoalserviceproviderRepository;
       
        private readonly ICaseSmartGoalServiceLevelOutcomeRepository casesmartgoalserviceleveloutcomeRepository;
        private readonly ICaseAssessmentLivingConditionRepository caseassessmentlivingconditionRepository;
        public CaseSmartGoalServiceProviderController(IWorkerRepository workerRepository,
            IServiceProviderRepository serviceproviderRepository,
            IServiceRepository serviceRepository,
            ICaseSmartGoalRepository casesmartgoalRepository,
            IFinancialAssistanceSubCategoryRepository financialassistancesubcategoryRepository,
            IFinancialAssistanceCategoryRepository financialassistancecategoryRepository,
            IServiceLevelOutcomeRepository serviceleveloutcomeRepository,
            ICaseSmartGoalServiceLevelOutcomeRepository casesmartgoalserviceleveloutcomeRepository,
            ICaseSmartGoalServiceProviderRepository casesmartgoalserviceproviderRepository,
            ICaseAssessmentLivingConditionRepository caseassessmentlivingconditionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseRepository caseRepository,
            ICaseMemberRepository casememberRepository,
            IWorkerInRoleRepository workerinroleRepository, IWorkerInRoleNewRepository workerinrolenewRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.serviceleveloutcomeRepository = serviceleveloutcomeRepository;
            this.serviceproviderRepository = serviceproviderRepository;
            this.serviceRepository = serviceRepository;
            this.workerRepository = workerRepository;
            this.casesmartgoalRepository = casesmartgoalRepository;
            this.financialassistancesubcategoryRepository = financialassistancesubcategoryRepository;
            this.casesmartgoalserviceproviderRepository = casesmartgoalserviceproviderRepository;
            this.casememberRepository = casememberRepository;
            this.casesmartgoalserviceleveloutcomeRepository = casesmartgoalserviceleveloutcomeRepository;
            this.financialassistancecategoryRepository = financialassistancecategoryRepository;
            this.caseassessmentlivingconditionRepository = caseassessmentlivingconditionRepository;
            this.workerinroleRepository = workerinroleRepository;
            this.workerinrolenewRepository = workerinrolenewRepository;
        }

        [WorkerAuthorize]
        public ActionResult Index(int casesmartgoalId,int caseId, int? caseMemberId,int? casesmartgoalserviceproviderId)
        {
            //<JL:add:06/02/2017>
            var varCase = caseRepository.Find(caseId);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //<JL:add:06/02/2017>

            //create a new instance of caseSmartGoalServiceProvider
            CaseSmartGoalServiceProvider caseSmartGoalServiceProvider = new CaseSmartGoalServiceProvider();
            if (casesmartgoalserviceproviderId.HasValue)
            {
                caseSmartGoalServiceProvider = casesmartgoalserviceproviderRepository.Find(casesmartgoalserviceproviderId.Value);
                if (caseSmartGoalServiceProvider != null)
                {
                    casesmartgoalId = caseSmartGoalServiceProvider.CaseSmartGoalID;
                }
            }
            caseSmartGoalServiceProvider.CaseID = caseId;
            caseSmartGoalServiceProvider.CaseSmartGoalID = casesmartgoalId;
            if (caseMemberId.HasValue)
            {
                caseSmartGoalServiceProvider.CaseMemberID = caseMemberId.Value;
            }
            CaseSmartGoal caseSmartGoal = casesmartgoalRepository.Find(casesmartgoalId);
            if (caseSmartGoal != null)
            {
                caseSmartGoalServiceProvider.QualityOfLifeCategoryID=caseSmartGoal.QualityOfLifeCategoryID;
            }
            if (caseSmartGoalServiceProvider.CaseMemberID == 0 && caseSmartGoal != null && caseSmartGoal.CaseGoal != null)
            {
                caseSmartGoalServiceProvider.CaseMemberID = caseSmartGoal.CaseGoal.CaseMemberID;
            }
            if (caseSmartGoal != null)
            {
                List<CaseSmartGoalAssignment> goalAssignmentList = casesmartgoalRepository.FindAllCaseSmartGoalAssignmentByCaseSmargGoalID(caseSmartGoal.ID);
                if (goalAssignmentList != null)
                {
                    foreach (CaseSmartGoalAssignment goalAssignment in goalAssignmentList)
                    {
                        caseSmartGoal.SmartGoalName = caseSmartGoal.SmartGoalName.Concate(",", goalAssignment.SmartGoal.Name);
                    }
                }
            }
            if (caseSmartGoal != null && caseSmartGoal.CaseGoal != null && caseSmartGoal.CaseGoal.CaseMember != null)
            {
                caseSmartGoal.CaseMemberName = caseSmartGoal.CaseGoal.CaseMember.FirstName + " " + caseSmartGoal.CaseGoal.CaseMember.LastName;
            }
            caseSmartGoalServiceProvider.CaseSmartGoal = caseSmartGoal;
            caseSmartGoalServiceProvider.ServiceTypeID = 1;
            caseSmartGoalServiceProvider.CaseSmartGoalServiceLevelOutcome = new CaseSmartGoalServiceLevelOutcome();
            caseSmartGoalServiceProvider.CaseSmartGoalServiceLevelOutcome.CaseSmartGoalID = casesmartgoalId;

            caseSmartGoalServiceProvider.CaseAction = new CaseAction();
            //return view result
            ViewBag.ServiceProviderID = casesmartgoalserviceproviderId.ToString(true);
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseSmartGoalServiceProvider.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseSmartGoalServiceProvider.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                    if (caseMember.Case != null)
                    {
                        caseSmartGoalServiceProvider.RegionID = caseMember.Case.RegionID;
                    }
                }
            }
            else
            {
                //var varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                    caseSmartGoalServiceProvider.RegionID = varCase.RegionID;
                }
            }
            //if (!caseSmartGoalServiceProvider.IsUsed)
            //{
            //    caseSmartGoalServiceProvider.IsProposed = true;
            //}
            return View(caseSmartGoalServiceProvider);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int casesmartgoalId, int caseId, int caseMemberId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            var losting=casesmartgoalserviceproviderRepository
                .All.Where(item => item.CaseSmartGoalID == casesmartgoalId).AsEnumerable().ToList();
            DataSourceResult result = casesmartgoalserviceproviderRepository
                .All.Where(item => item.CaseSmartGoalID == casesmartgoalId).AsEnumerable().ToList()
                .Select(item => new CaseSmartGoalServiceProvider()
                {
                    ID=item.ID,
                    ServiceProviderName=item.ServiceProvider.Name,
                    ServiceProviderType=((item.ServiceProvider.IsExternal || item.ServiceProvider.Name=="Other") ? "External" : "Internal"),
                    ServiceProviderUsedProposed=(item.IsUsed ? "Used" : "Proposed"),
                    ServiceName = (item.Service != null ? item.Service.Name : string.Empty),
                    FinancialAssistanceSubCategoryName = (item.FinancialAssistanceSubCategory!=null ? item.FinancialAssistanceSubCategory.Name : string.Empty),
                    WorkerName = (item.Worker!=null ? item.Worker.FirstName+" "+item.Worker.LastName : item.WorkerName),
                    IsWorkerActive=item.IsWorkerActive,
                    IsNotificationEnabled=item.IsNotificationEnabled,
                    StartDate=item.StartDate,
                    EndDate=item.EndDate,
                    Amount=item.Amount,
                    CaseID = caseId,
                    CaseMemberID = caseMemberId
                }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// delete caseSmartGoalServiceProvider from database usign ajax operation
        /// </summary>
        /// <param name="id">caseSmartGoalServiceProvider id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the caseSmartGoalServiceProvider in database
            //find the varCase in database
            BaseModel statusModel = new BaseModel();
            if (!workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Delete, true))
            {
                statusModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
            }

            CaseSmartGoalServiceProvider caseSmartGoalServiceProvider = casesmartgoalserviceproviderRepository.Find(id);
            if (caseSmartGoalServiceProvider == null)
            {
                //set error message if it does not exist in database
                caseSmartGoalServiceProvider = new CaseSmartGoalServiceProvider();
                caseSmartGoalServiceProvider.ErrorMessage = "CaseSmartGoalServiceProvider not found";
            }
            else
            {
                try
                {
                    var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseSmartGoalServiceProvider.CaseSmartGoal.CaseGoal.CaseMember.CaseID);
                    if (caseSmartGoalServiceProvider.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    }
                    //delete caseSmartGoalServiceProvider from database
                    casesmartgoalserviceproviderRepository.Delete(caseSmartGoalServiceProvider);
                    casesmartgoalserviceproviderRepository.Save();
                    //set success message
                    caseSmartGoalServiceProvider.SuccessMessage = "Case Smart Goal Service Provider has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    caseSmartGoalServiceProvider.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        caseSmartGoalServiceProvider.SuccessMessage = "Case Smart Goal Service Provider has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        caseSmartGoalServiceProvider.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (caseSmartGoalServiceProvider.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseSmartGoalServiceProvider) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseSmartGoalServiceProvider) });
            }
        }

        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, int caseId, int casememberId)
        {
            var varCase = caseRepository.Find(caseId);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseSmartGoalServiceProvider casesmartgoalserviceprovider = null;
            if (id > 0)
            {
                //find an existing casesmartgoalserviceprovider from database
                casesmartgoalserviceprovider = casesmartgoalserviceproviderRepository.Find(id);

                if (casesmartgoalserviceprovider == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Measurable goal service provider not found");
                }

                if (casesmartgoalserviceprovider.ServiceProvider != null && (casesmartgoalserviceprovider.ServiceProvider.IsExternal || casesmartgoalserviceprovider.ServiceProvider.Name=="Other"))
                {
                    casesmartgoalserviceprovider.ServiceTypeID = 2;
                }
                else
                {
                    casesmartgoalserviceprovider.ServiceTypeID = 1;
                }
                if (casesmartgoalserviceprovider.FinancialAssistanceSubCategory != null)
                {
                    casesmartgoalserviceprovider.FinancialAssistanceCategoryID = casesmartgoalserviceprovider.FinancialAssistanceSubCategory.FinancialAssistanceCategoryID;
                }
                //casesmartgoalserviceprovider.CaseActionID = casesmartgoalserviceprovider.ID;
            }
            else
            {
                //create a new instance if id is not provided
                casesmartgoalserviceprovider = new CaseSmartGoalServiceProvider();
            }
            casesmartgoalserviceprovider.CaseID = caseId;
            casesmartgoalserviceprovider.CaseMemberID = casememberId;
            CaseMember caseMember = casememberRepository.Find(casesmartgoalserviceprovider.CaseMemberID);
            if (caseMember != null)
            {
                ViewBag.DisplayID = caseMember.DisplayID;
                if (caseMember.Case != null)
                {
                    casesmartgoalserviceprovider.RegionID = caseMember.Case.RegionID;
                }
            }
            //if (!casesmartgoalserviceprovider.IsUsed)
            //{
            //    casesmartgoalserviceprovider.IsProposed = true;
            //}
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, casesmartgoalserviceprovider));
        }
        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="casesmartgoalserviceprovider">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseSmartGoalServiceProvider casesmartgoalserviceprovider)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = casesmartgoalserviceprovider.ID == 0;

           

            //validate data
            if (ModelState.IsValid)
            {
                if (casesmartgoalserviceprovider.StartDate > casesmartgoalserviceprovider.EndDate)
                {
                    throw new CustomException("Start date can't be greater than end date.");
                }
                try
                {
                    //if (casesmartgoalserviceprovider.IsProposed)
                    //{
                    //    casesmartgoalserviceprovider.IsUsed = false;
                    //}
                    //else
                    //{
                    //    casesmartgoalserviceprovider.IsUsed = true;
                    //}
                    //set the id of the worker who has added/updated this record
                    //if (!isNew)
                    //{
                    //    var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoalserviceprovider.CaseID);
                    //    if (casesmartgoalserviceprovider.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //    {
                    //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //    }
                    //}
                    casesmartgoalserviceprovider.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    //call repository function to save the data in database
                    casesmartgoalserviceproviderRepository.InsertOrUpdate(casesmartgoalserviceprovider);
                    casesmartgoalserviceproviderRepository.Save();
                   
                    //set status message
                    if (isNew)
                    {
                        casesmartgoalserviceprovider.SuccessMessage = "Service provider has been added successfully";
                    }
                    else
                    {
                        casesmartgoalserviceprovider.SuccessMessage = "Service provider has been updated successfully";
                    }
                }
                catch (DbUpdateException)
                {
                    casesmartgoalserviceprovider.ErrorMessage = "The selected service provider is already added to this goal";
                }
                catch (CustomException ex)
                {
                    casesmartgoalserviceprovider.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    casesmartgoalserviceprovider.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        casesmartgoalserviceprovider.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (casesmartgoalserviceprovider.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (casesmartgoalserviceprovider.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalserviceprovider) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalserviceprovider) });
            }
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveCaseSmartGoalServiceLevelOutcomeAjax(CaseSmartGoalServiceLevelOutcome casesmartgoalserviceleveloutcome)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = casesmartgoalserviceleveloutcome.ID == 0;
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //if (!isNew)
                    //{
                    //    var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoalserviceleveloutcome.CaseSmartGoal.CaseGoal.CaseMember.CaseID);
                    //    if (casesmartgoalserviceleveloutcome.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //    {
                    //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //    }
                    //}
                    //set the id of the worker who has added/updated this record
                    casesmartgoalserviceleveloutcome.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    //call repository function to save the data in database
                    casesmartgoalserviceleveloutcomeRepository.InsertOrUpdate(casesmartgoalserviceleveloutcome);
                    casesmartgoalserviceleveloutcomeRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        casesmartgoalserviceleveloutcome.SuccessMessage = "Service level outcome has been added successfully";
                    }
                    else
                    {
                        casesmartgoalserviceleveloutcome.SuccessMessage = "Service level outcome has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    casesmartgoalserviceleveloutcome.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    casesmartgoalserviceleveloutcome.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        casesmartgoalserviceleveloutcome.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (casesmartgoalserviceleveloutcome.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (casesmartgoalserviceleveloutcome.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalserviceleveloutcome) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalserviceleveloutcome) });
            }
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteCaseSmartGoalServiceLevelOutcomeAjax(int id)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalServiceProvider, Constants.Actions.Delete, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the caseSmartGoalServiceProvider in database
            CaseSmartGoalServiceLevelOutcome casesmartgoalserviceleveloutcome = casesmartgoalserviceleveloutcomeRepository.Find(id);
            if (casesmartgoalserviceleveloutcome == null)
            {
                //set error message if it does not exist in database
                casesmartgoalserviceleveloutcome = new CaseSmartGoalServiceLevelOutcome();
                casesmartgoalserviceleveloutcome.ErrorMessage = "Service level outcome not found";
            }
            else
            {
                try
                {
                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoalserviceleveloutcome.CaseSmartGoal.CaseGoal.CaseMember.CaseID);
                    //if (casesmartgoalserviceleveloutcome.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //delete caseSmartGoalServiceProvider from database
                    casesmartgoalserviceleveloutcomeRepository.Delete(casesmartgoalserviceleveloutcome);
                    casesmartgoalserviceleveloutcomeRepository.Save();
                    //set success message
                    casesmartgoalserviceleveloutcome.SuccessMessage = "Service level outcome has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    casesmartgoalserviceleveloutcome.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        casesmartgoalserviceleveloutcome.SuccessMessage = "Service level outcome has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        casesmartgoalserviceleveloutcome.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (casesmartgoalserviceleveloutcome.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalserviceleveloutcome) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalserviceleveloutcome) });
            }
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult CaseSmartGoalServiceLevelOutcomeIndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int casesmartgoalId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = casesmartgoalserviceleveloutcomeRepository
                .All.Where(item => item.CaseSmartGoalID == casesmartgoalId).AsEnumerable().ToList()
                .Select(item => new CaseSmartGoalServiceLevelOutcome()
                {
                    ID = item.ID,
                    ServiceLevelOutcomeName = item.ServiceLevelOutcome.Name
                }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult LoadCaseSmartGoalServiceLevelAjax(int casesmartgoalId)
        {
            List<CaseSmartGoalServiceProvider> result = casesmartgoalserviceproviderRepository
                .All.Where(item => item.CaseSmartGoalID == casesmartgoalId).AsEnumerable().ToList()
                .Select(item => new CaseSmartGoalServiceProvider()
                {
                    ID = item.ID,
                    ServiceProviderName = item.ServiceProvider.Name
                }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadServiceLevelOutcomeListAjax()
        {
            return Json(serviceleveloutcomeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadFinancialAssistanceSubCategoryAjax(int categoryID,int? regionID)
        {
            return Json(financialassistancesubcategoryRepository.FindAllForDropDownList(categoryID, regionID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadFinancialAssistanceCategoryAjax()
        {
            return Json(financialassistancecategoryRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadServiceProviderAjax(int serviceTypeID, int? RegionID=0)
        {
            
            
            return Json(serviceproviderRepository.FindAllActiveForDropDownList(serviceTypeID, RegionID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadServiceAjax(int serviceProviderID)
        {
           
            return Json(serviceRepository.FindAllActiveForDropDownList(serviceProviderID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadWorkerByRoleIDAjax(int serviceTypeID)
        {
            int workerRoleID = 7;
            if (serviceTypeID == 2)
            {
                workerRoleID = 6;
            }
            List<SelectListItem> workerList = workerRepository.FindAllByRoleID(workerRoleID).ToList();
            if (workerList == null)
            {
                workerList = new List<SelectListItem>();
            }
            else
            {
                foreach (SelectListItem item in workerList)
                {
                    int workerID = item.Value.ToInteger();
                    //<JL:Comment:06/02/2017>
                    //List<String> workerInRoleList = workerinroleRepository.FindAllByWorkerID(workerID).GroupBy(m => m.Program.Name).Select(m => m.Key).ToList();

                    //List<String> workerInRoleList = workerinrolenewRepository.FindAllByWorkerID(workerID).GroupBy(m => m.Program.Name).Select(m => m.Key).ToList();

                    //if (workerInRoleList != null)
                    //{
                    //    string programNames = string.Empty;
                    //    foreach (String role in workerInRoleList)
                    //    {
                    //        programNames = programNames.Concate(",", role);
                    //    }
                    //    item.Text = item.Text + "(" + programNames + ")";
                    //}

                    //<JL:Add:06/02/2017>
                    string programNames = string.Empty;
                    programNames =  workerinrolenewRepository.FindAllActiveProgramByWorkerID(workerID);
                    item.Text = item.Text + "(" + programNames + ")";
                }
            }
            workerList.Add(new SelectListItem() { Text="Other", Value="0" });
            return Json(workerList, JsonRequestBehavior.AllowGet);
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
                    .Where(item => item.CaseAssessment.CaseMemberID == CaseMemberID && item.QualityOfLife.QualityOfLifeSubCategory.QualityOfLifeCategoryID == QualityOfLifeCategoryID && item.Note != null && item.Note != "")
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
    }
}

