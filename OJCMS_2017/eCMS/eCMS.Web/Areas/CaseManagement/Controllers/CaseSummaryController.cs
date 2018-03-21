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
    public class CaseSummaryController : CaseBaseController
    {
        private readonly ICaseSummaryRepository _casesummaryrepository;
        private readonly ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository;
        private readonly ICaseInitialAssessmentRepository caseInitialAssessmentRepository;
        public CaseSummaryController(IWorkerRepository workerRepository,
           ICaseRepository caseRepository,
           IRelationshipStatusRepository relationshipstatusRepository,
           ILanguageRepository languageRepository,
           IGenderRepository genderRepository,
           IEthnicityRepository ethnicityRepository,
           ICaseMemberEthinicity caseEthinicityRepository,
           IMaritalStatusRepository maritalstatusRepository,
           IMemberStatusRepository memberstatusRepository,
           ICaseMemberRepository casememberRepository,
           ICaseWorkerRepository caseworkerRepository,
          IWorkerRoleRepository workerRoleRepository,
           ICaseSummaryRepository caseSummaryRepository,
           IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
           , IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository
            , ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository
            , ICaseInitialAssessmentRepository caseInitialAssessmentRepository
          )
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.relationshipStatusRepository = relationshipstatusRepository;
            this.languageRepository = languageRepository;
            this.genderRepository = genderRepository;
            this.ethnicityRepository = ethnicityRepository;
            this.caseEthinicityRepository = caseEthinicityRepository;
            this.maritalStatusRepository = maritalstatusRepository;
            this.memberstatusRepository = memberstatusRepository;
            this.casememberRepository = casememberRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.workerroleRepository = workerRoleRepository;
            this._casesummaryrepository = caseSummaryRepository;
            this.caseHouseholdIncomeRepository = caseHouseholdIncomeRepository;
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
        }

        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, Case searchCase, int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseSummaryVM caseSummary = _casesummaryrepository.GetCaseDetails(caseID);
            caseSummary.caseMember = new CaseMember();
            caseSummary.caseMember.CaseID = caseID;
            caseSummary.CurrentHouseholdIncomeID = 0;

            caseSummary.CaseInitialHouseholdIncomeVM = caseHouseholdIncomeRepository.GetInitialIncomeForCaseSummary(caseID);
            caseSummary.CaseCurrentHouseholdIncomeVM = caseHouseholdIncomeRepository.GetCurrentIncomeForCaseSummary(caseID);

            if (caseSummary.CaseCurrentHouseholdIncomeVM == null && caseSummary.CaseInitialHouseholdIncomeVM != null)
            {
                caseSummary.CaseCurrentHouseholdIncomeVM = caseSummary.CaseInitialHouseholdIncomeVM;
            }
            
            if (caseSummary.CaseCurrentHouseholdIncomeVM != null)
                caseSummary.CurrentHouseholdIncomeID = caseSummary.CaseCurrentHouseholdIncomeVM.ID;

            caseSummary.AssesmentIndicators = caseInitialAssessmentRepository.GetAllIndicators();
            caseSummary.CaseInitialAssessment = caseInitialAssessmentRepository.GetCaseAssessment(caseID);

            return View(caseSummary);
        }
    }
}