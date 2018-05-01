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
using ChartJs.Models;

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
            //caseAssessmentSummary.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessment(CaseID);
            caseAssessmentSummary.CaseInitialAssessment = new List<CaseInitialAssessmentVM>();

            var varCase = caseRepository.Find(CaseID);
            ViewBag.DisplayID = varCase.DisplayID;
            ViewBag.CaseID = CaseID;
            ViewBag.ChartData = null;
            return View(caseAssessmentSummary);

        }
        [HttpPost]
        public ActionResult Index(int CaseID, int CaseMemberID)
        {
            CaseAssessmentSummaryVM caseAssessmentSummary = new CaseAssessmentSummaryVM();

            caseAssessmentSummary.CaseID = CaseID;
            caseAssessmentSummary.CaseMemberID = CaseMemberID;
            caseAssessmentSummary.AssesmentIndicators = caseInitialAssessmentRepository.GetAllIndicators();
            caseAssessmentSummary.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessmentSummary(CaseID, CaseMemberID);

            var chartlabels = caseAssessmentSummary.CaseInitialAssessment
                .Select(m => new { m.CreateDate }).Distinct().OrderBy(m=>m.CreateDate).ToArray();

            Chart _chart = new Chart();
            _chart.labels = new string[chartlabels.Length];

            for (int rwcnt = 0; rwcnt < chartlabels.Length; rwcnt++)
            {
                _chart.labels[rwcnt] = chartlabels[rwcnt].CreateDate.Value.Date.ToShortDateString();
            }

            //_chart.labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            int indcnt = 0;
            var colorarray = new string[] { "rgb(255, 99, 132)",
                         "rgb(255, 159, 64)",
                         "rgb(255, 205, 86)",
                         "rgb(75, 192, 192)",
                        "rgb(54, 162, 235)",
                         "rgb(153, 102, 255)",
                         "rgb(201, 203, 207)"};

            foreach (var item in caseAssessmentSummary.AssesmentIndicators)
            {
                var test = caseAssessmentSummary.CaseInitialAssessment.Where(c => c.IndicatorTypeID == item.IndicatorTypeID)
                    .OrderBy(c => c.CreateDate).Select(m=>m.AssessmentValue).ToArray();

                _dataSet.Add(new Datasets()
                {
                    label = item.IndicatorName,
                    //data = new int[] { 28, 48, 40, 19, 86, 27, 90, 20, 45, 65, 34, 22 },
                    data = test,
                    borderColor = new string[] { "rgba(75,192,192,1)" },
                    backgroundColor = new string[] { colorarray[indcnt++] },
                    borderWidth = "1"
                });

                //indcnt++;
            }
           
            _chart.datasets = _dataSet;
            ViewBag.ChartData = _chart;

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
                caseAssessmentSummary.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessmentSummary(CaseID, CaseMemberID);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(caseAssessmentSummary);
        }
    }
}