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
using eCMS.DataLogic.ViewModels;
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseProgressNoteController : CaseBaseController
    {
        private readonly ICaseProgressNoteRepository caseprogressnoteRepository;

        private readonly ICaseAssessmentRepository caseassessmentRepository;
        private readonly ICaseGoalRepository caseGoalRepository;
        private readonly IWorkerNotificationRepository workernotificationRepository;
        public CaseProgressNoteController(IWorkerRepository workerRepository,
            ICaseMemberRepository casememberRepository,
            ITimeSpentRepository timespentRepository,
            IContactMethodRepository contactmethodRepository,
            ICaseProgressNoteRepository caseprogressnoteRepository,

            ICaseRepository caseRepository,
            IActivityTypeRepository activitytypeRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseMemberProfileRepository casememberprofileRepository,
            ICaseAssessmentRepository caseassessmentRepository,
            ICaseGoalRepository caseGoalRepository,
            IWorkerNotificationRepository workernotificationRepository,
             ICaseProgressNoteMembersRepository caseprogressnoteMembersRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.casememberRepository = casememberRepository;
            this.activitytypeRepository = activitytypeRepository;
            this.timespentRepository = timespentRepository;
            this.contactmethodRepository = contactmethodRepository;
            this.caseprogressnoteRepository = caseprogressnoteRepository;
            this.casememberprofileRepository = casememberprofileRepository;
            this.caseassessmentRepository = caseassessmentRepository;
            this.caseGoalRepository = caseGoalRepository;
            this.workernotificationRepository = workernotificationRepository;
            this.caseProgressNoteMembersRepository = caseprogressnoteMembersRepository;
        }

        /// <summary>
        /// This action returns the list of CaseProgressNote
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseProgressNote">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index(int caseID, int? caseMemberID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseProgressNote caseProgressNote = new CaseProgressNote();
            //caseProgressNote.ActivityTypeID = 1;
            caseProgressNote.CaseID = caseID;
            if (caseMemberID.HasValue && caseMemberID > 0)
            {
                caseProgressNote.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseProgressNote.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                    int muridProfileCount = casememberprofileRepository.All.Count(item => item.CaseMember.CaseID == caseID && item.CaseMemberID == caseMemberID.Value);
                    if (muridProfileCount == 0)
                    {
                        caseProgressNote.ErrorMessage = "There is no profile created for family or family member " + caseMember.FirstName + " " + caseMember.LastName;
                    }
                    else
                    {
                        int assessmentCount = caseassessmentRepository.All.Count(item => item.CaseMember.CaseID == caseID && item.CaseMemberID == caseMemberID.Value);
                        if (assessmentCount == 0)
                        {
                            caseProgressNote.ErrorMessage = "There is no assessment created for family or family member " + caseMember.FirstName + " " + caseMember.LastName;
                        }
                        else
                        {
                            int goalCount = caseGoalRepository.All.Count(item => item.CaseMember.CaseID == caseID && item.CaseMemberID == caseMemberID.Value);
                            if (goalCount == 0)
                            {
                                caseProgressNote.ErrorMessage = "There is no goal identified for family or family member " + caseMember.FirstName + " " + caseMember.LastName;
                            }
                        }
                    }
                }
            }
            else
            {
                caseProgressNote.CaseMemberID = 0;
                int muridProfileCount = casememberprofileRepository.All.Count(item => item.CaseMember.CaseID == caseID);
                if (muridProfileCount == 0)
                {
                    caseProgressNote.ErrorMessage = "There is no profile created for this case";
                }
                else
                {
                    int assessmentCount = caseassessmentRepository.All.Count(item => item.CaseMember.CaseID == caseID);
                    if (assessmentCount == 0)
                    {
                        caseProgressNote.ErrorMessage = "There is no assessment created for this case";
                    }
                    else
                    {
                        int goalCount = caseGoalRepository.All.Count(item => item.CaseMember.CaseID == caseID);
                        if (goalCount == 0)
                        {
                            caseProgressNote.ErrorMessage = "There is no goal identified for this case";
                        }
                    }
                }
            }
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                caseProgressNote.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseProgressNote.CaseMemberID);
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
            return View(caseProgressNote);
        }

        [WorkerAuthorize]
        public ViewResult MyNotification()
        {
            return View();
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult MyNotificationAjax([DataSourceRequest] DataSourceRequest dsRequest)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = caseprogressnoteRepository.MyNotification(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult WorkerNotificationAjax([DataSourceRequest] DataSourceRequest dsRequest)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = workernotificationRepository.All.Where(item => item.WorkerID == CurrentLoggedInWorker.ID).Select(item => new { item.CreateDate, item.Notification }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action creates new caseprogressnote
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int caseID, int? caseMemberID)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Create, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //create a new instance of caseprogressnote
            CaseProgressNote caseProgressNote = new CaseProgressNote();
            caseProgressNote.CaseID = caseID;
            caseProgressNote.IsInitialContact = false;
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                caseProgressNote.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseProgressNote.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            return View(caseProgressNote);
        }

        /// <summary>
        /// This action saves new caseprogressnote to database
        /// </summary>
        /// <param name="caseProgressNote">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseProgressNote caseProgressNote, int caseID, int? caseMemberID)
        {
            caseProgressNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //if (caseProgressNote.NoteDate > DateTime.Today)
                //{
                //    throw new CustomException("Note date can't be future date.");
                //}

                //validate data
                if (ModelState.IsValid)
                {
                    caseProgressNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    caseprogressnoteRepository.InsertOrUpdate(caseProgressNote);
                    caseprogressnoteRepository.Save();

                    if (caseProgressNote.IsInitialContact)
                    {
                        if (caseProgressNote.CaseMembersIds != null && caseProgressNote.CaseMembersIds.Count() > 0)
                        {
                            foreach (var item in caseProgressNote.CaseMembersIds)
                            {
                                CaseProgressNoteMembers caseProgressNoteMembers = new CaseProgressNoteMembers();
                                caseProgressNoteMembers.CaseMemberID = Convert.ToInt32(item);
                                caseProgressNoteMembers.CaseProgressNoteID = Convert.ToInt32(caseProgressNote.ID);
                                caseProgressNoteMembersRepository.InsertOrUpdate(caseProgressNoteMembers);
                                caseProgressNoteMembersRepository.Save();
                            }
                        }
                        else
                            throw new CustomException("Select atleast one family member");
                    }
                    else
                    {
                        if (caseProgressNote.CaseMemberID > 0)
                        {
                            CaseProgressNoteMembers caseProgressNoteMembers = new CaseProgressNoteMembers();
                            caseProgressNoteMembers.CaseMemberID = caseProgressNote.CaseMemberID;
                            caseProgressNoteMembers.CaseProgressNoteID = Convert.ToInt32(caseProgressNote.ID);
                            caseProgressNoteMembersRepository.InsertOrUpdate(caseProgressNoteMembers);
                            caseProgressNoteMembersRepository.Save();
                        }
                        else
                            throw new CustomException("Please select family or family member");
                    }
                    //Audit Log
                    if (caseProgressNote.IsInitialContact)
                    {
                        return RedirectToAction(Constants.Actions.Index, Constants.Controllers.CaseWorker, new { caseID = caseProgressNote.CaseID, caseMemberID = caseMemberID });
                    }
                    else
                    {
                        return RedirectToAction(Constants.Actions.Edit, new { noteID = caseProgressNote.ID, caseID = caseProgressNote.CaseID, caseMemberID = caseMemberID });
                    }
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseProgressNote.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseProgressNote.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                caseProgressNote.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseProgressNote.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                caseProgressNote.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseProgressNote.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            return View(caseProgressNote);
        }

        /// <summary>
        /// This action edits an existing caseprogressnote
        /// </summary>
        /// <param name="id">caseprogressnote id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int noteID, int caseID, int? caseMemberID)
        {
            Case varCase = caseRepository.Find(caseID);

            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Edit, true);

            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the existing caseprogressnote from database
            CaseProgressNote caseProgressNote = caseprogressnoteRepository.Find(noteID);
            caseProgressNote.CaseID = caseID;
            List<string> MemberList = new List<string>();
            if (caseProgressNote.CaseMemberID > 0)
            {
                MemberList.Add(caseProgressNote.CaseMemberID.ToString());
            }
            else
            {

                var caseProgressNoteMembers = caseProgressNoteMembersRepository.SearchMembers(caseProgressNote.ID);
                if (caseProgressNoteMembers != null && caseProgressNoteMembers.Count > 0)
                {
                    foreach (var item in caseProgressNoteMembers)
                    {
                        MemberList.Add(item.CaseMemberID.ToString());
                    }
                }

            }
            if (MemberList.Count > 0)
            {
                string[] members = MemberList.ToArray();
                caseProgressNote.CaseMembersIds = members;
            }
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                caseProgressNote.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseProgressNote.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                //Case varCase = caseRepository.Find(caseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(caseProgressNote);
        }

        [WorkerAuthorize]
        public ActionResult Read(int noteID, int caseID, int? caseMemberID)
        {
            Case varCase = caseRepository.Find(caseID);

            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Read, true);

            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            //find the existing caseprogressnote from database
            CaseProgressNote caseProgressNote = caseprogressnoteRepository.Find(noteID);
            caseProgressNote.CaseID = caseID;
            List<string> MemberList = new List<string>();
            if (caseProgressNote.CaseMemberID > 0)
            {
                MemberList.Add(caseProgressNote.CaseMemberID.ToString());
            }
            else
            {

                var caseProgressNoteMembers = caseProgressNoteMembersRepository.SearchMembers(caseProgressNote.ID);
                if (caseProgressNoteMembers != null && caseProgressNoteMembers.Count > 0)
                {
                    foreach (var item in caseProgressNoteMembers)
                    {
                        MemberList.Add(item.CaseMemberID.ToString());
                    }
                }

            }
            if (MemberList.Count > 0)
            {
                string[] members = MemberList.ToArray();
                caseProgressNote.CaseMembersIds = members;
            }
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                caseProgressNote.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseProgressNote.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                //Case varCase = caseRepository.Find(caseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return editor view
            return View(caseProgressNote);
        }

        /// <summary>
        /// This action saves an existing caseprogressnote to database
        /// </summary>
        /// <param name="caseprogressnote">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseProgressNote caseprogressnote, int caseID, int? caseMemberID)
        {
            caseprogressnote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //if (caseprogressnote.NoteDate > DateTime.Today)
                //{
                //    throw new CustomException("Note date can't be future date.");
                //}
                //validate data
                if (ModelState.IsValid)
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (caseprogressnote.ID > 0 && caseprogressnote.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    //return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>

                    //call the repository function to save in database
                    caseprogressnoteRepository.InsertOrUpdate(caseprogressnote);
                    caseprogressnoteRepository.Save();
                    if (caseprogressnote.CaseMembersIds != null && caseprogressnote.CaseMembersIds.Count() > 0)
                    {

                        foreach (var item in caseprogressnote.CaseMembersIds)
                        {

                            CaseProgressNoteMembers caseProgressNoteMembers = new CaseProgressNoteMembers();
                            caseProgressNoteMembers.CaseMemberID = Convert.ToInt32(item);
                            caseProgressNoteMembers.CaseProgressNoteID = Convert.ToInt32(caseprogressnote.ID);
                            caseProgressNoteMembersRepository.InsertOrUpdate(caseProgressNoteMembers);
                            caseProgressNoteMembersRepository.Save();
                        }



                    }

                    //redirect to list page after successful operation
                    if (caseprogressnote.IsInitialContact)
                    {
                        return RedirectToAction(Constants.Actions.InitialContact, new { caseID = caseprogressnote.CaseID });
                    }
                    else
                    {
                        return RedirectToAction(Constants.Actions.Edit, new { noteID = caseprogressnote.ID, caseID = caseprogressnote.CaseID });
                    }
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseprogressnote.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseprogressnote.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                caseprogressnote.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseprogressnote.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                caseprogressnote.CaseMemberID = caseMemberID.Value;
                CaseMember caseMember = casememberRepository.Find(caseprogressnote.CaseMemberID);
                if (caseMember != null)
                {
                    ViewBag.DisplayID = caseMember.DisplayID;
                }
            }
            else
            {
                Case varCase = caseRepository.Find(caseID);
                if (varCase != null)
                {
                    ViewBag.DisplayID = varCase.DisplayID;
                }
            }
            //return view with error message if the operation is failed
            return View(caseprogressnote);
        }


        /// <summary>
        /// This action creates new caseprogressnote
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult InitialContact(int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseProgressNote caseprogressnote = caseprogressnoteRepository.FindInitialNoteByCaseID(caseID);
            if (caseprogressnote == null)
            {
                return RedirectToAction(Constants.Actions.CreateInitialContact, Constants.Controllers.CaseProgressNote, new { caseID = caseID });
            }
            caseprogressnote.CaseID = caseID;
            ViewBag.DisplayID = caseRepository.Find(caseID).DisplayID;
            return View(caseprogressnote);
        }

        [WorkerAuthorize]
        public ActionResult InitialContactRead(int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseProgressNote caseprogressnote = caseprogressnoteRepository.FindInitialNoteByCaseID(caseID);
            if (caseprogressnote == null)
            {
                return RedirectToAction(Constants.Actions.CreateInitialContactRead, Constants.Controllers.CaseProgressNote, new { caseID = caseID });
            }
            caseprogressnote.CaseID = caseID;
            ViewBag.DisplayID = caseRepository.Find(caseID).DisplayID;
            return View(caseprogressnote);
        }

        [WorkerAuthorize]
        public ActionResult CreateInitialContact(int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.CreateInitialContact, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseProgressNote caseprogressnote = caseprogressnoteRepository.FindInitialNoteByCaseID(caseID);
            if (caseprogressnote != null)
            {
                return RedirectToAction(Constants.Actions.InitialContact, Constants.Controllers.CaseProgressNote, new { caseID = caseID });
            }
            if (caseprogressnote == null)
            {
                caseprogressnote = new CaseProgressNote();
                caseprogressnote.CaseID = caseID;
                caseprogressnote.ActivityTypeID = 2;
                caseprogressnote.ContactMethodID = 1;
                ViewBag.DisplayID = caseRepository.Find(caseID).DisplayID;
            }
            return View(caseprogressnote);
        }

        [WorkerAuthorize]
        public ActionResult CreateInitialContactRead(int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.CreateInitialContact, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseProgressNote caseprogressnote = caseprogressnoteRepository.FindInitialNoteByCaseID(caseID);
            if (caseprogressnote != null)
            {
                return RedirectToAction(Constants.Actions.InitialContact, Constants.Controllers.CaseProgressNote, new { caseID = caseID });
            }
            if (caseprogressnote == null)
            {
                caseprogressnote = new CaseProgressNote();
                caseprogressnote.CaseID = caseID;
                caseprogressnote.ActivityTypeID = 2;
                caseprogressnote.ContactMethodID = 1;
                ViewBag.DisplayID = caseRepository.Find(caseID).DisplayID;
            }
            return View(caseprogressnote);
        }
        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId, int? caseMemberID)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = caseprogressnoteRepository.Search(dsRequest, caseId, CurrentLoggedInWorker.ID, caseMemberID, CurrentLoggedInWorkerRoleIDs);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens caseprogressnote editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">caseprogressnote id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string caseId)
        {
            CaseProgressNote caseprogressnote = null;
            if (id > 0)
            {
                Case varCase = caseRepository.Find(Convert.ToInt32(caseId));
                bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(Convert.ToInt32(caseId), CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Edit, true);
                if (!hasAccess)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                }

                //find an existing caseprogressnote from database
                caseprogressnote = caseprogressnoteRepository.Find(id);
                if (caseprogressnote == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Progress Note not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                caseprogressnote = new CaseProgressNote();
                caseprogressnote.CaseID = caseId.ToInteger(true);
            }

            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            ViewBag.PossibleCaseMembers = casememberRepository.All;
            //ViewBag.PossibleActivityTypes = activitytypeRepository.All;
            ViewBag.PossibleTimeSpents = timespentRepository.All;
            ViewBag.PossibleContactMethods = contactmethodRepository.All;
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.EditorPopUp, caseprogressnote));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="caseprogressnote">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseProgressNote caseprogressnote)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = caseprogressnote.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {
                if (caseprogressnote.NoteDate > DateTime.Today)
                {
                    throw new CustomException("Note date can't be future date.");
                }
                try
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (caseprogressnote.ID > 0 && caseprogressnote.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //</JL:Comment:07/08/2017>

                    caseprogressnote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    caseprogressnoteRepository.InsertOrUpdate(caseprogressnote);
                    caseprogressnoteRepository.Save();
                    //Audit log
                    var caseprogressmembers = caseProgressNoteMembersRepository.SearchMembers(caseprogressnote.ID);
                    foreach (var item in caseprogressmembers)
                    {
                        caseProgressNoteMembersRepository.Delete(item);
                    }
                    if (caseprogressnote.CaseMembersIds != null && caseprogressnote.CaseMembersIds.Count() > 0)
                    {

                        foreach (var item in caseprogressnote.CaseMembersIds)
                        {

                            CaseProgressNoteMembers caseProgressNoteMembers = new CaseProgressNoteMembers();
                            caseProgressNoteMembers.CaseMemberID = Convert.ToInt32(item);
                            caseProgressNoteMembers.CaseProgressNoteID = Convert.ToInt32(caseprogressnote.ID);
                            caseProgressNoteMembersRepository.InsertOrUpdate(caseProgressNoteMembers);
                            caseProgressNoteMembersRepository.Save();
                        }



                    }


                    if (isNew)
                    {
                        caseprogressnote.SuccessMessage = "Case Progress Note has been added successfully";
                    }
                    else
                    {
                        caseprogressnote.SuccessMessage = "Case Progress Note has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    caseprogressnote.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    caseprogressnote.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        caseprogressnote.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (caseprogressnote.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (caseprogressnote.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseprogressnote) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, caseprogressnote) });
            }
        }

        /// <summary>
        /// delete caseprogressnote from database usign ajax operation
        /// </summary>
        /// <param name="id">caseprogressnote id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the varCase in database
            BaseModel statusModel = new BaseModel();
            if (!workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Delete, true))
            {
                statusModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
            }
            try
            {
                var caseId = caseprogressnoteRepository.Find(id).CaseID;
                //delete varCase from database
                caseprogressnoteRepository.Delete(id);
                caseprogressnoteRepository.Save();



                //set success message
                statusModel.SuccessMessage = "Case progress note has been deleted successfully";
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                {
                    statusModel.SuccessMessage = "Case progress note has been deleted successfully";
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


        public JsonResult GetprogressNoteByCaseID(int CaseID)
        {
            var response = caseprogressnoteRepository.GetprogressNoteByCaseID(CaseID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Load Case Worker DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadCaseWorkerMemberListAjax(int caseID)
        {
            IEnumerable<SelectListItem> workerList;
            List<SelectListItem> memberList = new List<SelectListItem>();
            //Case Worker
            workerList = workerRepository.FindAllByCaseID(caseID);//.Select(item => new { ID = item.ID, Name = item.FirstName + " " + item.LastName });                

            //Case Member
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
            {
                memberList = casememberRepository.FindAllByCaseIDForDropDownList(caseID);

            }
            else
            {
                memberList = casememberRepository.FindAllByCaseIDAndWorkerIDForDropDownList(caseID, CurrentLoggedInWorker.ID);

            }
            if (memberList != null && memberList.Count > 0)
            {
                memberList = memberList.Select(x => new SelectListItem { Text = x.Text + "-M", Value = x.Value + "-M" }).ToList();
            }
            if (workerList != null && workerList.Count() > 0)
            {
                workerList = workerList.Select(x => new SelectListItem { Text = x.Text + "-W", Value = x.Value + "-W" }).ToList();
            }
            if (memberList != null && memberList.Count > 0)
            {
                workerList = workerList.Concat(memberList);
            }
            return Json(workerList, JsonRequestBehavior.AllowGet);

        }

    }
}

