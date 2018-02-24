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
using eCMS.Web.Areas.CaseManagement.Controllers;
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
    public class CaseWorkerNoteController : BaseController
    {
        private readonly ICaseWorkerNoteRepository caseWorkerNoteRepository;

        public CaseWorkerNoteController(IContactMethodRepository contactmethodRepository, IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, ICaseWorkerNoteRepository caseWorkerNoteRepository)
            : base(workerroleactionpermissionRepository, workerroleactionpermissionnewRepository)
        {
            this.contactmethodRepository = contactmethodRepository;
            this.caseWorkerNoteRepository = caseWorkerNoteRepository;
        }

        
        [WorkerAuthorize]
        public ActionResult Index(int CaseId, int ProgramID)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseWorkerNote caseWorkerNote = new CaseWorkerNote();
            caseWorkerNote.CaseID = CaseId;
            caseWorkerNote.ProgramID = ProgramID;
            return View(caseWorkerNote);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest,int CaseId, int ProgramID)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = caseWorkerNoteRepository.Search(dsRequest,CaseId, ProgramID); 
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens caseWorkerNote editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">caseWorkerNote id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            CaseWorkerNote caseWorkerNote = null;
            if (id > 0)
            {
                //find an existing caseWorkerNote from database
                caseWorkerNote = caseWorkerNoteRepository.Find(id);

                if (caseWorkerNote.IsFamily == true  && caseWorkerNote.IsFamilyMember == false)
                {
                    caseWorkerNote.Family = "Family";
                }
                else
                {
                    caseWorkerNote.Family = "FamilyMember";
                }

                if (caseWorkerNote == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Work Note not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                caseWorkerNote = new CaseWorkerNote();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, caseWorkerNote));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="caseWorkerNote">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseWorkerNote caseWorkerNote)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = caseWorkerNote.ID == 0;
            caseWorkerNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;            
            
            //validate data
            if (ModelState.IsValid)
            {
               
                try
                {
                    if (caseWorkerNote.TimeSpentHours == 0 && caseWorkerNote.TimeSpentMinutes == 0)
                    {
                        CustomException ex = new CustomException(CustomExceptionType.CommonServerError, "Please enter time spent.");
                        throw ex;
                    }

                    if (caseWorkerNote.Family == "Family")
                    {
                        caseWorkerNote.IsFamily = true;
                        caseWorkerNote.IsFamilyMember = false;
                    }
                    else
                    {
                        caseWorkerNote.IsFamily = false;
                        caseWorkerNote.IsFamilyMember = true;
                    }


                    //call repository function to save the data in database
                    caseWorkerNoteRepository.InsertOrUpdate(caseWorkerNote);
                    caseWorkerNoteRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        caseWorkerNote.SuccessMessage = "Work Note has been added successfully";
                    }
                    else
                    {
                        caseWorkerNote.SuccessMessage = "Work Note has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    caseWorkerNote.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    caseWorkerNote.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        caseWorkerNote.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (caseWorkerNote.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (caseWorkerNote.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, caseWorkerNote) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, caseWorkerNote) });
            }
        }

        /// <summary>
        /// delete caseWorkerNote from database usign ajax operation
        /// </summary>
        /// <param name="id">caseWorkerNote id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                BaseModel baseModel = new BaseModel();
                baseModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, baseModel) }, JsonRequestBehavior.AllowGet);
            }
            //find the caseWorkerNote in database
            CaseWorkerNote caseWorkerNote = caseWorkerNoteRepository.Find(id);
            if (caseWorkerNote == null)
            {
                //set error message if it does not exist in database
                caseWorkerNote = new CaseWorkerNote();
                caseWorkerNote.ErrorMessage = "caseWorkerNote not found";
            }
            else
            {
                try
                {
                    //delete caseWorkerNote from database
                    caseWorkerNoteRepository.Delete(id);
                    caseWorkerNoteRepository.Save();
                    //set success message
                    caseWorkerNote.SuccessMessage = "Work Note has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    caseWorkerNote.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    caseWorkerNote.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (caseWorkerNote.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, caseWorkerNote) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, caseWorkerNote) });
            }
        }

    }
}

