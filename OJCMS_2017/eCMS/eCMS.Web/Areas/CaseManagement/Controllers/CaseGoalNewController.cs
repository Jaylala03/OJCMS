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
        private readonly ICaseWorkerNoteRepository caseWorkerNoteRepository;

        public CaseGoalNewController(ICaseGoalNewRepository CaseGoalNewRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , ICaseInitialAssessmentRepository caseInitialAssessmentRepository, ICaseMemberRepository casememberRepository,
            ICaseWorkerNoteRepository caseWorkerNoteRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.caseGoalNewRepository = CaseGoalNewRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
            this.casememberRepository = casememberRepository;
            this.caseWorkerNoteRepository = caseWorkerNoteRepository;
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

                    if (varCaseGoalNew.CaseWorkerNote.ContactMethodID > 0)
                    {
                        varCaseGoalNew.CaseWorkerNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        varCaseGoalNew.CaseWorkerNote.CaseID = varCaseGoalNew.CaseID;
                        //incomemodel.CaseWorkerNote.CaseStatusID = incomemodel.CaseStatusID;
                        //incomemodel.CaseWorkerNote.ProgramID = incomemodel.ProgramID;
                        //varCase.CaseWorkerNote.NoteDate = Convert.ToDateTime(varCase.ContactDate);
                        varCaseGoalNew.CaseWorkerNote.IsFamily = true;
                        varCaseGoalNew.CaseWorkerNote.IsFamilyMember = false;
                        varCaseGoalNew.CaseWorkerNote.WorkerNoteActivityTypeID = (int)WorkerNoteActivityType.EditHouseholdIncome;

                        caseWorkerNoteRepository.InsertOrUpdate(varCaseGoalNew.CaseWorkerNote);
                        caseWorkerNoteRepository.Save();
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