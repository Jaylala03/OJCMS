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
using eCMS.Web.Areas.CaseManagement.Controllers;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using eCMS.BusinessLogic;
using System.Configuration;
using eCMS.DataLogic.ViewModels;
using eCMS.Web.Controllers;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseInitialAssessmentController : CaseBaseController
    {
        private readonly ICaseInitialAssessmentRepository caseInitialAssessmentRepository;
        private readonly ICaseWorkerNoteRepository caseWorkerNoteRepository;
        private readonly ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository;

        public CaseInitialAssessmentController(ICaseInitialAssessmentRepository caseInitialAssessmentRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository, IIncomeRangeRepository incomeRangeRepository
            ,ICaseWorkerNoteRepository caseWorkerNoteRepository,ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {            
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
            this.caseWorkerNoteRepository = caseWorkerNoteRepository;
            this.caseHouseholdIncomeRepository = caseHouseholdIncomeRepository;
            this.incomeRangeRepository = incomeRangeRepository;
        }

        /// <summary>
        /// This action returns the list of CaseInitialAssessment
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseInitialAssessment">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index(int caseid)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            InitialAssessmentVM iniass = new InitialAssessmentVM();
            iniass.CaseID = caseid;
            iniass.AssesmentIndicators = caseInitialAssessmentRepository.GetAllIndicators();
            iniass.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessment(caseid);
            iniass.CaseAssessmentReviewed = caseInitialAssessmentRepository.CaseAssessmentReviewed(caseid);
            iniass.CaseWorkerNote = new CaseWorkerNote();
            //iniass.CaseHouseholdIncome = caseHouseholdIncomeRepository.GetCurrentIncomeForCaseSummary(caseid);
            //if (iniass.CaseHouseholdIncome == null)
                iniass.CaseHouseholdIncome = new CaseHouseholdIncome();
            iniass.CaseHouseholdIncome.IncomeRanges = incomeRangeRepository.GetAll().ToList();
            var varCase = caseRepository.Find(caseid);
            ViewBag.DisplayID = varCase.DisplayID;
            ViewBag.CaseID = caseid;
            return View(iniass);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddAssessment(int CaseID, List<CaseInitialAssessmentVM> Asslist)
        {
            string result = string.Empty;
            try
            {
                caseInitialAssessmentRepository.AddAssessment(CaseID, Asslist);
                result = "Member assessment saved successfully.";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(InitialAssessmentVM iniass, int caseId)
        {
            if (iniass.CaseID == 0 && caseId > 0)
            {
                iniass.CaseID = caseId;
            }
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    if (iniass.CaseWorkerNote.ContactMethodID > 0)
                    {
                        iniass.CaseWorkerNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        iniass.CaseWorkerNote.CaseID = iniass.CaseID;
                        iniass.CaseWorkerNote.IsFamily = true;
                        iniass.CaseWorkerNote.IsFamilyMember = false;
                        iniass.CaseWorkerNote.WorkerNoteActivityTypeID = (int)eCMS.Shared.WorkerNoteActivityType.CaseInitialAssessment;
                        caseWorkerNoteRepository.InsertOrUpdate(iniass.CaseWorkerNote);
                        caseWorkerNoteRepository.Save();
                    }
                    if (iniass.CaseHouseholdIncome.IncomeRangeID > 0)
                    {
                        iniass.CaseHouseholdIncome.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        iniass.CaseHouseholdIncome.CaseID = iniass.CaseID;
                        iniass.CaseHouseholdIncome.IsInitialIncome = false;
                        caseHouseholdIncomeRepository.InsertOrUpdate(iniass.CaseHouseholdIncome);
                        caseHouseholdIncomeRepository.Save();
                    }
                    //return RedirectToAction(Constants.Views.Index, new { caseId = caseMember.CaseID });

                    return RedirectToAction(Constants.Views.Index, new { caseid = iniass.CaseID });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            iniass.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (iniass.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                iniass.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                iniass.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if operation is failed
            iniass.AssesmentIndicators = caseInitialAssessmentRepository.GetAllIndicators();
            iniass.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessment(iniass.CaseID);
            iniass.CaseAssessmentReviewed = caseInitialAssessmentRepository.CaseAssessmentReviewed(iniass.CaseID);

            return View("Index",iniass);
        }
    }
}