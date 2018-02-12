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
    public class WorkerRoleController : BaseController
    {
        public WorkerRoleController(IWorkerRoleRepository workerroleRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.workerroleRepository = workerroleRepository;
        }

        /// <summary>
        /// This action returns the list of Role
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchRole">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of role
            ViewBag.IsWorkerAdministrator = (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 ? false : true);
            WorkerRole role = new WorkerRole();
            //return view result
            return View(role);
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
            DataSourceResult result = workerroleRepository.All.Select(role => new { role.ID, role.CreateDate, role.LastUpdateDate, role.Name, role.Description, role.IsActive }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens role editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">role id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            WorkerRole role = null;
            if (id > 0)
            {
                //find an existing role from database
                role = workerroleRepository.Find(id);
                if (role == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Role not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                role = new WorkerRole();
            }
            ViewBag.IsWorkerAdministrator = (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 ? false : true);
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, role));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="role">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(WorkerRole role)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = role.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    workerroleRepository.InsertOrUpdate(role);
                    workerroleRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        role.SuccessMessage = "Role has been added successfully";
                    }
                    else
                    {
                        role.SuccessMessage = "Role has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    role.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    role.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        role.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (role.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (role.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = role.ErrorMessage });
            }
            else
            {
                return Json(new { success = true, data = role.SuccessMessage });
            }
        }

        /// <summary>
        /// delete role from database usign ajax operation
        /// </summary>
        /// <param name="id">role id</param>
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
            //find the role in database
            WorkerRole role = workerroleRepository.Find(id);
            if (role == null)
            {
                //set error message if it does not exist in database
                role = new WorkerRole();
                role.ErrorMessage = "Role not found";
            }
            else
            {
                try
                {
                    //delete role from database
                    workerroleRepository.Delete(role);
                    workerroleRepository.Save();
                    //set success message
                    role.SuccessMessage = "Role has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    role.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    role.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (role.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, role) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, role) });
            }
        }

    }
}

