using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories;
using eCMS.DataLogic.Models;
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
    public class CaseHouseholdIncomeController : CaseBaseController
    {
        private readonly ICaseHouseholdIncomeRepository caseHouseholdIncomeRepository;
        private readonly ICaseWorkerNoteRepository caseWorkerNoteRepository;
        public CaseHouseholdIncomeController(IWorkerRepository workerRepository,
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
             , IIncomeRangeRepository incomeRangeRepository,
            ICaseWorkerNoteRepository caseWorkerNoteRepository
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
            this.caseHouseholdIncomeRepository = caseHouseholdIncomeRepository;
            this.incomeRangeRepository = incomeRangeRepository;
            this.caseWorkerNoteRepository = caseWorkerNoteRepository;
        }

        [WorkerAuthorize]
        public ActionResult Edit(int CaseID, int IncomeID)
        {
            CaseHouseholdIncome caseHouseholdIncome = new CaseHouseholdIncome();
            //CaseSummaryVM caseSummary = new CaseSummaryVM();
            if (IncomeID != 0)
            {
                caseHouseholdIncome = caseHouseholdIncomeRepository.Find(IncomeID);
            }
            caseHouseholdIncome.CaseWorkerNote = new CaseWorkerNote();
            caseHouseholdIncome.CaseID = CaseID;
            caseHouseholdIncome.IncomeRanges = incomeRangeRepository.GetAll().ToList();
            //var varCase = caseRepository.Find(caseID);
            //bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
            //if (!hasAccess)
            //{
            //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
            //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            //}
            //caseSummary.caseMember = new CaseMember();
            //caseSummary.caseMember.CaseID = caseID;

            return View(caseHouseholdIncome);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseHouseholdIncome incomemodel)
        {
            incomemodel.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;

            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //if all blank then move forward
                    //if any one entered then check all entered
                    bool allentered = true;

                    if (incomemodel.NoOfMembers == 0 || incomemodel.IncomeRangeID == 0)
                    {
                        incomemodel.IncomeRanges = incomeRangeRepository.GetAll().ToList();
                        if(incomemodel.NoOfMembers == 0)
                            ModelState.AddModelError("","Please enter No. of members");
                        if(incomemodel.IncomeRangeID == 0)
                            ModelState.AddModelError("","Please select income range");
                        return  View(incomemodel);
                        //CustomException ex = new CustomException(CustomExceptionType.CommonServerError, "Please enter No. of members and income range.");
                        //throw ex;
                    }
                    if (!string.IsNullOrEmpty(incomemodel.CaseWorkerNote.Note) || incomemodel.CaseWorkerNote.NoteDate != null ||
                        incomemodel.CaseWorkerNote.TimeSpentHours != null || incomemodel.CaseWorkerNote.TimeSpentMinutes != null
                        || (incomemodel.CaseWorkerNote.ContactMethodID != null && incomemodel.CaseWorkerNote.ContactMethodID > 0))

                    {
                        incomemodel.IncomeRanges = incomeRangeRepository.GetAll().ToList();
                        var isnoteerror = false;

                        if (string.IsNullOrEmpty(incomemodel.CaseWorkerNote.Note))
                        {
                            ModelState.AddModelError("", "Please enter work note.");
                            isnoteerror = true;
                        }

                        if (incomemodel.CaseWorkerNote.NoteDate == null)
                        {
                            ModelState.AddModelError("", "Please enter not date");
                            isnoteerror = true;
                        }
                            
                        if ((incomemodel.CaseWorkerNote.TimeSpentHours == null || incomemodel.CaseWorkerNote.TimeSpentHours == 0) &&
                            (incomemodel.CaseWorkerNote.TimeSpentMinutes == null || incomemodel.CaseWorkerNote.TimeSpentMinutes == 0))
                        {
                            ModelState.AddModelError("", "Please enter time spent");
                            isnoteerror = true;
                        }
                            
                        if (incomemodel.CaseWorkerNote.ContactMethodID == null || incomemodel.CaseWorkerNote.ContactMethodID == 0)
                        {
                            ModelState.AddModelError("", "Please select contact method");
                            isnoteerror = true;
                        }
                        if (isnoteerror)
                            return View(incomemodel);
                    }

                    if (incomemodel.ID == 0)
                    {
                        incomemodel.IsInitialIncome = true;
                    }
                    else
                    {
                        incomemodel.ID = 0;
                        incomemodel.IsInitialIncome = false;
                    }

                    caseHouseholdIncomeRepository.InsertOrUpdate(incomemodel);
                    caseHouseholdIncomeRepository.Save();

                    if (incomemodel.CaseWorkerNote.ContactMethodID > 0)
                    {
                        incomemodel.CaseWorkerNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        incomemodel.CaseWorkerNote.CaseID = incomemodel.CaseID;
                        //incomemodel.CaseWorkerNote.CaseStatusID = incomemodel.CaseStatusID;
                        //incomemodel.CaseWorkerNote.ProgramID = incomemodel.ProgramID;
                        //varCase.CaseWorkerNote.NoteDate = Convert.ToDateTime(varCase.ContactDate);
                        incomemodel.CaseWorkerNote.IsFamily = true;
                        incomemodel.CaseWorkerNote.IsFamilyMember = false;
                        incomemodel.CaseWorkerNote.WorkerNoteActivityTypeID = (int)WorkerNoteActivityType.EditHouseholdIncome;

                        caseWorkerNoteRepository.InsertOrUpdate(incomemodel.CaseWorkerNote);
                        caseWorkerNoteRepository.Save();
                    }
                    return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseSummary, new { caseID = incomemodel.CaseID });

                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            incomemodel.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (incomemodel.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                incomemodel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                incomemodel.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return View(incomemodel);


        }
    }
}