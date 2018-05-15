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
    public class CaseStatusHistoryController : CaseBaseController
    {
        private readonly ICaseGoalNewRepository caseGoalNewRepository;
        private readonly ICaseInitialAssessmentRepository caseInitialAssessmentRepository;
        private readonly ICaseWorkerNoteRepository caseWorkerNoteRepository;
        private readonly ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository;
        private readonly ICaseStatusHistoryRepository caseStatusHistoryRepository;

        public CaseStatusHistoryController(ICaseGoalNewRepository CaseGoalNewRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , ICaseInitialAssessmentRepository caseInitialAssessmentRepository, ICaseMemberRepository casememberRepository,
            ICaseWorkerNoteRepository caseWorkerNoteRepository, 
            IGoalActionWorkNoteRepository goalActionWorkNoteRepository,
            ICaseStatusRepository casestatusRepository,
            IReasonsForDischargeRepository reasonsfordischargeRepository,
            ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository,
            IIncomeRangeRepository incomeRangeRepository,
            ICaseStatusHistoryRepository caseStatusHistoryRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.caseGoalNewRepository = CaseGoalNewRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
            this.casememberRepository = casememberRepository;
            this.caseWorkerNoteRepository = caseWorkerNoteRepository;
            this.casestatusRepository = casestatusRepository;
            this.reasonsfordischargeRepository = reasonsfordischargeRepository;
            this.caseHouseholdIncomeRepository = caseHouseholdIncomeRepository;
            this.incomeRangeRepository = incomeRangeRepository;
            this.caseStatusHistoryRepository = caseStatusHistoryRepository;
        }

       [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, int CaseId)
        {
            if (!ViewBag.HasAccessToCaseManagementModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseStatusHistory varCase = new CaseStatusHistory();
            varCase.CaseID = CaseId;
            varCase.CaseHouseholdIncome = new CaseHouseholdIncome();
            varCase.CaseHouseholdIncome.IncomeRanges = incomeRangeRepository.GetAll().ToList();
            varCase.CaseWorkerNote = new CaseWorkerNote();

            //DataSourceResult result = caseStatusHistoryRepository.CaseStatusByCaseID(CaseId).AsEnumerable().ToDataSourceResult(dsRequest);

            string status = caseStatusHistoryRepository.CaseStatusByCaseID(CaseId);

            int statusId = caseStatusHistoryRepository.CaseStatusIDByCaseID(CaseId);

            varCase.CaseStatus = status;

            varCase.CurrentStatusID = statusId;

            return View(varCase);
        }

        /// <summary>
        /// This action saves new varCase to database
        /// </summary>
        /// <param name="varCase">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseStatusHistory varCaseStatusHistory)
        {
            try
            {
                varCaseStatusHistory.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    caseStatusHistoryRepository.InsertOrUpdate(varCaseStatusHistory);
                    caseStatusHistoryRepository.Save();

                    varCaseStatusHistory.CaseHouseholdIncome.IncomeRanges = incomeRangeRepository.GetAll().ToList();

                    if (varCaseStatusHistory.CaseHouseholdIncome.NoOfMembers > 0)
                    {
                        varCaseStatusHistory.CaseHouseholdIncome.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        varCaseStatusHistory.CaseHouseholdIncome.CaseID = varCaseStatusHistory.CaseID;
                        caseHouseholdIncomeRepository.InsertOrUpdate(varCaseStatusHistory.CaseHouseholdIncome);
                        caseHouseholdIncomeRepository.Save();
                    }

                    if (varCaseStatusHistory.CaseWorkerNote.ContactMethodID > 0)
                    {
                        varCaseStatusHistory.CaseWorkerNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;

                        varCaseStatusHistory.CaseWorkerNote.CaseID = varCaseStatusHistory.CaseID;

                        caseWorkerNoteRepository.InsertOrUpdate(varCaseStatusHistory.CaseWorkerNote);
                        caseWorkerNoteRepository.Save();
                    }

                    caseRepository.UpdateCaseStatus(varCaseStatusHistory.CaseID, varCaseStatusHistory.StatusID, CurrentLoggedInWorker.ID);
                    caseRepository.Save();

                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseSummary, new { caseID = varCaseStatusHistory.CaseID });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            varCaseStatusHistory.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (varCaseStatusHistory.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                    varCaseStatusHistory.ErrorMessage = "Record not saved";
                    ViewBag.MessageErr = "Record not saved";
                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseStatusHistory, new { caseID = varCaseStatusHistory.CaseID });
                }
            }
            catch (CustomException ex)
            {
                varCaseStatusHistory.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                varCaseStatusHistory.ErrorMessage = Constants.Messages.UnhandelledError;
            }

            return View(varCaseStatusHistory);
        }

        public JsonResult LoadReasonsForDischargeAjax()
        {
            return Json(reasonsfordischargeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadCaseStatusExceptCurrentAjax(int statusid)
        {
            return Json(casestatusRepository.AllExceptCurrentDropDownList(statusid), JsonRequestBehavior.AllowGet);
        }
    }
}