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
using eCMS.Shared;
using eCMS.Web.Controllers;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseGoalNewController : CaseBaseController
    {
        private readonly ICaseGoalNewRepository caseGoalNewRepository;
        private readonly ICaseInitialAssessmentRepository caseInitialAssessmentRepository;
        //private readonly ICaseWorkerNoteRepository caseWorkerNoteRepository;
        private readonly IGoalActionWorkNoteRepository goalActionWorkNoteRepository;

        public CaseGoalNewController(ICaseGoalNewRepository CaseGoalNewRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , ICaseInitialAssessmentRepository caseInitialAssessmentRepository, ICaseMemberRepository casememberRepository,
            ICaseWorkerNoteRepository caseWorkerNoteRepository, IGoalActionWorkNoteRepository goalActionWorkNoteRepository,
            IContactMediaRepository contactmediaRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.caseGoalNewRepository = CaseGoalNewRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
            this.casememberRepository = casememberRepository;
            //this.caseWorkerNoteRepository = caseWorkerNoteRepository;
            this.goalActionWorkNoteRepository = goalActionWorkNoteRepository;
            this.contactmediaRepository = contactmediaRepository;
        }

        // GET: CaseManagement/CaseGoalNew
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, Case searchCase, int caseID)
        {
            if (!ViewBag.HasAccessToCaseManagementModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseGoalNewVM caseGoalNewVM = new CaseGoalNewVM();
            caseGoalNewVM.CaseID = caseID;
            caseGoalNewVM.CaseWorkerNote = new CaseWorkerNote();
            caseGoalNewVM.AssesmentIndicators = caseInitialAssessmentRepository.GetAllIndicators();
            caseGoalNewVM.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessment(caseID);

            return View(caseGoalNewVM);
        }

        /// <summary>
        /// This action saves new varCase to database
        /// </summary>
        /// <param name="varCase">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        //  [UpdateRoleAuthorize]
        [HttpPost]
        public ActionResult Create(CaseGoalNew varCaseGoalNew)
        {
            try
            {
                varCaseGoalNew.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                //validate data
                if (ModelState.IsValid)
                {
                    if (varCaseGoalNew.Family == "Family")
                    {
                        varCaseGoalNew.IsFamily = true;
                        varCaseGoalNew.IsFamilyMember = false;
                    }
                    else
                    {
                        varCaseGoalNew.IsFamily = false;
                        varCaseGoalNew.IsFamilyMember = true;
                    }

                    //call the repository function to save in database
                    caseGoalNewRepository.InsertOrUpdate(varCaseGoalNew);
                    caseGoalNewRepository.Save();

                    if (varCaseGoalNew.GoalActionWorkNote.ContactMethodID > 0)
                    {
                        varCaseGoalNew.GoalActionWorkNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        varCaseGoalNew.GoalActionWorkNote.CaseGoalID = varCaseGoalNew.ID;

                        varCaseGoalNew.GoalActionWorkNote.StatusID = (int)GoalWorkNote.Inprogress;

                        goalActionWorkNoteRepository.InsertOrUpdate(varCaseGoalNew.GoalActionWorkNote);
                        goalActionWorkNoteRepository.Save();
                    }

                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseSummary, new { caseID = varCaseGoalNew.CaseID });
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
                    varCaseGoalNew.ErrorMessage = "Record not saved";
                    ViewBag.MessageErr = "Record not saved";
                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseGoalNew, new { caseID = varCaseGoalNew.CaseID });
                }
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

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId)
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

            DataSourceResult result = caseGoalNewRepository.CaseGoalNewByCaseID(caseId).AsEnumerable().ToDataSourceResult(dsRequest);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action edits an existing casemember
        /// </summary>
        /// <param name="id">casegoalnew id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id, int caseId)
        {
            //CaseMember casemember = casememberRepository.Find(id);

            CaseGoalNew casegoalnew = null;
            if (id > 0)
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

                casegoalnew = caseGoalNewRepository.Find(id);
                if (casegoalnew == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Member not found");
                }
                else
                {
                }
            }
            else
            {
                //create a new instance if id is not provided
                casegoalnew = new CaseGoalNew();
                casegoalnew.CaseID = caseId.ToInteger(true);
            }

            ViewBag.ContactMediaList = contactmediaRepository.AllActiveForDropDownList;
            casegoalnew.GoalActionWorkNote = new GoalActionWorkNote();

            return View(casegoalnew);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseGoalNew varCaseGoalNew, int caseId)
        {
            if (varCaseGoalNew.CaseID == 0 && caseId > 0)
            {
                varCaseGoalNew.CaseID = caseId;
            }
            varCaseGoalNew.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                if (ModelState.IsValid)
                {
                    if (varCaseGoalNew.Family == "Family")
                    {
                        varCaseGoalNew.IsFamily = true;
                        varCaseGoalNew.IsFamilyMember = false;
                    }
                    else
                    {
                        varCaseGoalNew.IsFamily = false;
                        varCaseGoalNew.IsFamilyMember = true;
                    }

                    //call the repository function to save in database
                    caseGoalNewRepository.InsertOrUpdate(varCaseGoalNew);
                    caseGoalNewRepository.Save();

                    if (varCaseGoalNew.GoalActionWorkNote.ContactMethodID > 0)
                    {
                        varCaseGoalNew.GoalActionWorkNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        varCaseGoalNew.GoalActionWorkNote.CaseGoalID = varCaseGoalNew.ID;

                        varCaseGoalNew.GoalActionWorkNote.StatusID = (int)GoalWorkNote.Inprogress;

                        goalActionWorkNoteRepository.InsertOrUpdate(varCaseGoalNew.GoalActionWorkNote);
                        goalActionWorkNoteRepository.Save();
                    }

                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseSummary, new { caseID = varCaseGoalNew.CaseID });
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
                    varCaseGoalNew.ErrorMessage = "Record not saved";
                    ViewBag.MessageErr = "Record not saved";
                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseGoalNew, new { caseID = varCaseGoalNew.CaseID });
                }
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
    }
}