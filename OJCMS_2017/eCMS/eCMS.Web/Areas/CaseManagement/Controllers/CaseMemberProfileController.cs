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
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseMemberProfileController : CaseBaseController
    {
       public CaseMemberProfileController(IWorkerRepository workerRepository, 
            ICaseMemberRepository casememberRepository, 
            IProfileTypeRepository profiletypeRepository, 
            IHighestLevelOfEducationRepository highestlevelofeducationRepository, 
            IGPARepository gpaRepository, 
            IAnnualIncomeRepository annualincomeRepository, 
            ISavingsRepository savingsRepository, 
            IHousingQualityRepository housingqualityRepository, 
            IImmigrationCitizenshipStatusRepository immigrationcitizenshipstatusRepository,
            ICaseMemberProfileRepository casememberprofileRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
           IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseWorkerRepository caseworkerRepository,
            ICaseRepository caseRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.casememberRepository = casememberRepository;
            this.profiletypeRepository = profiletypeRepository;
            this.highestlevelofeducationRepository = highestlevelofeducationRepository;
            this.gpaRepository = gpaRepository;
            this.annualincomeRepository = annualincomeRepository;
            this.savingsRepository = savingsRepository;
            this.housingqualityRepository = housingqualityRepository;
            this.immigrationcitizenshipstatusRepository = immigrationcitizenshipstatusRepository;
            this.casememberprofileRepository = casememberprofileRepository;
            this.caseworkerRepository = caseworkerRepository;
        }

        /// <summary>
        /// This action returns the list of CaseMemberProfile
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseMemberProfile">search filter</param>
        /// <returns>view result</returns>
       [WorkerAuthorize]
       public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, CaseMemberProfile searchCaseMemberProfile, int caseId, int? caseMemberID)
       {
           var varCase = caseRepository.Find(caseId);
           bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Index, true);
           if (!hasAccess)
           {
               WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
               //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
           }
           searchCaseMemberProfile.HasPermissionToList = hasAccess;
           searchCaseMemberProfile.CaseID = caseId;
           if (caseMemberID.HasValue && caseMemberID.Value > 0)
           {
               searchCaseMemberProfile.CaseMemberID = caseMemberID.Value;
               CaseMember caseMember = casememberRepository.Find(searchCaseMemberProfile.CaseMemberID);
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
           searchCaseMemberProfile.HasPermissionToCreate = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Create, true);
           return View(searchCaseMemberProfile);
       }

        /// <summary>
        /// This action creates new casememberprofile
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int caseId, int? caseMemberID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Create, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of casememberprofile
            CaseMemberProfile casememberprofile = new CaseMemberProfile();
            //return view result
            casememberprofile.CaseID = caseId;
            casememberprofile.ProfileDate = DateTime.Now;
            casememberprofile.ProfileTypeID = 1;
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                casememberprofile.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(casememberprofile.CaseMemberID);
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
            return View(casememberprofile);
        }

        /// <summary>
        /// This action saves new casememberprofile to database
        /// </summary>
        /// <param name="casememberprofile">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseMemberProfile casememberprofile, int caseId, int? caseMemberID)
        {
            casememberprofile.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //if (casememberprofile.ProfileDate > DateTime.Today)
                //{
                //    throw new CustomException("Profile date can't be future date.");
                //}
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    casememberprofileRepository.InsertOrUpdate(casememberprofile);
                    casememberprofileRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { caseId = casememberprofile.CaseID, caseMemberID = casememberprofile.CaseMemberID });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            casememberprofile.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (casememberprofile.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                casememberprofile.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                casememberprofile.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if operation is failed
            if (casememberprofile.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(casememberprofile.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(casememberprofile.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            return View(casememberprofile);
        }

        /// <summary>
        /// This action edits an existing casememberprofile
        /// </summary>
        /// <param name="id">casememberprofile id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id, int caseId, int? caseMemberID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //find the existing casememberprofile from database
            CaseMemberProfile casememberprofile = casememberprofileRepository.Find(id);
            casememberprofile.CaseID = caseId;
            if (casememberprofile.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(casememberprofile.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(casememberprofile.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(casememberprofile);
        }

        /// <summary>
        /// This action saves an existing casememberprofile to database
        /// </summary>
        /// <param name="casememberprofile">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseMemberProfile casememberprofile, int caseId, int? caseMemberID)
        {
            casememberprofile.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //if (casememberprofile.ProfileDate > DateTime.Today)
                //{
                //    throw new CustomException("Profile date can't be future date.");
                //}
                //validate data
                if (ModelState.IsValid)
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>

                    //var casememberprofile1 = casememberprofileRepository.Find(casememberprofile.ID);
                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(casememberprofile1.CaseMember.CaseID);
                    //List<CaseWorker> caseworker = caseworkerRepository.FindAllByCaseID(caseId).Where(x => x.WorkerID == CurrentLoggedInWorker.ID).ToList();
                    //if ((caseworker == null || caseworker.Count() == 0) && casememberprofile.ID > 0 && casememberprofile.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    //return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>

                    //call the repository function to save in database
                    casememberprofileRepository.InsertOrUpdate(casememberprofile);
                    casememberprofileRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { caseId = casememberprofile.CaseID, caseMemberID = casememberprofile.CaseMemberID });
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            casememberprofile.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (casememberprofile.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                casememberprofile.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                casememberprofile.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            if (casememberprofile.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(casememberprofile.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(casememberprofile.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return view with error message if the operation is failed
            return View(casememberprofile);
        }

        [WorkerAuthorize]
        public ActionResult Read(int id, int caseId, int? caseMemberID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Read, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the existing casememberprofile from database
            CaseMemberProfile casememberprofile = casememberprofileRepository.Find(id);
            casememberprofile.CaseID = caseId;
            if (casememberprofile.CaseMemberID > 0)
            {
                CaseMember caseMember = casememberRepository.Find(casememberprofile.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(casememberprofile.CaseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(casememberprofile);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId,int? caseMemberId)
        {
            return Json(casememberprofileRepository.Search(dsRequest,caseId,CurrentLoggedInWorker.ID,caseMemberId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// delete casememberprofile from database usign ajax operation
        /// </summary>
        /// <param name="id">casememberprofile id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the casememberprofile in database
            CaseMemberProfile casememberprofile = casememberprofileRepository.Find(id);
            if (casememberprofile == null)
            {
                //set error message if it does not exist in database
                casememberprofile = new CaseMemberProfile();
                casememberprofile.ErrorMessage = "CaseMemberProfile not found";
            }
            else
            {
                try
                {
                    bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Delete, true);
                    if (!hasAccess)
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    }

                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(casememberprofile.CaseMember.CaseID);
                    //if (casememberprofile.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //delete casememberprofile from database
                    casememberprofileRepository.Delete(casememberprofile);
                    casememberprofileRepository.Save();
                    //set success message
                    casememberprofile.SuccessMessage = "Case Member Profile has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    casememberprofile.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        casememberprofile.SuccessMessage = "Case Member Profile has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        casememberprofile.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (casememberprofile.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casememberprofile) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casememberprofile) });
            }
        }

    }
}

