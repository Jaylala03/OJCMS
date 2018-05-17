using eCMS.Shared;
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
using eCMS.DataLogic.ViewModels;
using eCMS.ExceptionLoging;
using eCMS.Web.Controllers;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseActionNewController : CaseBaseController
    {
        private readonly ICaseGoalNewRepository caseGoalNewRepository;
        private readonly ICaseInitialAssessmentRepository caseInitialAssessmentRepository;
        private readonly IGoalActionWorkNoteRepository goalActionWorkNoteRepository;
        private readonly IGoalAssigneeRoleRepository goalassigneeroleRepository;
        private readonly ICaseActionNewRepository caseactionnewRepository;

        public CaseActionNewController(ICaseGoalNewRepository CaseGoalNewRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , ICaseInitialAssessmentRepository caseInitialAssessmentRepository, ICaseMemberRepository casememberRepository,
            ICaseWorkerNoteRepository caseWorkerNoteRepository, IGoalActionWorkNoteRepository goalActionWorkNoteRepository,
            IContactMediaRepository contactmediaRepository, IGoalStatusRepository goalstatusRepository,
            IGoalAssigneeRoleRepository goalassigneeroleRepository, ICaseActionNewRepository caseactionnewRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.caseGoalNewRepository = CaseGoalNewRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
            this.casememberRepository = casememberRepository;
            this.goalActionWorkNoteRepository = goalActionWorkNoteRepository;
            this.contactmediaRepository = contactmediaRepository;
            this.goalstatusRepository = goalstatusRepository;
            this.goalassigneeroleRepository = goalassigneeroleRepository;
            this.caseactionnewRepository = caseactionnewRepository;
        }

        [WorkerAuthorize]
        // GET: CaseManagement/CaseActionNew
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, int caseId, int caseGoalId)
        {
            CaseGoalEditVM casegoalnew = null;
            if (caseGoalId > 0)
            {
                //find an existing casemember from database
                Case varCase = caseRepository.Find(Convert.ToInt32(caseId));
                ViewBag.DisplayID = varCase.DisplayID;
                ViewBag.CaseID = caseId;
                bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(Convert.ToInt32(caseId), CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Edit, true);
                if (!hasAccess)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                }

                //casegoalnew = caseGoalNewRepository.Find(caseGoalId);
                casegoalnew = caseGoalNewRepository.GetCaseGoal(caseGoalId);
                if (casegoalnew == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case goal not found");
                }
                else
                {
                }
            }
            else
            {
                //create a new instance if id is not provided
                //casegoalnew = new CaseGoalNew();
                //casegoalnew.CaseID = caseId.ToInteger(true);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case goal not found");
            }

            ViewBag.ContactMediaList = contactmediaRepository.AllActiveForDropDownList;
            casegoalnew.GoalActionWorkNote = new GoalActionWorkNote();

            return View(casegoalnew);
        }

        [WorkerAuthorize]
        // GET: CaseManagement/CaseActionNew
        public ActionResult Create(int caseId, int caseGoalId)
        {
<<<<<<< HEAD
            CaseActionNew caseactionnew = new CaseActionNew();
=======
            CaseActionNew caseactionnew = null;
>>>>>>> refs/remotes/origin/CaseGoalChanges
            if (caseGoalId > 0)
            {
                //find an existing casemember from database
                Case varCase = caseRepository.Find(Convert.ToInt32(caseId));
                ViewBag.DisplayID = varCase.DisplayID;
                ViewBag.CaseID = caseId;
                caseactionnew.CaseID = caseId;
                caseactionnew.CaseGoalID = caseGoalId;
                caseactionnew.RegionID = varCase.RegionID;

                bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(Convert.ToInt32(caseId), CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Edit, true);
                if (!hasAccess)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                }

                //casegoalnew = caseGoalNewRepository.Find(caseGoalId);
                CaseGoalEditVM casegoalnew = caseGoalNewRepository.GetCaseGoal(caseGoalId);
                if (casegoalnew == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case goal not found");
                }
                else
                {
                }
            }
            else
            {
                //create a new instance if id is not provided
                //casegoalnew = new CaseGoalNew();
                //casegoalnew.CaseID = caseId.ToInteger(true);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case goal not found");
            }

            ViewBag.ContactMediaList = contactmediaRepository.AllActiveForDropDownList;
            caseactionnew.GoalActionWorkNote = new GoalActionWorkNote();

            return View(caseactionnew);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseActionNew varCaseGoalNew)
        {
            varCaseGoalNew.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
<<<<<<< HEAD
                bool isvalid = true;
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    if (varCaseGoalNew.CaseMemberID.HasValue && varCaseGoalNew.CaseMemberID.Value > 0)
                    {
                        if (!varCaseGoalNew.FamilyAgreeToAction)
                        {
                            varCaseGoalNew.ErrorMessage = "Please confirm family member has agreed to the Action.";
                            isvalid = false;
                        }
                    }
                    else if (varCaseGoalNew.GoalAssigneeRoleID > 0)
                    {
                        if (!varCaseGoalNew.CaseMemberID.HasValue && !varCaseGoalNew.ServiceProviderID.HasValue
                            && !varCaseGoalNew.WorkerID.HasValue && string.IsNullOrEmpty(varCaseGoalNew.ServiceProviderOther)
                            && string.IsNullOrEmpty(varCaseGoalNew.AssigneeOther) && string.IsNullOrEmpty(varCaseGoalNew.SubjectMatterExpertOther))
                        {
                            varCaseGoalNew.ErrorMessage = "Please select or enter assignee to (service provider / family member / subject matter expert / other.)";
                            isvalid = false;
                        }
                        if (varCaseGoalNew.GoalAssigneeRoleID == 5 && string.IsNullOrEmpty(varCaseGoalNew.AssigneeOther))
                        {
                            varCaseGoalNew.ErrorMessage = "Please enter assignee to other.)";
                            isvalid = false;
                        }
                        if (varCaseGoalNew.WorkerID == 0 && string.IsNullOrEmpty(varCaseGoalNew.SubjectMatterExpertOther))
                        {
                            varCaseGoalNew.ErrorMessage = "Please enter assignee to other.)";
                            isvalid = false;
                        }
                        if (varCaseGoalNew.ServiceProviderID == 56 && string.IsNullOrEmpty(varCaseGoalNew.ServiceProviderOther))
                        {
                            varCaseGoalNew.ErrorMessage = "Please enter assignee to other.)";
                            isvalid = false;
                        }
                    }
                    if (isvalid)
                    {
                        if(!varCaseGoalNew.ActionStatusID.HasValue)
                            varCaseGoalNew.ActionStatusID = (int)GoalWorkNote.Inprogress;

                        caseactionnewRepository.InsertOrUpdate(varCaseGoalNew);
                        caseactionnewRepository.Save();

                        if (varCaseGoalNew.GoalActionWorkNote.ContactMethodID > 0)
                        {
                            varCaseGoalNew.GoalActionWorkNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                            varCaseGoalNew.GoalActionWorkNote.IsAction = true;
                            varCaseGoalNew.GoalActionWorkNote.CaseGoalID = varCaseGoalNew.CaseGoalID;
                            varCaseGoalNew.GoalActionWorkNote.CaseActionID = varCaseGoalNew.ID;

                            varCaseGoalNew.GoalActionWorkNote.StatusID = (int)GoalWorkNote.Inprogress;

                            goalActionWorkNoteRepository.InsertOrUpdate(varCaseGoalNew.GoalActionWorkNote);
                            goalActionWorkNoteRepository.Save();
                        }

                        return RedirectToAction(Constants.Actions.Create, Constants.Controllers.CaseActionNew, new { caseId = varCaseGoalNew.CaseID, caseGoalId = varCaseGoalNew.CaseGoalID });
                    }
                    
=======
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    caseactionnewRepository.InsertOrUpdate(varCaseGoalNew);
                    caseactionnewRepository.Save();

                    if (varCaseGoalNew.GoalActionWorkNote.ContactMethodID > 0)
                    {
                        varCaseGoalNew.GoalActionWorkNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        varCaseGoalNew.GoalActionWorkNote.CaseGoalID = varCaseGoalNew.ID;

                        varCaseGoalNew.GoalActionWorkNote.StatusID = (int)GoalWorkNote.Inprogress;

                        goalActionWorkNoteRepository.InsertOrUpdate(varCaseGoalNew.GoalActionWorkNote);
                        goalActionWorkNoteRepository.Save();
                    }

                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseAction, new { caseId = varCaseGoalNew.CaseID, caseGoalId = varCaseGoalNew.CaseGoalID });
>>>>>>> refs/remotes/origin/CaseGoalChanges
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            varCaseGoalNew.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (varCaseGoalNew.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
<<<<<<< HEAD
                    //return RedirectToAction(Constants.Actions.Create, Constants.Controllers.CaseActionNew, new { caseId = varCaseGoalNew.CaseID, caseGoalId = varCaseGoalNew.CaseGoalID });
                    //varCaseGoalNew.ErrorMessage = "Record not saved";
                    //ViewBag.MessageErr = "Record not saved";
                    //return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseGoalNew, new { caseID = varCaseGoalNew.CaseID });
                }

=======
                    varCaseGoalNew.ErrorMessage = "Record not saved";
                    ViewBag.MessageErr = "Record not saved";
                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseGoalNew, new { caseID = varCaseGoalNew.CaseID });
                }
>>>>>>> refs/remotes/origin/CaseGoalChanges
            }
            catch (CustomException ex)
            {
                varCaseGoalNew.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                varCaseGoalNew.ErrorMessage = Constants.Messages.UnhandelledError;
            }

            return View(varCaseGoalNew);

        }

<<<<<<< HEAD
        [WorkerAuthorize]
        // GET: CaseManagement/CaseActionNew
        public ActionResult Edit(int caseId, int caseGoalId,int caseActionId)
        {
            CaseActionNew caseactionnew = new CaseActionNew();
            if (caseGoalId > 0)
            {
                //find an existing casemember from database
                Case varCase = caseRepository.Find(Convert.ToInt32(caseId));
                ViewBag.DisplayID = varCase.DisplayID;
                ViewBag.CaseID = caseId;

                caseactionnew = caseactionnewRepository.Find(caseActionId);
                caseactionnew.OLDCaseMemberID = caseactionnew.CaseMemberID;
                caseactionnew.OLDWorkerID = caseactionnew.WorkerID;
                caseactionnew.OLDServiceProviderID = caseactionnew.ServiceProviderID;
                caseactionnew.OLDAssigneeOther = caseactionnew.AssigneeOther;
                caseactionnew.OLDSubjectMatterExpertOther = caseactionnew.SubjectMatterExpertOther;
                if (caseactionnew.ServiceProviderID == 56)
                {
                    caseactionnew.ServiceProviderOther = caseactionnew.AssigneeOther;
                }
                
                caseactionnew.RegionID = varCase.RegionID;
                caseactionnew.CaseID = caseId;
                bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(Convert.ToInt32(caseId), CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Edit, true);
                if (!hasAccess)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                }

                //casegoalnew = caseGoalNewRepository.Find(caseGoalId);
                CaseGoalEditVM casegoalnew = caseGoalNewRepository.GetCaseGoal(caseGoalId);
                if (casegoalnew == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case goal not found");
                }
                else
                {
                }
            }
            else
            {
                //create a new instance if id is not provided
                //casegoalnew = new CaseGoalNew();
                //casegoalnew.CaseID = caseId.ToInteger(true);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case goal not found");
            }

            ViewBag.ContactMediaList = contactmediaRepository.AllActiveForDropDownList;
            caseactionnew.GoalActionWorkNote = new GoalActionWorkNote();

            return View("Create",caseactionnew);
        }

=======
>>>>>>> refs/remotes/origin/CaseGoalChanges
        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult LoadWorkNotes([DataSourceRequest] DataSourceRequest dsRequest, int caseGoalId, int caseActionId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }

            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Delete, true);
            bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Read, true);
            bool IsUserAdminWorker = CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1;
            //bool IsUserRegionalManager = workerroleRepository.IsWorkerRegionalAdmin() > 0 ? true : false;//CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1;

            //FilterDescriptor newDesc = new FilterDescriptor("CaseID", FilterOperator.IsEqualTo, caseId);
            //dsRequest.Filters.Add(newDesc);

            //var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseId);

            //List<CaseGoalNew> caseGoalNew = caseGoalNewRepository.CaseGoalNewByCaseID(caseId);
            //return Json(caseGoalNew, JsonRequestBehavior.AllowGet);

            DataSourceResult result = goalActionWorkNoteRepository.Search(dsRequest, caseGoalId, caseActionId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult ServicePlanGoalActionHistory([DataSourceRequest] DataSourceRequest dsRequest, int casegoalId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }

            DataSourceResult result = caseactionnewRepository.CaseGoalActionHistory(casegoalId).AsEnumerable().ToDataSourceResult(dsRequest);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadGoalStatusAjax()
        {
            return Json(goalstatusRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadGoalAssigneeRoleAjax()
        {
            return Json(goalassigneeroleRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult EditGoalDetails(CaseGoalEditVM casegoal)
        {
            //validate data
            if (ModelState.IsValid)
            {
                try
                {
                    bool isupdated = caseGoalNewRepository.UpdateDetails(casegoal);
                    if (isupdated)
                        casegoal.SuccessMessage = "Goal details updated successfully.";
                    else
                        casegoal.SuccessMessage = "Goal details not found.";
                }
                catch (DbUpdateException ex)
                {
                    casegoal.ErrorMessage = ex.Message;
                }
                catch (CustomException ex)
                {
                    casegoal.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    casegoal.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        casegoal.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (casegoal.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (casegoal.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casegoal) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casegoal) });
            }
        }
<<<<<<< HEAD

        public JsonResult LoadCaseGoalActionsAjax(int caseGoalID)
        {
            return Json(caseactionnewRepository.GetAllActions(caseGoalID), JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(GoalActionWorkNote GoalActionWorkNote)
        {
            //validate data
            if (ModelState.IsValid)
            {
                try
                {
                    if (GoalActionWorkNote.ContactMethodID > 0)
                    {
                        GoalActionWorkNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        GoalActionWorkNote.StatusID = (int)GoalWorkNote.Inprogress;
                        goalActionWorkNoteRepository.InsertOrUpdate(GoalActionWorkNote);
                        goalActionWorkNoteRepository.Save();
                        GoalActionWorkNote.SuccessMessage = "Goal details updated successfully.";
                    }

=======

        public JsonResult LoadCaseGoalActionsAjax(int caseGoalID)
        {
            return Json(caseactionnewRepository.GetAllActions(caseGoalID), JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(GoalActionWorkNote GoalActionWorkNote)
        {
            //validate data
            if (ModelState.IsValid)
            {
                try
                {
                    if (GoalActionWorkNote.ContactMethodID > 0)
                    {
                        if (GoalActionWorkNote.IsGoal == true)
                        {
                            var Ids = Request.Url.Segments.Last();

                            GoalActionWorkNote.CaseGoalID = Convert.ToInt32(Ids);                             
                        }

                        if (GoalActionWorkNote.IsAction == true)
                        {
                            GoalActionWorkNote.CaseActionID = GoalActionWorkNote.CaseActionID;
                        }

                        GoalActionWorkNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        GoalActionWorkNote.StatusID = (int)GoalWorkNote.Inprogress;
                        goalActionWorkNoteRepository.InsertOrUpdate(GoalActionWorkNote);
                        goalActionWorkNoteRepository.Save();
                        GoalActionWorkNote.SuccessMessage = "Goal details updated successfully.";
                    }

>>>>>>> refs/remotes/origin/CaseGoalChanges
                }
                catch (DbUpdateException ex)
                {
                    GoalActionWorkNote.ErrorMessage = ex.Message;
                }
                catch (CustomException ex)
                {
                    GoalActionWorkNote.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    GoalActionWorkNote.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        GoalActionWorkNote.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (GoalActionWorkNote.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (GoalActionWorkNote.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, GoalActionWorkNote) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, GoalActionWorkNote) });
            }
        }
    }
}