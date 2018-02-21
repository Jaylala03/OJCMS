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
    public class CaseController : BaseController
    {
        private readonly ICaseRepository caseRepository;
        private readonly ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository;
        private readonly ICaseWorkerNoteRepository caseWorkerNoteRepository;
        public CaseController(IWorkerRepository workerRepository,
            IProgramRepository programRepository,
            ISubProgramRepository subprogramRepository,
            IRegionRepository regionRepository,
            IJamatkhanaRepository jamatkhanaRepository,
            IIntakeMethodRepository intakemethodRepository,
            IReferralSourceRepository referralsourceRepository,
            IHearingSourceRepository hearingsourceRepository,
            ICaseStatusRepository casestatusRepository,
            ICaseRepository caseRepository,
            IContactMethodRepository contactmethodRepository,
            ICaseWorkerNoteRepository caseWorkerNoteRepository,
            ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository,
            IRelationshipStatusRepository relationshipStatusRepository,
            IWorkerRolePermissionRepository workerRolePermissionRepository,
            IWorkerInRoleRepository workerinroleRepository,
            IWorkerInRoleNewRepository workerinrolenewRepository,
            ICaseWorkerRepository caseworkerRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseAuditLogRepository caseAuditLogRepository
            , IIncomeRangeRepository incomeRangeRepository)
            : base(workerroleactionpermissionRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.programRepository = programRepository;
            this.regionRepository = regionRepository;
            this.jamatkhanaRepository = jamatkhanaRepository;
            this.intakemethodRepository = intakemethodRepository;
            this.referralsourceRepository = referralsourceRepository;
            this.hearingsourceRepository = hearingsourceRepository;
            this.casestatusRepository = casestatusRepository;
            this.caseRepository = caseRepository;
            this.caseHouseholdIncomeRepository = caseHouseholdIncomeRepository;
            this.caseWorkerNoteRepository = caseWorkerNoteRepository;
            this.contactmethodRepository = contactmethodRepository;
            this.subprogramRepository = subprogramRepository;
            this.relationshipStatusRepository = relationshipStatusRepository;
            this.workerRolePermissionRepository = workerRolePermissionRepository;
            this.workerinroleRepository = workerinroleRepository;
            this.workerinrolenewRepository = workerinrolenewRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.caseAuditLogRepository = caseAuditLogRepository;
            this.incomeRangeRepository = incomeRangeRepository;
        }

        /// <summary>
        /// This action returns the list of Case
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCase">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, Case searchCase)
        {
            if (!ViewBag.HasAccessToCaseManagementModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //<JL:Comment:06/01/2017>
            //searchCase.HasPermissionToCreate = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Create, true);
            //<JL:Add:06/01/2017>
            //CurrentLoggedInWorkerRoleIDs = workerroleactionpermissionnewRepository.GetWorkerInRoleNew(CurrentLoggedInWorker.ID,0,0,0);
            searchCase.HasPermissionToCreate = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Create, true);

            //searchCase.HasPermissionToReadmit = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Readmit, true);
            //searchCase.HasPermissionToEdit = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Edit, true);
            //searchCase.HasPermissionToDelete = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Delete, true);
            searchCase.AssignedToWorkerID = CurrentLoggedInWorker.ID;
            return View(searchCase);
        }

        [WorkerAuthorize]
        public ActionResult MyCase([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, Case searchCase)
        {
            if (!ViewBag.HasAccessToCaseManagementModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            searchCase.HasPermissionToCreate = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Create, true);
            //searchCase.HasPermissionToReadmit = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Readmit, true);
            //searchCase.HasPermissionToEdit = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Edit, true);
            //searchCase.HasPermissionToDelete = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Delete, true);
            searchCase.AssignedToWorkerID = CurrentLoggedInWorker.ID;
            return View(searchCase);
        }

        [WorkerAuthorize]
        public ActionResult Readmit(int caseId)
        {
            //find the existing varCase from database
            Case varCase = caseRepository.Find(caseId);
            if (varCase == null)
            {
                return RedirectToAction(Constants.Actions.Create, Constants.Controllers.Case, new { area = Constants.Areas.CaseManagement });
            }
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Readmit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            ViewBag.HasAccessToAssignmentModule = false;
            ViewBag.HasAccessToIndividualModule = false;
            ViewBag.HasAccessToInitialContactModule = false;
            ViewBag.HasAccessToCaseAuditLogModule = true;
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
            {
                ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
            }
            else
            {
                ViewBag.HasAccessToAssignmentModule = true;
                ViewBag.HasAccessToIndividualModule = true;
                ViewBag.HasAccessToInitialContactModule = true;
            }
            //return editor view
            ViewBag.CaseID = caseId;
            ViewBag.DisplayID = varCase.DisplayID;
            varCase.IsReadmit = true;
            varCase.CaseNumber = varCase.DisplayID;
            return View(Constants.Views.Edit, varCase);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Readmit(Case varCase)
        {
            varCase.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {


                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    Case newCase = caseRepository.Readmit(varCase);
                    caseRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseMember, new { caseID = newCase.ID });
                }
                else
                {
                    varCase = caseRepository.Find(varCase.ID);
                    ViewBag.HasAccessToAssignmentModule = false;
                    ViewBag.HasAccessToIndividualModule = false;
                    ViewBag.HasAccessToInitialContactModule = false;
                    ViewBag.HasAccessToCaseAuditLogModule = true;
                    if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                    {
                        ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                        ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                        ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
                    }
                    else
                    {
                        ViewBag.HasAccessToAssignmentModule = true;
                        ViewBag.HasAccessToIndividualModule = true;
                        ViewBag.HasAccessToInitialContactModule = true;
                    }
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            varCase.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (varCase.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                varCase = caseRepository.Find(varCase.ID);
                ViewBag.HasAccessToAssignmentModule = false;
                ViewBag.HasAccessToIndividualModule = false;
                ViewBag.HasAccessToInitialContactModule = false;
                ViewBag.HasAccessToCaseAuditLogModule = true;
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                {
                    ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                    ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                    ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
                }
                else
                {
                    ViewBag.HasAccessToAssignmentModule = true;
                    ViewBag.HasAccessToIndividualModule = true;
                    ViewBag.HasAccessToInitialContactModule = true;
                }
                varCase.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                varCase = caseRepository.Find(varCase.ID);
                ViewBag.HasAccessToAssignmentModule = false;
                ViewBag.HasAccessToIndividualModule = false;
                ViewBag.HasAccessToInitialContactModule = false;
                ViewBag.HasAccessToCaseAuditLogModule = true;
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                {
                    ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                    ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                    ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
                }
                else
                {
                    ViewBag.HasAccessToAssignmentModule = true;
                    ViewBag.HasAccessToIndividualModule = true;
                    ViewBag.HasAccessToInitialContactModule = true;
                }
                ExceptionManager.Manage(ex);
                varCase.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if the operation is failed
            return View(varCase);
        }

        /// <summary>
        /// This action creates new varCase
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create()
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Create, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            Case varCase = new Case();
            varCase.CaseHouseholdIncome = new CaseHouseholdIncome();
            varCase.CaseHouseholdIncome.IncomeRanges = incomeRangeRepository.GetAll().ToList();
            varCase.CaseStatusID = 1;
            varCase.CaseWorkerNote = new CaseWorkerNote();
            return View(varCase);
        }

        /// <summary>
        /// This action saves new varCase to database
        /// </summary>
        /// <param name="varCase">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        //  [UpdateRoleAuthorize]
        [HttpPost]
        public ActionResult Create(Case varCase)
        {
            varCase.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    if ((varCase.ProgramID == 3 && !string.IsNullOrEmpty(varCase.PresentingProblem))|| varCase.ProgramID != 3 ) 
                    {
                        //call the repository function to save in database
                        caseRepository.InsertOrUpdate(varCase);
                        caseRepository.Save();

                        if (varCase.CaseHouseholdIncome.IncomeRangeID > 0)
                        {
                            varCase.CaseHouseholdIncome.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                            varCase.CaseHouseholdIncome.CaseID = varCase.ID;
                            varCase.CaseHouseholdIncome.CaseStatusID = varCase.CaseStatusID;
                            caseHouseholdIncomeRepository.InsertOrUpdate(varCase.CaseHouseholdIncome);
                            caseHouseholdIncomeRepository.Save();
                        }
                        if (varCase.CaseWorkerNote.ContactMethodID > 0)
                        {
                            varCase.CaseWorkerNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                            varCase.CaseWorkerNote.CaseID = varCase.ID;
                            varCase.CaseWorkerNote.CaseStatusID = varCase.CaseStatusID;
                            varCase.CaseWorkerNote.NoteDate = Convert.ToDateTime(varCase.ContactDate);
                            caseWorkerNoteRepository.InsertOrUpdate(varCase.CaseWorkerNote);
                            caseWorkerNoteRepository.Save();
                        }
                        var caseWorker = new CaseWorker();
                        //var workerRoleList = workerinroleRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID);
                        var workerRoleList = workerinrolenewRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID);
                        if (workerRoleList != null)
                        {
                            caseWorker.IsActive = true;
                            caseWorker.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                            caseWorker.CaseID = varCase.ID;
                            caseWorker.WorkerID = CurrentLoggedInWorker.ID;
                            caseworkerRepository.InsertOrUpdate(caseWorker);
                            caseworkerRepository.Save();
                        }
                        //redirect to list page after successful operation
                        //return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseMember, new { caseID = varCase.ID });
                        return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseSummary, new { caseID = varCase.ID });
                    }
                    else {
                        varCase.ErrorMessage = "Please enter presenting problem.";
                    }
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            varCase.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (varCase.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                varCase.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                varCase.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return View(varCase);
        }

        /// <summary>
        /// This action edits an existing varCase
        /// </summary>
        /// <param name="id">varCase id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int caseId)
        {
            Case varCase = caseRepository.Find(caseId);
            ViewBag.HasAccessToAssignmentModule = false;
            ViewBag.HasAccessToIndividualModule = false;
            ViewBag.HasAccessToInitialContactModule = false;
            ViewBag.HasAccessToCaseAuditLogModule = true;
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
            {
                ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
            }
            else
            {
                ViewBag.HasAccessToAssignmentModule = true;
                ViewBag.HasAccessToIndividualModule = true;
                ViewBag.HasAccessToInitialContactModule = true;
            }
            //find the existing varCase from database
            if (varCase == null)
            {
                return RedirectToAction(Constants.Actions.Create, Constants.Controllers.Case, new { area = Constants.Areas.CaseManagement });
            }
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //return editor view
            ViewBag.CaseID = caseId;
            ViewBag.DisplayID = varCase.DisplayID;
            return View(varCase);
        }

        [WorkerAuthorize]
        public ActionResult Read(int caseId)
        {
            //find the existing varCase from database
            Case varCase = caseRepository.Find(caseId);
            if (varCase == null)
            {
                return RedirectToAction(Constants.Actions.Create, Constants.Controllers.Case, new { area = Constants.Areas.CaseManagement });
            }

            ViewBag.HasAccessToAssignmentModule = false;
            ViewBag.HasAccessToIndividualModule = false;
            ViewBag.HasAccessToInitialContactModule = false;
            ViewBag.HasAccessToCaseAuditLogModule = true;
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
            {
                ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
            }
            else
            {
                ViewBag.HasAccessToAssignmentModule = true;
                ViewBag.HasAccessToIndividualModule = true;
                ViewBag.HasAccessToInitialContactModule = true;
            }

            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Read, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //return editor view
            ViewBag.CaseID = caseId;
            ViewBag.DisplayID = varCase.DisplayID;
            return View(varCase);
        }

        /// <summary>
        /// This action saves an existing varCase to database
        /// </summary>
        /// <param name="varCase">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        //[UpdateRoleAuthorize]
        [HttpPost]
        public ActionResult Edit(Case varCase)
        {
            varCase.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;

            try
            {
                if (varCase.ReferralDate > varCase.EnrollDate)
                {
                    throw new CustomException("Case Referal date must be before Case Start Date.");
                }
                //validate data
                if (ModelState.IsValid)
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(varCase.ID);
                    //List<CaseWorker> caseworker = caseworkerRepository.FindAllByCaseID(varCase.ID).Where(x => x.WorkerID == CurrentLoggedInWorker.ID).ToList();
                    ////call the repository function to save in database

                    //if ((caseworker == null || caseworker.Count() == 0) && varCase.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1 || CurrentLoggedInWorkerRegionIDs.IndexOf(varCase.RegionID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    caseRepository.InsertOrUpdate(varCase);
                    caseRepository.Save();
                    //Audit log

                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseMember, new { caseID = varCase.ID });
                }
                else
                {
                    varCase = caseRepository.Find(varCase.ID);
                    ViewBag.HasAccessToAssignmentModule = false;
                    ViewBag.HasAccessToIndividualModule = false;
                    ViewBag.HasAccessToInitialContactModule = false;
                    ViewBag.HasAccessToCaseAuditLogModule = true;
                    if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                    {
                        ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                        ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                        ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
                    }
                    else
                    {
                        ViewBag.HasAccessToAssignmentModule = true;
                        ViewBag.HasAccessToIndividualModule = true;
                        ViewBag.HasAccessToInitialContactModule = true;
                    }

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            varCase.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (varCase.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                varCase = caseRepository.Find(varCase.ID);
                ViewBag.HasAccessToAssignmentModule = false;
                ViewBag.HasAccessToIndividualModule = false;
                ViewBag.HasAccessToInitialContactModule = false;
                ViewBag.HasAccessToCaseAuditLogModule = true;
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                {
                    ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                    ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                    ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
                }
                else
                {
                    ViewBag.HasAccessToAssignmentModule = true;
                    ViewBag.HasAccessToIndividualModule = true;
                    ViewBag.HasAccessToInitialContactModule = true;
                }
                varCase.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                varCase = caseRepository.Find(varCase.ID);
                ViewBag.HasAccessToAssignmentModule = false;
                ViewBag.HasAccessToIndividualModule = false;
                ViewBag.HasAccessToInitialContactModule = false;
                ViewBag.HasAccessToCaseAuditLogModule = true;
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                {
                    ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                    ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                    ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
                }
                else
                {
                    ViewBag.HasAccessToAssignmentModule = true;
                    ViewBag.HasAccessToIndividualModule = true;
                    ViewBag.HasAccessToInitialContactModule = true;
                }
                ExceptionManager.Manage(ex);
                varCase.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if the operation is failed
            return View(varCase);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, Case searchCase)
        {
            if (CurrentLoggedInWorker.IsActive)
            {
                DataSourceResult result = caseRepository.Search(searchCase, dsRequest);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("");
            }
        }

        /// <summary>
        /// delete varCase from database usign ajax operation
        /// </summary>
        /// <param name="id">varCase id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the varCase in database
            BaseModel statusModel = new BaseModel();
            if (!workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Delete, true))
            {
                statusModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
            }

            Case varCase = caseRepository.Find(id);
            //bool hasAccess = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Delete, true);
            var primaryWorkerID = GetPrimaryWorkerOfTheCase(varCase.ID);
            if (varCase.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1 || CurrentLoggedInWorkerRegionIDs.IndexOf(varCase.RegionID) != -1))
            {
                statusModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
            try
            {
                //delete varCase from database
                caseRepository.Delete(id);
                caseRepository.Save();



                //set success message
                statusModel.SuccessMessage = "Case has been deleted successfully";
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                {
                    statusModel.SuccessMessage = "Case has been deleted successfully";
                }
                else
                {
                    ExceptionManager.Manage(ex);
                    statusModel.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (statusModel.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
        }

        [WorkerAuthorize]
        public ActionResult CaseAuditLog(int caseId)
        {
            Case varCase = caseRepository.Find(caseId);
            ViewBag.HasAccessToAssignmentModule = false;
            ViewBag.HasAccessToIndividualModule = false;
            ViewBag.HasAccessToInitialContactModule = false;
            ViewBag.HasAccessToCaseAuditLogModule = true;
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
            {
                ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
            }
            else
            {
                ViewBag.HasAccessToAssignmentModule = true;
                ViewBag.HasAccessToIndividualModule = true;
                ViewBag.HasAccessToInitialContactModule = true;
            }
            //find the existing varCase from database
            if (varCase == null)
            {
                return RedirectToAction(Constants.Actions.Create, Constants.Controllers.Case, new { area = Constants.Areas.CaseManagement });
            }
            if (!ViewBag.HasAccessToCaseManagementModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //return editor view
            ViewBag.CaseID = caseId;
            ViewBag.DisplayID = varCase.DisplayID;
            return View(varCase);
        }

        [WorkerAuthorize]
        public ActionResult CaseAuditLogForMember(int caseId)
        {
            Case varCase = caseRepository.Find(caseId);
            ViewBag.HasAccessToAssignmentModule = false;
            ViewBag.HasAccessToIndividualModule = false;
            ViewBag.HasAccessToInitialContactModule = false;
            ViewBag.HasAccessToCaseAuditLogModule = true;
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
            {
                ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
            }
            else
            {
                ViewBag.HasAccessToAssignmentModule = true;
                ViewBag.HasAccessToIndividualModule = true;
                ViewBag.HasAccessToInitialContactModule = true;
            }
            //find the existing varCase from database
            if (varCase == null)
            {
                return RedirectToAction(Constants.Actions.Create, Constants.Controllers.Case, new { area = Constants.Areas.CaseManagement });
            }
            if (!ViewBag.HasAccessToCaseManagementModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //return editor view
            ViewBag.CaseID = caseId;
            ViewBag.DisplayID = varCase.DisplayID;
            return View(varCase);
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult LogAjax(CaseAuditLog caseAuditLog, [DataSourceRequest] DataSourceRequest dsRequest, string CaseId, string tablename)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            var result = caseAuditLogRepository.Search(dsRequest, CaseId, tablename);
            //foreach (WorkerListViewModel o in result.Data)
            //{
            //    o.WorkerInRoleList = workerinroleRepository.FindAllByWorkerID(o.ID).AsEnumerable().Select(item => new WorkerInRole() { ID = item.ID, WorkerRoleName = item.WorkerRole.Name, ProgramName = item.Program.Name, RegionName = item.Region.Name, EffectiveFrom = item.EffectiveFrom, EffectiveTo = item.EffectiveTo }).ToList();
            //    if (o.WorkerInRoleList != null)
            //    {
            //        foreach (WorkerInRole role in o.WorkerInRoleList)
            //        {
            //            List<WorkerSubProgram> subProgramList = workersubprogramRepository.FindAllByWorkerInRoleID(role.ID);
            //            if (subProgramList != null)
            //            {
            //                foreach (WorkerSubProgram subProgram in subProgramList)
            //                {
            //                    role.SubProgramNames = role.SubProgramNames.Concate(',', subProgram.SubProgram.Name);
            //                }
            //            }
            //        }
            //    }
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Smith",
                CompanyId = 1,
                CompanyName = "Microsoft"
            }
            );
            employees.Add(new Employee()
            {
                Id = 2,
                FirstName = "Tim",
                LastName = "Smith",
                CompanyId = 2,
                CompanyName = "UbiSoft"
            }
            );
            return employees;
        }
        public JsonResult ReadEmployees([DataSourceRequest] DataSourceRequest request)
        {
            List<Employee> employess = GetAllEmployees();
            DataSourceResult result = employess.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public class Company
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public List<Company> GetAllCompanies()
        {
            List<Company> companies = new List<Company>();
            companies.Add(new Company()
            {
                Id = 1,
                Name = "Microsoft"
            }
            );
            companies.Add(new Company()
            {
                Id = 2,
                Name = "UbiSoft"
            }
            );
            return companies;
        }

        public ActionResult Employees()
        {
            ViewBag.Companies = GetAllCompanies();
            return View();
        }

        public ActionResult Editing_Inline()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateEmployee([DataSourceRequest] DataSourceRequest request, Employee employee)
        {
            if (employee != null && ModelState.IsValid)
            {
            }

            return Json(new[] { employee }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateEmployees([DataSourceRequest] DataSourceRequest request, Employee employee)
        {
            if (employee != null && ModelState.IsValid)
            {
            }

            return Json(new[] { employee }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyEmployees([DataSourceRequest] DataSourceRequest request, Employee employee)
        {
            if (employee != null)
            {
            }

            return Json(new[] { employee }.ToDataSourceResult(request, ModelState));
        }
    }


}

