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
    public class CaseSSSWorkerNoteController : BaseController
    {
        private readonly ICaseSSSWorkerNoteRepository caseSSSWorkerNoteRepository;

        public CaseSSSWorkerNoteController(IContactMethodRepository contactmethodRepository, IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, ICaseSSSWorkerNoteRepository caseSSSWorkerNoteRepository)
            : base(workerroleactionpermissionRepository, workerroleactionpermissionnewRepository)
        {
            this.contactmethodRepository = contactmethodRepository;
            this.caseSSSWorkerNoteRepository = caseSSSWorkerNoteRepository;
        }

        
        [WorkerAuthorize]
        public ActionResult Index(int CaseId, int ProgramID)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseSSSWorkerNote caseSSSWorkerNote = new CaseSSSWorkerNote();
            caseSSSWorkerNote.CaseID = CaseId;
            caseSSSWorkerNote.ProgramID = ProgramID;
            return View(caseSSSWorkerNote);
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
            DataSourceResult result = caseSSSWorkerNoteRepository.Search(dsRequest,CaseId, ProgramID); 
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens caseSSSWorkerNote editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">caseSSSWorkerNote id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            CaseSSSWorkerNote caseSSSWorkerNote = null;
            if (id > 0)
            {
                //find an existing caseSSSWorkerNote from database
                caseSSSWorkerNote = caseSSSWorkerNoteRepository.Find(id);

                if (caseSSSWorkerNote.IsFamily == true  && caseSSSWorkerNote.IsFamilyMember == false)
                {
                    caseSSSWorkerNote.Family = "Family";
                }
                else
                {
                    caseSSSWorkerNote.Family = "FamilyMember";
                }

                if (caseSSSWorkerNote == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "caseSSSWorkerNote not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                caseSSSWorkerNote = new CaseSSSWorkerNote();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, caseSSSWorkerNote));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="caseSSSWorkerNote">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseSSSWorkerNote caseSSSWorkerNote)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = caseSSSWorkerNote.ID == 0;
            caseSSSWorkerNote.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;            
            
            //validate data
            if (ModelState.IsValid)
            {
               
                try
                {
                    if (caseSSSWorkerNote.TimeSpentHours == 0 && caseSSSWorkerNote.TimeSpentMinutes == 0)
                    {
                        CustomException ex = new CustomException(CustomExceptionType.CommonServerError, "Please enter time spent.");
                        throw ex;
                    }

                    if (caseSSSWorkerNote.Family == "Family")
                    {
                        caseSSSWorkerNote.IsFamily = true;
                        caseSSSWorkerNote.IsFamilyMember = false;
                    }
                    else
                    {
                        caseSSSWorkerNote.IsFamily = false;
                        caseSSSWorkerNote.IsFamilyMember = true;
                    }

                    caseSSSWorkerNote.WorkerNoteActivityTypeID = (int)eCMS.Shared.WorkerNoteActivityType.WorkNote;
                    //call repository function to save the data in database
                    caseSSSWorkerNoteRepository.InsertOrUpdate(caseSSSWorkerNote);
                    caseSSSWorkerNoteRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        caseSSSWorkerNote.SuccessMessage = "Work note has been added successfully";
                    }
                    else
                    {
                        caseSSSWorkerNote.SuccessMessage = "Work note has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    caseSSSWorkerNote.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    caseSSSWorkerNote.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        caseSSSWorkerNote.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (caseSSSWorkerNote.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (caseSSSWorkerNote.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, caseSSSWorkerNote) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, caseSSSWorkerNote) });
            }
        }

        /// <summary>
        /// delete caseSSSWorkerNote from database usign ajax operation
        /// </summary>
        /// <param name="id">caseSSSWorkerNote id</param>
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
            //find the caseSSSWorkerNote in database
            CaseSSSWorkerNote caseSSSWorkerNote = caseSSSWorkerNoteRepository.Find(id);
            if (caseSSSWorkerNote == null)
            {
                //set error message if it does not exist in database
                caseSSSWorkerNote = new CaseSSSWorkerNote();
                caseSSSWorkerNote.ErrorMessage = "Work note not found";
            }
            else
            {
                try
                {
                    //delete caseSSSWorkerNote from database
                    caseSSSWorkerNoteRepository.Delete(id);
                    caseSSSWorkerNoteRepository.Save();
                    //set success message
                    caseSSSWorkerNote.SuccessMessage = "Work note has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    caseSSSWorkerNote.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    caseSSSWorkerNote.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (caseSSSWorkerNote.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, caseSSSWorkerNote) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, caseSSSWorkerNote) });
            }
        }

    }
}

