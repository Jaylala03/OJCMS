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
using eCMS.Web.Controllers;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.Web.Areas.Lookup.Controllers
{
    public class SubProgramController : BaseController
    {
        public SubProgramController(ISubProgramRepository subprogramRepository,
            IProgramRepository programRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.subprogramRepository = subprogramRepository;
            this.programRepository = programRepository;
        }

        /// <summary>
        /// This action returns the list of SubProgram
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchSubProgram">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            SubProgram subProgram = new SubProgram();
            return View(subProgram);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = subprogramRepository.All.Select(subProgram => new { subProgram.ID, subProgram.CreateDate, subProgram.LastUpdateDate, subProgram.Name, subProgram.Description, subProgram.IsActive,ProgramName=subProgram.Program.Name }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens subProgram editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">subProgram id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            SubProgram subProgram = null;
            if (id > 0)
            {
                //find an existing subProgram from database
                subProgram = subprogramRepository.Find(id);
                if (subProgram == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "SubProgram not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                subProgram = new SubProgram();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, subProgram));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="subProgram">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(SubProgram subProgram)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = subProgram.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    subprogramRepository.InsertOrUpdate(subProgram);
                    subprogramRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        subProgram.SuccessMessage = "SubProgram has been added successfully";
                    }
                    else
                    {
                        subProgram.SuccessMessage = "SubProgram has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    subProgram.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    subProgram.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        subProgram.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (subProgram.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (subProgram.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, subProgram) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, subProgram) });
            }
        }

        /// <summary>
        /// delete subProgram from database usign ajax operation
        /// </summary>
        /// <param name="id">subProgram id</param>
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
            //find the subProgram in database
            SubProgram subProgram = subprogramRepository.Find(id);
            if (subProgram == null)
            {
                //set error message if it does not exist in database
                subProgram = new SubProgram();
                subProgram.ErrorMessage = "SubProgram not found";
            }
            else
            {
                try
                {
                    //delete subProgram from database
                    subprogramRepository.Delete(subProgram);
                    subprogramRepository.Save();
                    //set success message
                    subProgram.SuccessMessage = "SubProgram has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    subProgram.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    subProgram.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (subProgram.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, subProgram) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, subProgram) });
            }
        }

    }
}

