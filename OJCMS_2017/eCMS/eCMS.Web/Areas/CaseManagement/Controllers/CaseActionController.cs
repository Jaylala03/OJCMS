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
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseActionController : CaseBaseController
    {
        private readonly ICaseProgressNoteRepository caseprogressnoteRepository;
        private readonly ICaseActionRepository caseactionRepository;
        private readonly ICaseSmartGoalRepository casesmartgoalRepository;
        private readonly ICaseAssessmentRepository caseassessmentRepository;
        private readonly ICaseGoalRepository caseGoalRepository;
        private readonly IWorkerToDoRepository workertodoRepository;
        private readonly IEmailTemplateRepository emailTemplateRepository;
        public CaseActionController(IWorkerRepository workerRepository,
            ICaseProgressNoteRepository caseprogressnoteRepository, 
            ICaseWorkerRepository caseworkerRepository,
            ICaseActionRepository caseactionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseRepository caseRepository,
            ICaseSmartGoalRepository casesmartgoalRepository,
            ICaseMemberRepository casememberRepository,
            ICaseMemberProfileRepository casememberprofileRepository,
            ICaseAssessmentRepository caseassessmentRepository,
            ICaseGoalRepository caseGoalRepository,
            IWorkerToDoRepository workertodoRepository,
            IEmailTemplateRepository emailTemplateRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.caseprogressnoteRepository = caseprogressnoteRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.caseactionRepository = caseactionRepository;
            this.casesmartgoalRepository = casesmartgoalRepository;
            this.casememberRepository = casememberRepository;
            this.casememberprofileRepository = casememberprofileRepository;
            this.caseassessmentRepository = caseassessmentRepository;
            this.caseGoalRepository = caseGoalRepository;
            this.workertodoRepository = workertodoRepository;
            this.emailTemplateRepository = emailTemplateRepository;
        }

        /// <summary>
        /// This action returns the list of CaseAction
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseAction">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index(int caseID, int? caseMemberID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseAction, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseAction caseAction = new CaseAction();
            caseAction.CaseID = caseID;
            if (caseMemberID.HasValue && caseMemberID > 0)
            {
                caseAction.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseAction.CaseMemberID.Value);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                    int muridProfileCount = casememberprofileRepository.All.Count(item => item.CaseMember.CaseID == caseID && item.CaseMemberID == caseMemberID.Value);
                    if (muridProfileCount == 0)
                    {
                        caseAction.ErrorMessage = "There is no profile created for family or family member " + caseMember.FirstName + " " + caseMember.LastName;
                    }
                    else
                    {
                        int assessmentCount = caseassessmentRepository.All.Count(item => item.CaseMember.CaseID == caseID && item.CaseMemberID == caseMemberID.Value);
                        if (assessmentCount == 0)
                        {
                            caseAction.ErrorMessage = "There is no assessment created for family or family member " + caseMember.FirstName + " " + caseMember.LastName;
                        }
                        else
                        {
                            int goalCount = caseGoalRepository.All.Count(item => item.CaseMember.CaseID == caseID && item.CaseMemberID == caseMemberID.Value);
                            if (goalCount == 0)
                            {
                                caseAction.ErrorMessage = "There is no goal identified for family or family member " + caseMember.FirstName + " " + caseMember.LastName;
                            }
                        }
                    }
                }
            }
            else
            {
                caseAction.CaseMemberID = 0;
                int muridProfileCount = casememberprofileRepository.All.Count(item => item.CaseMember.CaseID == caseID);
                if (muridProfileCount == 0)
                {
                    caseAction.ErrorMessage = "There is no profile created for this case";
                }
                else
                {
                    int assessmentCount = caseassessmentRepository.All.Count(item => item.CaseMember.CaseID == caseID);
                    if (assessmentCount == 0)
                    {
                        caseAction.ErrorMessage = "There is no assessment created for this case";
                    }
                    else
                    {
                        int goalCount = caseGoalRepository.All.Count(item => item.CaseMember.CaseID == caseID);
                        if (goalCount == 0)
                        {
                            caseAction.ErrorMessage = "There is no goal identified for this case";
                        }
                    }
                }
            }
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                caseAction.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseAction.CaseMemberID.Value);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                if (varCase == null)
                {
                    varCase = caseRepository.Find(caseID);
                }
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return view result
            return View(caseAction);
        }

        [WorkerAuthorize]
        public ViewResult SmartGoalAction(int casesmartgoalId, int caseId, int? caseMemberId)
        {
            CaseSmartGoal caseSmartGoal = casesmartgoalRepository.Find(casesmartgoalId);
            if (caseSmartGoal != null)
            {
                List<CaseSmartGoalAssignment> goalAssignmentList = casesmartgoalRepository.FindAllCaseSmartGoalAssignmentByCaseSmargGoalID(caseSmartGoal.ID);
                if (goalAssignmentList != null)
                {
                    foreach (CaseSmartGoalAssignment goalAssignment in goalAssignmentList)
                    {
                        caseSmartGoal.SmartGoalName = caseSmartGoal.SmartGoalName.Concate(",", goalAssignment.SmartGoal.Name);
                    }
                }
            }
            caseSmartGoal.CaseID = caseId;
            CaseAction caseaction = new CaseAction();
            caseaction.CaseSmartGoal = caseSmartGoal;
            caseaction.CaseSmartGoalID = casesmartgoalId;
            caseaction.CaseID = caseId;
            if (caseMemberId.HasValue && caseMemberId.Value > 0)
            {
                caseaction.CaseMemberID = caseMemberId.Value;
                CaseMember caseMember = casememberRepository.Find(caseaction.CaseMemberID.Value);
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
            return View(caseaction);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, bool isCompleted, int noteId, int casesmartgoalId = 0, int caseId = 0, int casememberId = 0, int casesmartgoalserviceproviderid = 0, bool includeServiceProviderAction = false, bool isProviderAction=false)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            int workerId = CurrentLoggedInWorker.ID;
            if (casesmartgoalId > 0 || noteId > 0)
            {
                workerId = 0;
            }
            DataSourceResult result = caseactionRepository.Search(dsRequest, caseId, workerId, casememberId, isCompleted, casesmartgoalId, noteId, casesmartgoalserviceproviderid, includeServiceProviderAction, isProviderAction);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        public ViewResult MyToDo()
        {
            return View();
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult MyToDoAjax([DataSourceRequest] DataSourceRequest dsRequest)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = new DataSourceResult();
            result = caseactionRepository.AllIncluding().
            Where(item => item.CaseWorker.WorkerID == CurrentLoggedInWorker.ID)
            .OrderByDescending(item => item.ActionEndTime).OrderByDescending(item => item.ActionStartTime)
            .Select(caseaction => new
            {
                CaseProgramName = caseaction.CaseMember != null ? caseaction.CaseMember.Case.Program.Name : caseaction.CaseSmartGoal != null ? caseaction.CaseSmartGoal.CaseGoal.CaseMember.Case.Program.Name : caseaction.CaseSmartGoalServiceProvider != null ? caseaction.CaseSmartGoalServiceProvider.CaseSmartGoal.CaseGoal.CaseMember.Case.Program.Name : string.Empty,
                CaseDisplayID = caseaction.CaseMember != null ? caseaction.CaseMember.Case.DisplayID : caseaction.CaseSmartGoal != null ? caseaction.CaseSmartGoal.CaseGoal.CaseMember.Case.DisplayID : caseaction.CaseSmartGoalServiceProvider != null ? caseaction.CaseSmartGoalServiceProvider.CaseSmartGoal.CaseGoal.CaseMember.Case.DisplayID : string.Empty,
                caseaction.ID,
                caseaction.CaseProgressNoteID,
                CaseWorkerName = caseaction.CaseWorker.Worker.FirstName + " " + caseaction.CaseWorker.Worker.LastName,
                caseaction.Action,
                caseaction.ActionStartTime,
                caseaction.ActionEndTime,
                caseaction.IsCompleted,
                CaseID = caseaction.CaseMember != null ? caseaction.CaseMember.CaseID : caseaction.CaseSmartGoal != null ? caseaction.CaseSmartGoal.CaseGoal.CaseMember.CaseID : caseaction.CaseSmartGoalServiceProvider != null ? caseaction.CaseSmartGoalServiceProvider.CaseSmartGoal.CaseGoal.CaseMember.CaseID : 0,
                CreatedByWorkerName = caseaction.CreatedByWorker.FirstName + " " + caseaction.CreatedByWorker.LastName
            }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult MyToDoNotificationAjax([DataSourceRequest] DataSourceRequest dsRequest)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = workertodoRepository.All.Where(item => item.WorkerID == CurrentLoggedInWorker.ID).Select(item => new { item.CreateDate, item.Subject }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens caseaction editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">caseaction id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, int caseId, int? caseprogressnoteId, int? casesmartgoalId)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAction, Constants.Actions.Edit, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseAction caseaction = null;
            if (id > 0)
            {
                //find an existing caseaction from database
                caseaction = caseactionRepository.Find(id);
                if (caseaction == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Progress Note Action not found");
                }
                else
                {
                    if (caseaction.CaseMemberID != null)
                    {
                        caseaction.CaseMemberIds = Convert.ToInt32(caseaction.CaseMemberID);
                    }
                    
                    if (caseaction.CaseMemberID != null)
                    {
                        caseaction.CaseMemberWorkerID = caseaction.CaseMemberID+"-M";
                    }
                    else if (caseaction.CaseWorkerID != null)
                    {
                        caseaction.CaseMemberWorkerID = caseaction.CaseWorkerID+"-W";
                    }
                    
                }
                //caseaction.CaseActionID = caseaction.ID;
            }
            else
            {
                //create a new instance if id is not provided
                caseaction = new CaseAction();
                caseaction.CaseProgressNoteID = caseprogressnoteId;
                caseaction.CaseSmartGoalID = casesmartgoalId;
                
               
            }
           
            caseaction.CaseID = caseId;
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, caseaction));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="caseaction">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseAction caseaction)
        {
            //id=0 means add operation, update operation otherwise
            //caseaction.ID = caseaction.CaseActionID;
            bool isNew = caseaction.ID == 0;
            if (ModelState.ContainsKey("CaseMemberID"))
                ModelState["CaseMemberID"].Errors.Clear();
            //validate data
            if (ModelState.IsValid)
            {
                try
                {
                    
                    //if (caseaction.IsCompleted == false || caseaction.IsCompleted==null)
                    //{
                    //    throw new CustomException("Please check Is Completed");
                    //}
                    if (caseaction.ActionStartTime > caseaction.ActionEndTime)
                    {
                        throw new CustomException("Start date can't be greater than end date.");
                    }

                    if (string.IsNullOrEmpty(caseaction.CaseMemberWorkerID))
                    {
                        throw new CustomException("Please select a worker/member");
                    }
                    else
                    {
                        var CaseWorker_Member = caseaction.CaseMemberWorkerID.Split('-');
                        if (CaseWorker_Member[1] == "M")
                        {
                            caseaction.CaseMemberID = Convert.ToInt32(CaseWorker_Member[0]);
                            caseaction.CaseWorkerID = null;
                        }
                        else
                        {
                            caseaction.CaseWorkerID = Convert.ToInt32(CaseWorker_Member[0]);
                            caseaction.CaseMemberID = null;
                        }
                    }

                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (caseaction.ID > 0)
                    //{
                    //    var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseaction.CaseID);
                    //    if (caseaction.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //    {
                    //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //    }
                    //}
                    //</JL:Comment:07/08/2017>

                    //set the id of the worker who has added/updated this record
                    caseaction.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                   
                        caseactionRepository.InsertOrUpdate(caseaction);
                        caseactionRepository.Save();
                   
                    //call repository function to save the data in database
                   
                    //set status message
                    if (isNew)
                    {
                        caseaction.SuccessMessage = "Action has been added successfully";
                    }
                    else
                    {
                        caseaction.SuccessMessage = "Action has been updated successfully";
                    }
                    try
                    {
                        var caseWorker = caseworkerRepository.Find(Convert.ToInt32(caseaction.CaseWorkerID));

                        var worker = workerRepository.Find(caseWorker.WorkerID);


                        string subject = ConfigurationManager.AppSettings["subject"].ToString();
                        string site = "";
                        String strPathAndQuery = Request.Url.PathAndQuery;
                        String currentHost = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                        string myHostTest = SiteConfigurationReader.GetAppSettingsString("Test");
                        string myHostLive = SiteConfigurationReader.GetAppSettingsString("Live");


                        if (currentHost == myHostLive)
                        {
                            site = myHostLive;
                        }
                        else
                        {

                            site = myHostTest;

                        }
                        string message = "Dear User,<br /><br />A new action has been assigned to you.<br /><br /><a href='{0}'>Click here to view the action</a><br /><br />Regards,<br /><br />eCMS System Manager";
                        if (caseaction.CaseProgressNoteID > 0)
                        {
                            message = message.Replace("{0}", site + "/CaseManagement/CaseProgressNote/Edit?noteID=" + caseaction.CaseProgressNoteID + "&CaseID=" + caseaction.CaseID + "&caseMemberId=" + caseaction.CaseMemberID + "");
                        }
                        else if (caseaction.CaseSmartGoalID > 0)
                        {
                            message = message.Replace("{0}", site + "/CaseManagement/CaseSmartGoal/Edit?casesmartgoalId=" + caseaction.CaseSmartGoalID + "&CaseID=" + caseaction.CaseID + "&caseMemberId=" + caseaction.CaseMemberID + "");
                        }
                        else
                        {
                            message = message.Replace("{0}", site + "/CaseManagement/CaseSmartGoalServiceProvider/Index?casesmartgoalId=0&casesmartgoalserviceproviderId=" + caseaction.CaseSmartGoalServiceProviderID + "&CaseID=" + caseaction.CaseID + "&caseMemberId=" + caseaction.CaseMemberID + "");
                        }
                        eCMS.BusinessLogic.Helpers.ExcEMail.SendEmail(worker.EmailAddress, subject, message, true);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (CustomException ex)
                {
                    caseaction.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    caseaction.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        caseaction.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (caseaction.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (caseaction.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseaction) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseaction) });
            }
        }

        /// <summary>
        /// delete caseaction from database usign ajax operation
        /// </summary>
        /// <param name="id">caseaction id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the caseaction in database
            CaseAction caseaction = caseactionRepository.Find(id);
            if (caseaction == null)
            {
                //set error message if it does not exist in database
                caseaction = new CaseAction();
                caseaction.ErrorMessage = "CaseAction not found";
            }
            else
            {
                try
                {
                    bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAction, Constants.Actions.Delete, true);
                    if (!hasAccess)
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    }

                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseaction.CaseMember != null ? caseaction.CaseMember.CaseID : (caseaction.CaseWorker!=null?caseaction.CaseWorker.CaseID:0));
                    //if (caseaction.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>

                    //delete caseaction from database
                    caseactionRepository.Delete(caseaction);
                    caseactionRepository.Save();
                    //set success message
                    caseaction.SuccessMessage = "Case Progress Note Action has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    caseaction.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        caseaction.SuccessMessage = "Case Progress Note Action has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        caseaction.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (caseaction.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseaction) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseaction) });
            }
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult CloseAjax(string ids)
        {
            //find the caseaction in database
            BaseModel statusModel = caseactionRepository.CloseAction(ids);
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

