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
using eCMS.DataLogic.Models.Lookup;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseAssessmentController : CaseBaseController
    {
        private readonly ICaseAssessmentRepository caseassessmentRepository;
        private readonly ICaseAssessmentLivingConditionRepository caseassessmentlivingconditionRepository;
        private readonly ICaseSmartGoalRepository casesmartgoalRepository;
        public CaseAssessmentController(IWorkerRepository workerRepository, 
            ICaseMemberRepository casememberRepository, 
            IAssessmentTypeRepository assessmenttypeRepository, 
            IMemberStatusRepository memberstatusRepository, 
            IRiskTypeRepository risktypeRepository, 
            ICaseWorkerRepository caseworkerRepository, 
            IReasonsForDischargeRepository reasonsfordischargeRepository,
            ICaseAssessmentRepository caseassessmentRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            ICaseRepository caseRepository,
            IQualityOfLifeRepository qualityoflifeRepository,
            IQualityOfLifeSubCategoryRepository qualityoflifesubcategoryRepository,
            IQualityOfLifeCategoryRepository qualityoflifecategoryRepository,
            ICaseAssessmentLivingConditionRepository caseassessmentlivingconditionRepository,
            ICaseSmartGoalRepository casesmartgoalRepository
            , IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.casememberRepository = casememberRepository;
            this.assessmenttypeRepository = assessmenttypeRepository;
            this.memberstatusRepository = memberstatusRepository;
            this.risktypeRepository = risktypeRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.reasonsfordischargeRepository = reasonsfordischargeRepository;
            this.caseassessmentRepository = caseassessmentRepository;
            this.qualityoflifeRepository = qualityoflifeRepository;
            this.qualityoflifesubcategoryRepository = qualityoflifesubcategoryRepository;
            this.qualityoflifecategoryRepository = qualityoflifecategoryRepository;
            this.caseassessmentlivingconditionRepository = caseassessmentlivingconditionRepository;
            this.casesmartgoalRepository = casesmartgoalRepository;
        }

        /// <summary>
        /// This action returns the list of CaseAssessment
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseAssessment">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, CaseAssessment searchCaseAssessment, int caseId, int? caseMemberId)
        {
            var varCase = caseRepository.Find(caseId);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            searchCaseAssessment.CaseID = caseId;
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                searchCaseAssessment.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(searchCaseAssessment.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                if (varCase == null)
                {
                    varCase = caseRepository.Find(caseId);
                }
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            searchCaseAssessment.HasPermissionToCreate = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Create, true);
            return View(searchCaseAssessment);
        }

        /// <summary>
        /// This action creates new caseassessment
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int caseId, int? caseMemberID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Create, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of caseassessment
            CaseAssessment caseassessment = new CaseAssessment();
            caseassessment.CaseID = caseId;
            caseassessment.DocumentedByID = CurrentLoggedInWorker.ID;
            caseassessment.StartDate = DateTime.Now;
            caseassessment.DischargeDate = DateTime.Now;
            caseassessment.MemberStatusID = 1;
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                caseassessment.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            WebHelper.CurrentSession.Content.Data = null;
            //return view result
            return View(caseassessment);
        }

        /// <summary>
        /// This action saves new caseassessment to database
        /// </summary>
        /// <param name="caseassessment">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseAssessment caseassessment, int caseId, int? caseMemberID)
        {
            caseassessment.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                if (caseassessment.StartDate > caseassessment.EndDate)
                {
                    throw new CustomException("Start date can't be greater than end date.");
                }
                caseassessment.QualityOfLifeIDs = Request.Form["QualityOfLifeIDs"].ToString(true);
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    caseassessmentRepository.InsertOrUpdate(caseassessment,Request.Form);
                    caseassessmentRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseassessment.CaseID, CaseMemberID = caseassessment.CaseMemberID });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseassessment.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseassessment.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                caseassessment.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseassessment.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            List<CaseAssessmentLivingCondition> selectedCaseAssessmentLivingConditionList = new List<CaseAssessmentLivingCondition>();
            string[] arraySelectedQOL = caseassessment.QualityOfLifeIDs.ToStringArray(',', true);
            if (arraySelectedQOL != null && arraySelectedQOL.Length > 0)
            {
                foreach (string qolID in arraySelectedQOL)
                {
                    CaseAssessmentLivingCondition newCaseAssessmentLivingCondition = new CaseAssessmentLivingCondition()
                    {
                        QualityOfLifeID = qolID.ToInteger(true),
                        CaseAssessmentID = caseassessment.ID,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = caseassessment.LastUpdatedByWorkerID,
                    };
                    newCaseAssessmentLivingCondition.Note = Request.Form["txtQualityOfLifeIDs" + qolID].ToString(true);
                    selectedCaseAssessmentLivingConditionList.Add(newCaseAssessmentLivingCondition);
                }
            }
            WebHelper.CurrentSession.Content.Data = selectedCaseAssessmentLivingConditionList;
            if (caseassessment.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseassessment.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return view with error message if operation is failed
            return View(caseassessment);
        }

        /// <summary>
        /// This action edits an existing caseassessment
        /// </summary>
        /// <param name="id">caseassessment id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id, int caseId, int? caseMemberID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //find the existing caseassessment from database
            CaseAssessment caseassessment = caseassessmentRepository.Find(id);
            caseassessment.CaseID = caseId;
            //if (caseassessment.AssessmentTypeID == 3)
            //{
            //    return RedirectToAction(Constants.Actions.Termination, Constants.Controllers.CaseAssessment, new { Id = caseassessment.ID, CaseID = caseassessment.CaseID, CaseMemberID = caseassessment.CaseMemberID });
            //}
            WebHelper.CurrentSession.Content.Data = caseassessmentlivingconditionRepository.FindAllByCaseAssessmentID(id).ToList();
            if (caseassessment.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseassessment.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(caseassessment);
        }

        [WorkerAuthorize]
        public ActionResult Read(int id, int caseId, int? caseMemberID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Read, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //find the existing caseassessment from database
            CaseAssessment caseassessment = caseassessmentRepository.Find(id);
            caseassessment.CaseID = caseId;
            //if (caseassessment.AssessmentTypeID == 3)
            //{
            //    return RedirectToAction(Constants.Actions.Termination, Constants.Controllers.CaseAssessment, new { Id = caseassessment.ID, CaseID = caseassessment.CaseID, CaseMemberID = caseassessment.CaseMemberID });
            //}
            WebHelper.CurrentSession.Content.Data = caseassessmentlivingconditionRepository.FindAllByCaseAssessmentID(id).ToList();
            if (caseassessment.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseassessment.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(caseassessment);
        }

        /// <summary>
        /// This action saves an existing caseassessment to database
        /// </summary>
        /// <param name="caseassessment">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseAssessment caseassessment, int caseId, int? caseMemberID)
        {
            caseassessment.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                if (caseassessment.StartDate > caseassessment.EndDate)
                {
                    throw new CustomException("Start date can't be greater than end date.");
                }
                caseassessment.QualityOfLifeIDs = Request.Form["QualityOfLifeIDs"].ToString(true);
                //validate data
                if (ModelState.IsValid)
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //List<CaseWorker> caseworker = caseworkerRepository.FindAllByCaseID(caseId).Where(x => x.WorkerID == CurrentLoggedInWorker.ID).ToList();
                    //if ((caseworker == null || caseworker.Count() == 0) && caseassessment.ID > 0 && caseassessment.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    //return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>
                    //call the repository function to save in database
                    caseassessmentRepository.InsertOrUpdate(caseassessment, Request.Form);
                    caseassessmentRepository.Save();
                    //redirect to list page after successful operation
                    //return RedirectToAction(Constants.Views.Index, new { caseId = caseassessment.CaseID, CaseMemberID=caseassessment.CaseMemberID });
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseassessment.CaseID});
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseassessment.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseassessment.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                caseassessment.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseassessment.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            try
            {
                List<CaseAssessmentLivingCondition> selectedCaseAssessmentLivingConditionList = new List<CaseAssessmentLivingCondition>();
                string[] arraySelectedQOL = caseassessment.QualityOfLifeIDs.ToStringArray(',', true);
                if (arraySelectedQOL != null && arraySelectedQOL.Length > 0)
                {
                    foreach (string qolID in arraySelectedQOL)
                    {
                        CaseAssessmentLivingCondition newCaseAssessmentLivingCondition = new CaseAssessmentLivingCondition()
                        {
                            QualityOfLifeID = qolID.ToInteger(true),
                            CaseAssessmentID = caseassessment.ID,
                            LastUpdateDate = DateTime.Now,
                            LastUpdatedByWorkerID = caseassessment.LastUpdatedByWorkerID,
                        };
                        newCaseAssessmentLivingCondition.Note = Request.Form["txtQualityOfLifeIDs" + qolID].ToString(true);
                        selectedCaseAssessmentLivingConditionList.Add(newCaseAssessmentLivingCondition);
                    }
                }
                WebHelper.CurrentSession.Content.Data = selectedCaseAssessmentLivingConditionList;
                if (caseassessment.CaseMemberID > 0)
                {
                    CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                    if (caseMember != null)
                    {
                        ViewBag.DisplayID = caseMember.DisplayID;
                    }
                }
                else
                {
                    Case varCase = caseRepository.Find(caseassessment.CaseID);
                    if (varCase != null)
                    {
                        ViewBag.DisplayID = varCase.DisplayID;
                    }
                }
            }
            catch { }
            //return view with error message if the operation is failed
            return View(caseassessment);
        }

        [WorkerAuthorize]
        public ActionResult Termination(int? id, int caseId, int? caseMemberId)
        {
            var varCase = caseRepository.Find(caseId);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Termination, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            if (!caseMemberId.HasValue || caseMemberId <= 0)
            {
                CaseMember caseMember = casememberRepository.All.FirstOrDefault(item => item.CaseID == caseId);
                if (caseMember != null)
                {
                    caseMemberId = caseMember.ID;
                }
                else
                {
                    caseMemberId = 0;
                }
            }
            //find the existing caseassessment from database
            CaseAssessment caseassessment = null;
            if (id.HasValue && id.Value > 0)
            {
                caseassessment = caseassessmentRepository.Find(id.Value);
            }
            if (caseassessment == null)
            {
                caseassessment = new CaseAssessment();
            }
            caseassessment.CaseID = caseId;
            if (caseMemberId.HasValue && caseMemberId > 0)
            {
                caseassessment.CaseMemberID = caseMemberId.Value;
            }
            List<CaseAssessment> assessmentList = caseassessmentRepository.FindAllByCaseMemberID(caseMemberId.Value);
            List<LookupBaseModel> presentingProblems = new List<LookupBaseModel>();
            List<LookupBaseModel> underlyingProblems = new List<LookupBaseModel>();
            List<LookupBaseModel> generalComments = new List<LookupBaseModel>();
            foreach (CaseAssessment assessment in assessmentList)
            {
                if (assessment.PresentingProblem.IsNotNullOrEmpty())
                {
                    presentingProblems.Add(new LookupBaseModel() { Name = assessment.PresentingProblem, CreateDate = assessment.StartDate });
                }
                if (assessment.UnderlyingProblem.IsNotNullOrEmpty())
                {
                    underlyingProblems.Add(new LookupBaseModel() { Name = assessment.UnderlyingProblem, CreateDate = assessment.StartDate });
                }
                if (assessment.GeneralComments.IsNotNullOrEmpty())
                {
                    generalComments.Add(new LookupBaseModel() { Name = assessment.GeneralComments, CreateDate = assessment.StartDate });
                }
            }
            ViewBag.PresentingProblem = presentingProblems;
            ViewBag.UnderlyingProblem = underlyingProblems;
            ViewBag.GeneralComments = generalComments;

            List<CaseAssessmentLivingCondition> assessmentQOLList = caseassessmentlivingconditionRepository.FindAllByCaseMemberID(caseMemberId.Value);
            if (assessmentQOLList == null)
            {
                assessmentQOLList = new List<CaseAssessmentLivingCondition>();
            }
            ViewBag.ImmediateNeeds = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 6);
            ViewBag.Education = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 3);
            ViewBag.Health = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 4);
            ViewBag.Housing = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 5);
            ViewBag.IncomeLivelihood = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 7);
            ViewBag.AssetsProduction = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 1);
            ViewBag.DignitySelfRespect = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 2);
            ViewBag.SocialSupport = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 8);
            //return editor view
            if (caseassessment.ID == 0)
            {
                caseassessment.AssessmentTypeID = 3;
                caseassessment.DocumentedByID = CurrentLoggedInWorker.ID;
                caseassessment.StartDate = DateTime.Now;
                caseassessment.DischargeDate = DateTime.Now;
                caseassessment.MemberStatusID = 14;
                caseassessment.CaseMemberID = caseMemberId.Value;
                caseassessment.CaseID = caseId;
                caseassessment.RiskTypeID = 1;
                caseassessment.NextAssessmentDate = DateTime.Now;
            }
            if (caseassessment.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                if (varCase == null)
                {
                    varCase = caseRepository.Find(caseassessment.CaseID);
                }
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            return View(caseassessment);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Termination(CaseAssessment caseassessment, int? id, int caseMemberId, int caseId)
        {
            caseassessment.AssessmentTypeID = 3;
            caseassessment.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                if (caseassessment.CaseMemberID > 0)
                {
                    if (casesmartgoalRepository.HasOpenSmartGoal(caseassessment.CaseMemberID))
                    {
                        throw new CustomException("You can't discharge this Family or Family Member because the Family or Family Member has open goals.");
                    }
                }
                //validate data
                if (ModelState.IsValid)
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (caseassessment.ID > 0 && caseassessment.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //        return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //    }
                    //</JL:Comment:07/08/2017>
                    caseassessment.QualityOfLifeIDs = string.Empty;
                    caseassessment.NextAssessmentDate = DateTime.Today;
                    //call the repository function to save in database
                    caseassessmentRepository.InsertOrUpdate(caseassessment, Request.Form);
                    caseassessmentRepository.Save();

                    CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                    if (caseMember != null)
                    {
                        caseMember.MemberStatusID = caseassessment.MemberStatusID;
                        casememberRepository.UpdateAndSave(caseMember);
                    }
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseassessment.CaseID });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseassessment.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseassessment.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                caseassessment.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseassessment.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            List<CaseAssessment> assessmentList = caseassessmentRepository.FindAllByCaseMemberID(caseassessment.CaseMemberID);
            List<LookupBaseModel> presentingProblems = new List<LookupBaseModel>();
            List<LookupBaseModel> underlyingProblems = new List<LookupBaseModel>();
            List<LookupBaseModel> generalComments = new List<LookupBaseModel>();
            foreach (CaseAssessment assessment in assessmentList)
            {
                if (assessment.PresentingProblem.IsNotNullOrEmpty())
                {
                    presentingProblems.Add(new LookupBaseModel() { Name = assessment.PresentingProblem, CreateDate = assessment.StartDate });
                }
                if (assessment.UnderlyingProblem.IsNotNullOrEmpty())
                {
                    underlyingProblems.Add(new LookupBaseModel() { Name = assessment.UnderlyingProblem, CreateDate = assessment.StartDate });
                }
                if (assessment.GeneralComments.IsNotNullOrEmpty())
                {
                    generalComments.Add(new LookupBaseModel() { Name = assessment.GeneralComments, CreateDate = assessment.StartDate });
                }
            }
            ViewBag.PresentingProblem = presentingProblems;
            ViewBag.UnderlyingProblem = underlyingProblems;
            ViewBag.GeneralComments = generalComments;

            List<CaseAssessmentLivingCondition> assessmentQOLList = caseassessmentlivingconditionRepository.FindAllByCaseMemberID(caseassessment.CaseMemberID);
            if (assessmentQOLList == null)
            {
                assessmentQOLList = new List<CaseAssessmentLivingCondition>();
            }
            ViewBag.ImmediateNeeds = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 6);
            ViewBag.Education = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 3);
            ViewBag.Health = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 4);
            ViewBag.Housing = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 5);
            ViewBag.IncomeLivelihood = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 7);
            ViewBag.AssetsProduction = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 1);
            ViewBag.DignitySelfRespect = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 2);
            ViewBag.SocialSupport = assessmentQOLList.Where(item => item.QualityOfLifeCategoryID == 8);
            //return view with error message if operation is failed
            if (caseassessment.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseassessment.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            return View(caseassessment);
        }

        [WorkerAuthorize]
        public ActionResult View(int id, int caseId, int? caseMemberID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //find the existing caseassessment from database
            CaseAssessment caseassessment = caseassessmentRepository.Find(id);
            caseassessment.CaseID = caseId;
            //if (caseassessment.AssessmentTypeID == 3)
            //{
            //    return RedirectToAction(Constants.Actions.Termination, Constants.Controllers.CaseAssessment, new { Id = caseassessment.ID, CaseID = caseassessment.CaseID, CaseMemberID = caseassessment.CaseMemberID });
            //}
            WebHelper.CurrentSession.Content.Data = caseassessmentlivingconditionRepository.FindAllByCaseAssessmentID(id).ToList();
            if (caseassessment.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(caseassessment.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseassessment.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(caseassessment);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId, int? caseMemberId)
        {
            DataSourceResult result = caseassessmentRepository.Search(dsRequest,caseId,CurrentLoggedInWorker.ID, caseMemberId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens caseassessment editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">caseassessment id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string casememberId)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseAssessment caseassessment = null;
            if (id > 0)
            {
                //find an existing caseassessment from database
                caseassessment = caseassessmentRepository.Find(id);
                if (caseassessment == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Member Assessment not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                caseassessment = new CaseAssessment();
                caseassessment.CaseMemberID = casememberId.ToInteger(true);
            }
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.EditorPopUp, caseassessment));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="caseassessment">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseAssessment caseassessment)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = caseassessment.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (caseassessment.ID > 0 && caseassessment.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>
                    //set the id of the worker who has added/updated this record
                    caseassessment.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    //call repository function to save the data in database
                    caseassessmentRepository.InsertOrUpdate(caseassessment);
                    caseassessmentRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        caseassessment.SuccessMessage = "Case Member Assessment has been added successfully";
                    }
                    else
                    {
                        caseassessment.SuccessMessage = "Case Member Assessment has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    caseassessment.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    caseassessment.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        caseassessment.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (caseassessment.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (caseassessment.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseassessment) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseassessment) });
            }
        }

        /// <summary>
        /// delete caseassessment from database usign ajax operation
        /// </summary>
        /// <param name="id">caseassessment id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            BaseModel statusModel = new BaseModel();
            if (!workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAssessment, Constants.Actions.Delete, true))
            {
                statusModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
            }

            //find the caseassessment in database
            CaseAssessment caseassessment = caseassessmentRepository.Find(id);
            if (caseassessment == null)
            {
                //set error message if it does not exist in database
                caseassessment = new CaseAssessment();
                caseassessment.ErrorMessage = "CaseAssessment not found";
            }
            else
            {
                
                try
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (caseassessment.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>
                    //delete caseassessment from database
                    caseassessmentRepository.Delete(caseassessment);
                    caseassessmentRepository.Save();
                    //set success message
                    caseassessment.SuccessMessage = "Case Member Assessment has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    caseassessment.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        caseassessment.SuccessMessage = "Case Member Assessment has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        caseassessment.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (caseassessment.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseassessment) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseassessment) });
            }
        }

        public ActionResult LoadQualityOfLifeAjax(int qolCategoryID)
        {
            List<QualityOfLifeSubCategory> subCategoryList = qualityoflifesubcategoryRepository.FindAllByQualityOfLifeCategoryID(qolCategoryID).AsEnumerable().Select(item => new QualityOfLifeSubCategory() { Name = item.Name, ID = item.ID, ErrorMessage = "", SuccessMessage="display:none;",Description="" }).ToList();
            if (subCategoryList != null)
            {
                foreach (QualityOfLifeSubCategory subCategory in subCategoryList)
                {
                    subCategory.QualityOfLifeList = qualityoflifeRepository.FindAllByQualityOfLifeSubCategoryID(subCategory.ID).AsEnumerable().Select(item => new QualityOfLife() { Name = item.Name, ID = item.ID, ErrorMessage = "", SuccessMessage = "display:none;", Description = "" }).ToList();
                }
            }
            if (WebHelper.CurrentSession.Content.Data != null)
            {
                List<CaseAssessmentLivingCondition> qolList = (List<CaseAssessmentLivingCondition>)WebHelper.CurrentSession.Content.Data;
                foreach (var assignedQOL in qolList)
                {
                    foreach (QualityOfLifeSubCategory qofSubCategory in subCategoryList)
                    {
                        foreach (QualityOfLife qualityOfLife in qofSubCategory.QualityOfLifeList)
                        {
                            if (assignedQOL.QualityOfLifeID == qualityOfLife.ID)
                            {
                                qualityOfLife.ErrorMessage = "checked";
                                qualityOfLife.SuccessMessage = string.Empty;
                                qualityOfLife.Description = assignedQOL.Note;
                                break;
                            }
                        }
                    }
                }
            }
            DataSourceRequest dataRequest = new DataSourceRequest();
            DataSourceResult dataResult = subCategoryList.ToDataSourceResult(dataRequest);
            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadAssignedQualityOfLifeAjax(int caseAssessmentID, int qolCategoryID)
        {
            DataSourceRequest dataRequest = new DataSourceRequest();
            DataSourceResult dataResult = caseassessmentlivingconditionRepository.FindAllByCaseAssessmentIDAndQualityOfLifeCategoryID(caseAssessmentID,qolCategoryID)
                .Select(
                caseassessment => new
                {
                    caseassessment.ID,
                    QualityOfLifeName = caseassessment.QualityOfLife.Name,
                    caseassessment.CreateDate,
                    caseassessment.CaseAssessmentID
                }).ToDataSourceResult(dataRequest);
            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadAssessmentTypeAjax()
        {
            return Json(assessmenttypeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadIndividualStatusAjax()
        {
            return Json(memberstatusRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadIndividualOpenStatusAjax()
        {
            return Json(memberstatusRepository.AllActiveOpenForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadRiskTypeAjax()
        {
            return Json(risktypeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadReasonsForDischargeAjax()
        {
            return Json(reasonsfordischargeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult LoadCaseMemberListForTerminationAjax(int caseID)
        {
            var data = casememberRepository.FindAllByCaseIDAndWorkerIDForDropDownListForTermination(caseID, CurrentLoggedInWorker.ID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadIndividualCompletedStatusAjax()
        {
            return Json(memberstatusRepository.AllActiveCompletedForDropDownList, JsonRequestBehavior.AllowGet);
        }
    }
}

