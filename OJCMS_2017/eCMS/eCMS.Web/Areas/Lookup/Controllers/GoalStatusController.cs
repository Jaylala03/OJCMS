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
    public class GoalStatusController : BaseController
    {
        public GoalStatusController(IGoalStatusRepository goalstatusRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.goalstatusRepository = goalstatusRepository;
        }

        /// <summary>
        /// This action returns the list of GoalStatus
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchGoalStatus">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of goalstatus
            GoalStatus goalstatus = new GoalStatus();
            //return view result
            return View(goalstatus);
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
            DataSourceResult result = goalstatusRepository.All.Select(program => new { program.ID, program.CreateDate, program.LastUpdateDate, program.Name, program.Description, program.IsActive }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens program editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">program id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            GoalStatus goalstatus = null;
            if (id > 0)
            {
                //find an existing program from database
                goalstatus = goalstatusRepository.Find(id);
                if (goalstatus == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Goal Status not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                goalstatus = new GoalStatus();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, goalstatus));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="program">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(GoalStatus goalstatus)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = goalstatus.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    goalstatusRepository.InsertOrUpdate(goalstatus);
                    goalstatusRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        goalstatus.SuccessMessage = "Goal Status has been added successfully";
                    }
                    else
                    {
                        goalstatus.SuccessMessage = "Goal Status has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    goalstatus.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    goalstatus.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        goalstatus.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (goalstatus.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (goalstatus.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = goalstatus.ErrorMessage });
            }
            else
            {
                return Json(new { success = true, data = goalstatus.SuccessMessage });
            }
        }

        /// <summary>
        /// delete program from database usign ajax operation
        /// </summary>
        /// <param name="id">program id</param>
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
            //find the program in database
            GoalStatus goalstatus = goalstatusRepository.Find(id);
            if (goalstatus == null)
            {
                //set error message if it does not exist in database
                goalstatus = new GoalStatus();
                goalstatus.ErrorMessage = "Goal Status not found";
            }
            else
            {
                try
                {
                    //delete program from database
                    goalstatusRepository.Delete(goalstatus);
                    goalstatusRepository.Save();
                    //set success message
                    goalstatus.SuccessMessage = "Goal Status has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    goalstatus.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    goalstatus.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (goalstatus.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, goalstatus) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, goalstatus) });
            }
        }

    }
}

