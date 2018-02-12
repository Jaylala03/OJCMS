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
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Kendo.Mvc.Extensions;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseGoalController : CaseBaseController
    {
        private readonly ICaseGoalRepository caseGoalRepository;
        private readonly ICaseGoalLivingConditionRepository casegoallivingconditionRepository;
        public CaseGoalController(IWorkerRepository workerRepository, 
            ICaseMemberRepository casememberRepository,
            ICaseGoalRepository caseGoalRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            ICaseRepository caseRepository,
            IQualityOfLifeCategoryRepository qualityoflifecategoryRepository,
            ICaseWorkerRepository caseworkerRepository,
            ICaseGoalLivingConditionRepository casegoallivingconditionRepository
            , IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.casememberRepository = casememberRepository;
            this.caseGoalRepository = caseGoalRepository;
            this.qualityoflifecategoryRepository = qualityoflifecategoryRepository;
            this.casegoallivingconditionRepository = casegoallivingconditionRepository;
            this.caseworkerRepository = caseworkerRepository;
        }

        /// <summary>
        /// This action returns the list of CaseGoal
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseGoal">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index(int caseId, int? caseMemberId)
        {
            var varCase = caseRepository.Find(caseId);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseGoal caseGoal = new CaseGoal();
            caseGoal.CaseID = caseId;
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseGoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseGoal.CaseMemberID);
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
            caseGoal.HasPermissionToCreate = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Create, true);
            return View(caseGoal);
        }

        /// <summary>
        /// This action creates new caseGoal
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int caseId, int? caseMemberId)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Create, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of caseGoal
            CaseGoal caseGoal = new CaseGoal();
            caseGoal.CaseID = caseId;
            caseGoal.StartDate = DateTime.Now;
            caseGoal.EndDate = caseGoal.StartDate.AddMonths(1);
            ViewBag.QualityOfLifeCategory = qualityoflifecategoryRepository.All.Where(item=>item.IsActive==true).AsEnumerable().Select(item => new CaseGoalLivingCondition() { QualityOfLifeCategoryID=item.ID, QualityOfLifeCategoryName=item.Name });
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseGoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseGoal.CaseMemberID);
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
            //return view result
            return View(caseGoal);
        }

        /// <summary>
        /// This action saves new caseGoal to database
        /// </summary>
        /// <param name="caseGoal">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseGoal caseGoal, int caseId, int? caseMemberId)
        {
            caseGoal.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                if (caseGoal.StartDate > caseGoal.EndDate)
                {
                    throw new CustomException("Start date can't be greater than end date.");
                }
                //validate data
                if (ModelState.IsValid)
                {
                   
                    caseGoal.QualityOfLifeCategoryIDs = Request.Form["QualityOfLifeCategoryIDs"].ToString(true);
                    if (caseGoal.QualityOfLifeCategoryIDs.IsNullOrEmpty())
                    {
                        throw new CustomException("You must select at least one Q.O.L. category");
                    }
                    //call the repository function to save in database
                    caseGoalRepository.InsertOrUpdate(caseGoal,Request.Form);
                    caseGoalRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseId, caseMemberId = caseMemberId });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseGoal.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseGoal.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                    //string selectedQOL = caseGoal.QualityOfLifeCategoryIDs;
                    //selectedQOL = selectedQOL.Replace("false", string.Empty);
                    //string[] arraySelectedQOL = selectedQOL.ToStringArray(',', true);
                    //if (arraySelectedQOL != null && arraySelectedQOL.Length > 0)
                    //{
                        
                    //    foreach (string qolID in arraySelectedQOL)
                    //    {
                    //        CaseGoalLivingCondition caseGoalLivingCondition = new CaseGoalLivingCondition();
                    //        caseGoalLivingCondition.QualityOfLifeCategoryID = qolID;

                              
                    //    }
                    //}
                }
            }
            catch (CustomException ex)
            {
                caseGoal.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseGoal.ErrorMessage = Constants.Messages.UnhandelledError;
            }

            List<CaseGoalLivingCondition> qolList = qualityoflifecategoryRepository.All.Where(item => item.IsActive == true).AsEnumerable().Select(item => new CaseGoalLivingCondition() { QualityOfLifeCategoryID = item.ID, QualityOfLifeCategoryName = item.Name }).ToList();
            List<Int32> existingQOLList = new List<int>();
            if (Request.Form["QualityOfLifeCategoryIDs"] != null)
            {
                existingQOLList = Request.Form["QualityOfLifeCategoryIDs"].ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            foreach (CaseGoalLivingCondition qol in qolList)
            {
                if (existingQOLList != null)
                {
                    foreach (int existingQOL in existingQOLList)
                    {
                        if (qol.QualityOfLifeCategoryID == existingQOL)
                        {
                            qol.ID = existingQOL;
                            qol.Note = Request.Form["txtQualityOfLifeCategoryIDs_" + existingQOL];
                            break;
                        }
                    }
                }
            }
            ViewBag.QualityOfLifeCategory = qolList;

            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseGoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseGoal.CaseMemberID);
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
            //return view with error message if operation is failed
            return View(caseGoal);
        }

        /// <summary>
        /// This action edits an existing caseGoal
        /// </summary>
        /// <param name="id">caseGoal id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id, int caseId, int? caseMemberId)
        {
            var varCase = caseRepository.Find(caseId);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the existing caseGoal from database
            CaseGoal caseGoal = caseGoalRepository.Find(id);
            List<CaseGoalLivingCondition> qolList = qualityoflifecategoryRepository.All.Where(item => item.IsActive == true).AsEnumerable().Select(item => new CaseGoalLivingCondition() { QualityOfLifeCategoryID = item.ID, QualityOfLifeCategoryName = item.Name }).ToList();
            List<CaseGoalLivingCondition> existingQOLList = casegoallivingconditionRepository.AllIncluding(id).ToList();
            foreach (CaseGoalLivingCondition qol in qolList)
            {
                if (existingQOLList != null)
                {
                    foreach (CaseGoalLivingCondition existingQOL in existingQOLList)
                    {
                        if (qol.QualityOfLifeCategoryID == existingQOL.QualityOfLifeCategoryID)
                        {
                            qol.ID = existingQOL.ID;
                            qol.Note = existingQOL.Note;
                            break;
                        }
                    }
                }
            }
            ViewBag.QualityOfLifeCategory = qolList;
            caseGoal.CaseID = caseId;
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseGoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseGoal.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                //Case varCase = caseRepository.Find(caseId);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(caseGoal);
        }

        [WorkerAuthorize]
        public ActionResult Read(int id, int caseId, int? caseMemberId)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Read, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //find the existing caseGoal from database
            CaseGoal caseGoal = caseGoalRepository.Find(id);
            List<CaseGoalLivingCondition> qolList = qualityoflifecategoryRepository.All.Where(item => item.IsActive == true).AsEnumerable().Select(item => new CaseGoalLivingCondition() { QualityOfLifeCategoryID = item.ID, QualityOfLifeCategoryName = item.Name }).ToList();
            List<CaseGoalLivingCondition> existingQOLList = casegoallivingconditionRepository.AllIncluding(id).ToList();
            foreach (CaseGoalLivingCondition qol in qolList)
            {
                if (existingQOLList != null)
                {
                    foreach (CaseGoalLivingCondition existingQOL in existingQOLList)
                    {
                        if (qol.QualityOfLifeCategoryID == existingQOL.QualityOfLifeCategoryID)
                        {
                            qol.ID = existingQOL.ID;
                            qol.Note = existingQOL.Note;
                            break;
                        }
                    }
                }
            }
            ViewBag.QualityOfLifeCategory = qolList;
            caseGoal.CaseID = caseId;
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseGoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseGoal.CaseMemberID);
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
            //return editor view
            return View(caseGoal);
        }

        /// <summary>
        /// This action saves an existing caseGoal to database
        /// </summary>
        /// <param name="caseGoal">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseGoal caseGoal, int caseId, int? caseMemberId)
        {
            caseGoal.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                if (caseGoal.StartDate > caseGoal.EndDate)
                {
                    throw new CustomException("Start date can't be greater than end date.");
                }
                //validate data
                if (ModelState.IsValid)
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseGoal.CaseID);
                    //List<CaseWorker> caseworker = caseworkerRepository.FindAllByCaseID(caseId).Where(x => x.WorkerID == CurrentLoggedInWorker.ID).ToList();
                    //if ((caseworker == null || caseworker.Count() == 0) && caseGoal.ID > 0 && caseGoal.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>

                    caseGoal.QualityOfLifeCategoryIDs = Request.Form["QualityOfLifeCategoryIDs"].ToString(true);
                    //call the repository function to save in database
                    caseGoalRepository.InsertOrUpdate(caseGoal, Request.Form);
                    caseGoalRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseId, caseMemberId = caseMemberId });
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseGoal.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseGoal.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                caseGoal.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseGoal.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            List<CaseGoalLivingCondition> qolList = qualityoflifecategoryRepository.All.Where(item => item.IsActive == true).AsEnumerable().Select(item => new CaseGoalLivingCondition() { QualityOfLifeCategoryID = item.ID, QualityOfLifeCategoryName = item.Name }).ToList();
            List<Int32> existingQOLList = new List<int>();
            if (Request.Form["QualityOfLifeCategoryIDs"] != null)
            {
                existingQOLList = Request.Form["QualityOfLifeCategoryIDs"].ToString(true).ToStringArray(',', true).Select(Int32.Parse).ToList();
            }
            foreach (CaseGoalLivingCondition qol in qolList)
            {
                if (existingQOLList != null)
                {
                    foreach (int existingQOL in existingQOLList)
                    {
                        if (qol.QualityOfLifeCategoryID == existingQOL)
                        {
                            qol.ID = existingQOL;
                            qol.Note = Request.Form["txtQualityOfLifeCategoryIDs_" + existingQOL];
                            break;
                        }
                    }
                }
            }
            ViewBag.QualityOfLifeCategory = qolList;
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseGoal.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseGoal.CaseMemberID);
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
            //return view with error message if the operation is failed
            return View(caseGoal);
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
            DataSourceResult result = caseGoalRepository.Search(dsRequest, caseId, CurrentLoggedInWorker.ID, caseMemberId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// delete caseGoal from database usign ajax operation
        /// </summary>
        /// <param name="id">caseGoal id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            BaseModel baseModel = new BaseModel();
            //find the caseGoal in database
            try
            {
                bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseGoal, Constants.Actions.Delete, true);
                if (!hasAccess)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                }

                 CaseGoal casegoal = caseGoalRepository.Find(id);
                 //var primaryWorkerID = GetPrimaryWorkerOfTheCase(casegoal.CaseMember.CaseID);
                 //if (casegoal.ID > 0 && casegoal.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                 //{
                 //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                 //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                 //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                 //}
                //delete caseGoal from database
                caseGoalRepository.Delete(casegoal);
                caseGoalRepository.Save();
                //set success message
                baseModel.SuccessMessage = "Case Member Goal has been deleted successfully";
            }
            catch (CustomException ex)
            {
                baseModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                {
                    baseModel.SuccessMessage = "Case Member Goal has been deleted successfully";
                }
                else
                {
                    ExceptionManager.Manage(ex);
                    baseModel.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (baseModel.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, baseModel) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, baseModel) });
            }
        }

    }
}

