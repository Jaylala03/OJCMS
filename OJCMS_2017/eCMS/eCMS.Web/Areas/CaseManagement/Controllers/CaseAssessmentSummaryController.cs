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
    public class CaseAssessmentSummaryController : CaseBaseController
    {
        private readonly ICaseInitialAssessmentRepository caseInitialAssessmentRepository;
        private readonly ICaseWorkerNoteRepository caseWorkerNoteRepository;
        private readonly ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository;

        public CaseAssessmentSummaryController(ICaseInitialAssessmentRepository caseInitialAssessmentRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository, ICaseMemberRepository casememberRepository,
            IIncomeRangeRepository incomeRangeRepository, ICaseWorkerNoteRepository caseWorkerNoteRepository, ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
            this.caseWorkerNoteRepository = caseWorkerNoteRepository;
            this.caseHouseholdIncomeRepository = caseHouseholdIncomeRepository;
            this.incomeRangeRepository = incomeRangeRepository;
            this.casememberRepository = casememberRepository;
        }

        public ActionResult Index(int CaseID)
        {
            CaseAssessmentSummaryVM caseAssessmentSummary = new CaseAssessmentSummaryVM();

            caseAssessmentSummary.CaseID = CaseID;
            caseAssessmentSummary.AssesmentIndicators = caseInitialAssessmentRepository.GetAllIndicators();
            caseAssessmentSummary.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessment(CaseID);

            var varCase = caseRepository.Find(CaseID);
            ViewBag.DisplayID = varCase.DisplayID;
            ViewBag.CaseID = CaseID;

            return View(caseAssessmentSummary);

        }

        public JsonResult GetInitialAssessmentSummary(int CaseID, int CaseMemberID)
        {
            string result = string.Empty;
            CaseAssessmentSummaryVM caseAssessmentSummary = new CaseAssessmentSummaryVM();
            try
            {
                result = "Success";
                caseAssessmentSummary.CaseID = CaseID;
                caseAssessmentSummary.CaseMemberID = CaseMemberID;
                caseAssessmentSummary.AssesmentIndicators = caseInitialAssessmentRepository.GetAllIndicators();
                //caseAssessmentSummary.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessmentSummary(CaseID, CaseMemberID);
                //caseAssessmentSummary.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessmentSummary(CaseID, CaseMemberID);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(caseAssessmentSummary);
        }
    }
}