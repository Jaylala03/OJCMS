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
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseSmartGoalProgressController : CaseBaseController
    {
        private readonly ICaseSmartGoalRepository casesmartgoalRepository;
        private readonly IServiceLevelOutcomeRepository serviceleveloutcomeRepository;
        private readonly ICaseSmartGoalProgressRepository casesmartgoalprogressRepository;
        private readonly ICaseActionRepository caseactionRepository;
        public CaseSmartGoalProgressController(IWorkerRepository workerRepository, 
            ICaseSmartGoalRepository casesmartgoalRepository, 
            IServiceLevelOutcomeRepository serviceleveloutcomeRepository,
            ICaseSmartGoalProgressRepository casesmartgoalprogressRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseRepository caseRepository,
            ICaseActionRepository caseactionRepository,
            ICaseMemberRepository casememberRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.casesmartgoalRepository = casesmartgoalRepository;
            this.serviceleveloutcomeRepository = serviceleveloutcomeRepository;
            this.casesmartgoalprogressRepository = casesmartgoalprogressRepository;
            this.caseactionRepository = caseactionRepository;
            this.casememberRepository = casememberRepository;
        }

        /// <summary>
        /// This action returns the list of CaseSmartGoalProgress
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseSmartGoalProgress">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ViewResult Index(int casesmartgoalId, int caseId, int? caseMemberId)
        {
            CaseSmartGoal caseSmartGoal = casesmartgoalRepository.Find(casesmartgoalId);
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
            caseSmartGoal.CaseID = caseId;
            CaseSmartGoalProgress caseSmartGoalProgress = new CaseSmartGoalProgress();
            caseSmartGoalProgress.CaseSmartGoal = caseSmartGoal;
            caseSmartGoalProgress.CaseSmartGoalID = casesmartgoalId;
            caseSmartGoalProgress.PendingActionCount = caseactionRepository.FindPendingActionCount(casesmartgoalId);

            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseSmartGoalProgress.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseSmartGoalProgress.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                var varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            return View(caseSmartGoalProgress);
        }

        /// <summary>
        /// This action creates new casesmartgoalprogress
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int casesmartgoalId)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalProgress, Constants.Actions.Create, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //create a new instance of casesmartgoalprogress
            CaseSmartGoalProgress casesmartgoalprogress = new CaseSmartGoalProgress();
            casesmartgoalprogress.CaseSmartGoalID = casesmartgoalId;
            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleServiceLevelOutcomes = serviceleveloutcomeRepository.All;
            //return view result
            return View(casesmartgoalprogress);
        }

        /// <summary>
        /// This action saves new casesmartgoalprogress to database
        /// </summary>
        /// <param name="casesmartgoalprogress">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseSmartGoalProgress casesmartgoalprogress, int casesmartgoalId)
        {
            casesmartgoalprogress.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                if (casesmartgoalprogress.ProgressDate > DateTime.Today)
                {
                    throw new CustomException("Progress date can't be future date.");
                }
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    casesmartgoalprogressRepository.InsertOrUpdate(casesmartgoalprogress);
                    casesmartgoalprogressRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { casesmartgoalId = casesmartgoalId });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            casesmartgoalprogress.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (casesmartgoalprogress.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                casesmartgoalprogress.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                casesmartgoalprogress.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleServiceLevelOutcomes = serviceleveloutcomeRepository.All;
            //return view with error message if operation is failed
            return View(casesmartgoalprogress);
        }

        /// <summary>
        /// This action edits an existing casesmartgoalprogress
        /// </summary>
        /// <param name="id">casesmartgoalprogress id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalProgress, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the existing casesmartgoalprogress from database
            CaseSmartGoalProgress casesmartgoalprogress = casesmartgoalprogressRepository.Find(id);
            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleServiceLevelOutcomes = serviceleveloutcomeRepository.All;
            //return editor view
            return View(casesmartgoalprogress);
        }

        /// <summary>
        /// This action saves an existing casesmartgoalprogress to database
        /// </summary>
        /// <param name="casesmartgoalprogress">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseSmartGoalProgress casesmartgoalprogress, int casesmartgoalId)
        {
            casesmartgoalprogress.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                var casesmartgoalprogress1 = casesmartgoalprogressRepository.Find(casesmartgoalprogress.ID);
                if (casesmartgoalprogress.ProgressDate > DateTime.Today)
                {
                    throw new CustomException("Progress date can't be future date.");
                }
                //validate data
                if (ModelState.IsValid)
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoalprogress1.CaseSmartGoal.CaseGoal.CaseMember.CaseID);
                    //if (casesmartgoalprogress.ID > 0 && casesmartgoalprogress.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //   // return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //   return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>

                    //call the repository function to save in database
                    casesmartgoalprogressRepository.InsertOrUpdate(casesmartgoalprogress);
                    casesmartgoalprogressRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { casesmartgoalId = casesmartgoalId });
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            casesmartgoalprogress.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (casesmartgoalprogress.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                casesmartgoalprogress.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                casesmartgoalprogress.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleServiceLevelOutcomes = serviceleveloutcomeRepository.All;
            //return view with error message if the operation is failed
            return View(casesmartgoalprogress);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int casesmartgoalId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = casesmartgoalprogressRepository.AllIncluding(casesmartgoalId, casesmartgoalprogress => casesmartgoalprogress.ServiceLevelOutcome).Select(casesmartgoalprogress => new
            {
                casesmartgoalprogress.ID,
                ServiceLevelOutcomeName = casesmartgoalprogress.ServiceLevelOutcome.Name,
                casesmartgoalprogress.ProgressDate,
                casesmartgoalprogress.Comment,
                SmartGoalName = casesmartgoalprogress.SmartGoal!=null ? casesmartgoalprogress.SmartGoal.Name : String.Empty
            }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens casesmartgoalprogress editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">casesmartgoalprogress id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string casesmartgoalId)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalProgress, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseSmartGoalProgress casesmartgoalprogress = null;
            if (id > 0)
            {
                //find an existing casesmartgoalprogress from database
                casesmartgoalprogress = casesmartgoalprogressRepository.Find(id);
                if (casesmartgoalprogress == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Smart Goal Progress not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                casesmartgoalprogress = new CaseSmartGoalProgress();
                casesmartgoalprogress.CaseSmartGoalID = casesmartgoalId.ToInteger(true);
            }

            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleServiceLevelOutcomes = serviceleveloutcomeRepository.All;
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.EditorPopUp, casesmartgoalprogress));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="casesmartgoalprogress">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseSmartGoalProgress casesmartgoalprogress)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = casesmartgoalprogress.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {
                try
                {
                    if (casesmartgoalprogress.ProgressDate > DateTime.Today)
                    {
                        throw new CustomException("Progress date can't be future date.");
                    }
                    //if (!isNew)
                    //{
                    //    var casesmartgoalprogress1 = casesmartgoalprogressRepository.Find(casesmartgoalprogress.ID);
                    //    var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoalprogress1.CaseSmartGoal.CaseGoal.CaseMember.CaseID);
                    //    if (casesmartgoalprogress.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //    {
                    //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //    }
                    //}
                    //set the id of the worker who has added/updated this record
                    casesmartgoalprogress.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    //call repository function to save the data in database
                    casesmartgoalprogressRepository.InsertOrUpdate(casesmartgoalprogress);
                    casesmartgoalprogressRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        casesmartgoalprogress.SuccessMessage = "Case Smart Goal Progress has been added successfully";
                    }
                    else
                    {
                        casesmartgoalprogress.SuccessMessage = "Case Smart Goal Progress has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    casesmartgoalprogress.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    casesmartgoalprogress.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        casesmartgoalprogress.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (casesmartgoalprogress.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (casesmartgoalprogress.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalprogress) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalprogress) });
            }
        }

        /// <summary>
        /// delete casesmartgoalprogress from database usign ajax operation
        /// </summary>
        /// <param name="id">casesmartgoalprogress id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the casesmartgoalprogress in database
            CaseSmartGoalProgress casesmartgoalprogress = casesmartgoalprogressRepository.Find(id);
            if (casesmartgoalprogress == null)
            {
                //set error message if it does not exist in database
                casesmartgoalprogress = new CaseSmartGoalProgress();
                casesmartgoalprogress.ErrorMessage = "CaseSmartGoalProgress not found";
            }
            else
            {
                try
                {

                    bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseSmartGoalProgress, Constants.Actions.Delete, true);
                    if (!hasAccess)
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    }

                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(casesmartgoalprogress.CaseSmartGoal.CaseGoal.CaseMember.CaseID);
                    //if (casesmartgoalprogress.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //delete casesmartgoalprogress from database
                    casesmartgoalprogressRepository.Delete(casesmartgoalprogress);
                    casesmartgoalprogressRepository.Save();
                    //set success message
                    casesmartgoalprogress.SuccessMessage = "Case Smart Goal Progress has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    casesmartgoalprogress.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        casesmartgoalprogress.SuccessMessage = "Case Smart Goal Progress has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        casesmartgoalprogress.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (casesmartgoalprogress.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalprogress) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesmartgoalprogress) });
            }
        }

        public JsonResult LoadServiceLevelOutcomeListAjax()
        {
            return Json(serviceleveloutcomeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSmartGoalListAjax(int caseSmartGoalID)
        {
            return Json(casesmartgoalRepository.FindAllCaseSmartGoalAssignmentForDropDownListByCaseSmargGoalID(caseSmartGoalID), JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        public ActionResult EndGoalAjax(int casesmartgoalId, int caseId, int? caseMemberId)
        {
            //find the casesmartgoal in database
            BaseModel baseModel = new BaseModel();
            try
            {
                casesmartgoalRepository.EndGoal(casesmartgoalId);
            }
            catch (CustomException ex)
            {
                baseModel.ErrorMessage = ex.UserDefinedMessage;
            }
            if (baseModel.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, baseModel) });
            }
            else
            {
                if (caseMemberId.HasValue && caseMemberId.Value > 0)
                {
                    return Json(new { success = true, url = Url.Action(Constants.Actions.Index, Constants.Controllers.CaseGoal, new { CaseID = caseId, CaseMemberID=caseMemberId.Value }) });
                }
                else
                {
                    return Json(new { success = true, url = Url.Action(Constants.Actions.Index, Constants.Controllers.CaseGoal, new { CaseID = caseId }) });
                }
            }
        }

        [WorkerAuthorize]
        public ActionResult GetCaseSmartGoalAssignmentAjax(int SmartGoalID, int CaseSmartGoalID)
        {
            CaseSmartGoalAssignment caseSmartGoalAssignment = casesmartgoalprogressRepository.FindCaseSmartGoalAssignment(SmartGoalID, CaseSmartGoalID);
            if (caseSmartGoalAssignment!=null)
            {
                string startDate = string.Empty;
                if (caseSmartGoalAssignment.StartDate.HasValue)
                {
                    startDate = caseSmartGoalAssignment.StartDate.Value.ToShortDateString();
                }

                string endDate = string.Empty;
                if (caseSmartGoalAssignment.EndDate.HasValue)
                {
                    endDate = caseSmartGoalAssignment.EndDate.Value.ToShortDateString();
                }
                return Json(new { success = true, SmartGoalStartDate = startDate, SmartGoalEndDate = endDate }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

