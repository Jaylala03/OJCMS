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
using System.Web.Mvc;
using System.Linq;
using eCMS.DataLogic.ViewModels;
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseWorkerController : CaseBaseController
    {
        private readonly ICaseWorkerMemberAssignmentRepository caseworkermemberassignmentRepository;
        public CaseWorkerController(IWorkerRepository workerRepository, 
            ICaseRepository caseRepository, 
            ICaseWorkerRepository caseworkerRepository, 
            IWorkerRoleRepository workerroleRepository,
            ICaseMemberRepository casememberRepository,
            ICaseWorkerMemberAssignmentRepository caseworkermemberassignmentRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.workerroleRepository = workerroleRepository;
            this.casememberRepository = casememberRepository;
            this.caseworkermemberassignmentRepository = caseworkermemberassignmentRepository;
        }

        /// <summary>
        /// This action returns the list of CaseWorker
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseWorker">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index(int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID,CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID,varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            AssignmentModel assignmentModel = new AssignmentModel();
            assignmentModel.CaseWorker = new CaseWorker();
            assignmentModel.CaseSupportCircle = new CaseSupportCircle();
            assignmentModel.CaseSupportCircle.CaseID = caseID;
            assignmentModel.CaseWorker.CaseID = caseID;
            assignmentModel.CaseWorker.CaseMemberList = casememberRepository.FindAllByCaseIDForDropDownList(caseID);
            //List<SelectListItem> workerList = workerRepository.FindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
            List<SelectListItem> workerList = workerRepository.NewFindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
            if (workerList == null || (workerList != null && workerList.Count == 0))
            {
                assignmentModel.ErrorMessage = "There is no worker found for Program:" + varCase.Program.Name + ", Region:" + varCase.Region.Name + ", Sub-Program:" + varCase.SubProgram.Name;
            }
            else
            {
                workerList = workerList.OrderBy(m => m.Text).ToList();
            }
            ViewBag.PossibleWorkerList = workerList;
            ViewBag.DisplayID = caseRepository.Find(caseID).DisplayID;
            return View(assignmentModel);
        }

        [WorkerAuthorize]
        public ActionResult IndexRead(int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            AssignmentModel assignmentModel = new AssignmentModel();
            assignmentModel.CaseWorker = new CaseWorker();
            assignmentModel.CaseSupportCircle = new CaseSupportCircle();
            assignmentModel.CaseSupportCircle.CaseID = caseID;
            assignmentModel.CaseWorker.CaseID = caseID;
            assignmentModel.CaseWorker.CaseMemberList = casememberRepository.FindAllByCaseIDForDropDownList(caseID);
            //List<SelectListItem> workerList = workerRepository.FindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
            List<SelectListItem> workerList = workerRepository.NewFindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
            if (workerList == null || (workerList != null && workerList.Count == 0))
            {
                assignmentModel.ErrorMessage = "There is no worker found for Program:" + varCase.Program.Name + ", Region:" + varCase.Region.Name + ", Sub-Program:" + varCase.SubProgram.Name;
            }
            else
            {
                workerList = workerList.OrderBy(m => m.Text).ToList();
            }
            ViewBag.PossibleWorkerList = workerList;
            ViewBag.DisplayID = caseRepository.Find(caseID).DisplayID;
            return View(assignmentModel);
        }
        public JsonResult LoadWorkerAjax(int caseID,int? RoleID=0)
        {
            var varCase = caseRepository.Find(caseID);
            if (RoleID > 0)
            {
                //List<SelectListItem> workerList = workerRepository.FindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID,RoleID);
                List<SelectListItem> workerList = workerRepository.NewFindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, RoleID);
                return Json(workerList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //List<SelectListItem> workerList = workerRepository.FindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
                List<SelectListItem> workerList = workerRepository.NewFindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
                return Json(workerList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadWorkerRoleByRegionProgram(int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
            {
                return Json(workerroleRepository.AllActiveForDropDownList.Where(item => item.Value != "1"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(workerroleRepository.GetWorkerRoleByProgramAndRegionID(varCase.ProgramID, varCase.RegionID,varCase.SubProgramID,varCase.JamatkhanaID), JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// This action creates new caseworker
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int caseId)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Create, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseWorker caseworker = new CaseWorker();
            caseworker.CaseID = caseId;
            Case varCase=caseRepository.Find(caseId);
            //List<SelectListItem> workerList = workerRepository.FindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
            List<SelectListItem> workerList = workerRepository.NewFindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
            if (workerList == null || (workerList != null && workerList.Count == 0))
            {
                caseworker.ErrorMessage = "There is no worker found for Program:" + varCase.Program.Name + ", Region:" + varCase.Region.Name + ", Sub-Program:" + varCase.SubProgram.Name;
            }
            ViewBag.PossibleWorkerList = workerList;
            return View(caseworker);
        }

        /// <summary>
        /// This action saves new caseworker to database
        /// </summary>
        /// <param name="caseworker">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseWorker caseworker, int caseId)
        {
            caseworker.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    caseworkerRepository.InsertOrUpdate(caseworker);
                    caseworkerRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseId });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseworker.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseworker.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                caseworker.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseworker.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleWorkers = workerRepository.All;
            return View(caseworker);
        }

       

        /// <summary>
        /// This action edits an existing caseworker
        /// </summary>
        /// <param name="id">caseworker id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id)
        {
            //find the existing caseworker from database
            CaseWorker caseworker = caseworkerRepository.Find(id);
            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleWorkers = workerRepository.All;
            //return editor view
            return View(caseworker);
        }

        /// <summary>
        /// This action saves an existing caseworker to database
        /// </summary>
        /// <param name="caseworker">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseWorker caseworker, int caseId)
        {
            caseworker.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseworker.CaseID);
                    if (caseworker.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        //return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                        return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    }
                    caseworkerRepository.InsertOrUpdate(caseworker);
                    caseworkerRepository.Save();
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseId });
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseworker.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseworker.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    } 
                }    
            }
            catch (CustomException ex)
            {
                caseworker.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseworker.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleWorkers = workerRepository.All;
            //return view with error message if the operation is failed
            return View(caseworker);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            CaseWorker caseWrker = new CaseWorker();
            caseWrker.CaseID = caseId;
            //List<CaseWorker> caseWorkerList = caseworkerRepository.AllIncluding().Where(item => item.CaseID == caseId).OrderBy(item => item.LastUpdateDate).ToList().AsEnumerable().Select(caseworker => new CaseWorker()
            //{
            //    ID=caseworker.ID,
            //    WorkerName = caseworker.Worker.FirstName + " " + caseworker.Worker.LastName,
            //    CaseID=caseworker.CaseID,
            //    WorkerID=caseworker.WorkerID,
            //    IsActive=caseworker.IsActive,
            //    AllowNotification=caseworker.AllowNotification,
            //    IsPrimary=caseworker.IsPrimary,
                
                
            //}).ToList();
           
            //if (caseWorkerList != null)
            //{
            //    foreach (CaseWorker caseWorker in caseWorkerList)
            //    {
            //        List<CaseWorkerMemberAssignment> memberAssignment = caseworkermemberassignmentRepository.FindAllByCaseWorkerID(caseWorker.ID);
            //        if (memberAssignment != null)
            //        {
            //            foreach (CaseWorkerMemberAssignment member in memberAssignment)
            //            {
            //                caseWorker.AssignedMembers = caseWorker.AssignedMembers.Concate(',', member.CaseMember.FirstName + " " + member.CaseMember.LastName);
            //            }
            //        }
            //    }
            //}
            DataSourceResult result = caseworkerRepository.Search(caseWrker, dsRequest);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action displays the details of an existing caseworker on popup
        /// </summary>
        /// <param name="id">caseworker id</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult DetailsAjax(int id)
        {
            CaseWorker caseworker = caseworkerRepository.Find(id);
            if (caseworker == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Worker not found");
            }
            return Content(this.RenderPartialViewToString(Constants.PartialViews.Details, caseworker));
        }

        /// <summary>
        /// This action opens caseworker editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">caseworker id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, int caseId)
        {
            CaseWorker caseworker = null;
             Case varCase = caseRepository.Find(caseId);

            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseId, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            if (id > 0)
            {
                //find an existing caseworker from database
                caseworker = caseworkerRepository.Find(id);
                if (caseworker == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Worker not found");
                }
                caseworker.CaseMemberList = casememberRepository.FindAllByCaseIDForDropDownList(caseId);
                List<CaseWorkerMemberAssignment> assignment = caseworkermemberassignmentRepository.FindAllByCaseWorkerID(caseworker.ID);
                if (assignment != null)
                {
                    foreach (SelectListItem caseMember in caseworker.CaseMemberList)
                    {
                        if (assignment.Where(item => item.CaseMemberID == caseMember.Value.ToInteger(true)).Count() > 0)
                        {
                            caseMember.Selected = true;
                        }
                    }
                }
            }
            else
            {
                //create a new instance if id is not provided
                caseworker = new CaseWorker();
                caseworker.CaseID = caseId.ToInteger(true);
            }
            
            //List<SelectListItem> workerList = workerRepository.FindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
            List<SelectListItem> workerList = workerRepository.NewFindAllPossible(varCase.ProgramID, varCase.RegionID, varCase.SubProgramID);
            if (workerList == null || (workerList != null && workerList.Count == 0))
            {
                caseworker.ErrorMessage = "There is no worker found for Program:" + varCase.Program.Name + ", Region:" + varCase.Region.Name + ", Sub-Program:" + varCase.SubProgram.Name;
            }
            ViewBag.PossibleWorkerList = workerList;
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, caseworker));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="caseworker">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseWorker caseworker)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = caseworker.ID == 0;
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (!isNew)
                    //{
                    //    var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseworker.CaseID);
                    //    List<CaseWorker> lstcaseworker = caseworkerRepository.FindAllByCaseID(caseworker.CaseID).Where(x => x.WorkerID == CurrentLoggedInWorker.ID).ToList();
                    //    if ((lstcaseworker == null || lstcaseworker.Count() == 0) && caseworker.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //    {
                    //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //    }
                    //}
                    //</JL:Comment:07/08/2017>

                    //set the id of the worker who has added/updated this record
                    caseworker.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    //call repository function to save the data in database
                    caseworkerRepository.InsertOrUpdate(caseworker);
                    caseworkerRepository.Save();
                    string selectedCaseMember = Request.Form["SelectedCaseMember"].ToString(true);
                    selectedCaseMember = selectedCaseMember.Replace("false", string.Empty);
                    string[] arraySelectedCaseMember = selectedCaseMember.ToStringArray(',', true);
                    List<CaseWorkerMemberAssignment> assignment = caseworkermemberassignmentRepository.FindAllByCaseWorkerID(caseworker.ID);
                    if (arraySelectedCaseMember != null && arraySelectedCaseMember.Length > 0)
                    {
                        foreach (string caseMemberID in arraySelectedCaseMember)
                        {
                            if (assignment.Where(item => item.CaseMemberID == caseMemberID.ToInteger(true)).Count() == 0)
                            {
                                CaseWorkerMemberAssignment newCaseWorkerMemberAssignment = new CaseWorkerMemberAssignment()
                                {
                                    CaseMemberID = caseMemberID.ToInteger(true),
                                    CaseWorkerID = caseworker.ID,
                                    LastUpdateDate = DateTime.Now,
                                    LastUpdatedByWorkerID = caseworker.LastUpdatedByWorkerID
                                };
                                caseworkermemberassignmentRepository.InsertOrUpdate(newCaseWorkerMemberAssignment);
                                caseworkermemberassignmentRepository.Save();
                            }
                        }
                    }

                    foreach (CaseWorkerMemberAssignment existingMember in assignment)
                    {
                        if (arraySelectedCaseMember==null || !arraySelectedCaseMember.Contains(existingMember.CaseMemberID.ToString(true)))
                        {
                            caseworkermemberassignmentRepository.Delete(existingMember);
                            caseworkermemberassignmentRepository.Save();
                        }
                    }
                    //set status message
                    if (isNew)
                    {
                        caseworker.SuccessMessage = "Case Worker has been added successfully";
                    }
                    else
                    {
                        caseworker.SuccessMessage = "Case Worker has been updated successfully";
                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    caseworker.ErrorMessage = "The worker is already assigned to the family or family member";
                }
                catch (CustomException ex)
                {
                    caseworker.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    caseworker.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        caseworker.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (caseworker.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (caseworker.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseworker) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseworker) });
            }
        }

        /// <summary>
        /// delete caseworker from database usign ajax operation
        /// </summary>
        /// <param name="id">caseworker id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Delete, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the varCase in database
            BaseModel statusModel = new BaseModel();
            //var caseworker = caseworkerRepository.Find(id);
            //var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseworker.CaseID);
            //if (caseworker.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
            //{
            //    statusModel.ErrorMessage = "You are not eligible to do this action";
            //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
            //}
            try
            {
                //delete varCase from database
                caseworkerRepository.Delete(id);
                caseworkerRepository.Save();
                //set success message
                statusModel.SuccessMessage = "Case worker has been deleted successfully";
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                {
                    statusModel.SuccessMessage = "Case worker has been deleted successfully";
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

    }
}

