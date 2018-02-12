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
using System.Text;
using System.Web.Mvc;
namespace eCMS.Web.Areas.Lookup.Controllers
{
    public class WorkerRolePermissionController : BaseController
    {
        private readonly IWorkerRolePermissionNewRepository wokerrolepermissionnewRepository;
        public WorkerRolePermissionController(IWorkerRolePermissionNewRepository wokerrolepermissionnewRepository,
             IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
             IWorkerRoleRepository workerroleRepository,
             IPermissionRepository permissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.wokerrolepermissionnewRepository = wokerrolepermissionnewRepository;
            this.regionRepository = regionRepository;
            this.workerroleRepository = workerroleRepository;
            this.permissionRepository = permissionRepository;
        }

        /// <summary>
        /// This action returns the list of Worker roler permissions
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchService">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of wokerrolepermission
            WorkerRolePermissionNew wokerrolepermission = new WorkerRolePermissionNew();
            //return view result
            return View(wokerrolepermission);
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
            DataSourceResult result = wokerrolepermissionnewRepository.GetIndexList(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens worker role permission editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">worker role permission id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            WorkerRolePermissionNew rolepermission = null;
            if (id > 0)
            {
                //find an existing rolepermission from database
                rolepermission = wokerrolepermissionnewRepository.FindByID(id);
                if (rolepermission == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Role permission not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                rolepermission = new WorkerRolePermissionNew();
            }
            //rolepermission = new WorkerRolePermissionNew();
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, rolepermission));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="rolepermission">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(WorkerRolePermissionNew rolepermission)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = rolepermission.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {
                try
                {
                    //call repository function to save the data in database
                    wokerrolepermissionnewRepository.InsertOrUpdate(rolepermission);
                    wokerrolepermissionnewRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        rolepermission.SuccessMessage = "Role permission has been added successfully";
                    }
                    else
                    {
                        rolepermission.SuccessMessage = "Role permission has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    rolepermission.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    rolepermission.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        rolepermission.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (rolepermission.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (rolepermission.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = rolepermission.ErrorMessage });
            }
            else
            {
                return Json(new { success = true, data = rolepermission.SuccessMessage });
            }
        }

        /// <summary>
        /// delete rolepermission from database usign ajax operation
        /// </summary>
        /// <param name="id">rolepermission id</param>
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
            WorkerRolePermissionNew rolepermission = wokerrolepermissionnewRepository.Find(id);
            if (rolepermission == null)
            {
                //set error message if it does not exist in database
                rolepermission = new WorkerRolePermissionNew();
                rolepermission.ErrorMessage = "Role permission not found";
            }
            else
            {
                try
                {
                    //delete rolepermission from database
                    wokerrolepermissionnewRepository.Delete(rolepermission);
                    wokerrolepermissionnewRepository.Save();
                    //set success message
                    rolepermission.SuccessMessage = "Role permission has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    rolepermission.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    rolepermission.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (rolepermission.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, rolepermission) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, rolepermission) });
            }
        }

        //public JsonResult LoadPermissionAjax()
        //{
        //    List<SelectListItem> permissionList = new List<SelectListItem>();
        //    permissionList = permissionRepository.GetAll();
        //    return Json(permissionList, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult LoadWorkerRoleAjax()
        //{
        //    List<SelectListItem> workerroleList = new List<SelectListItem>();
        //    workerroleList = workerroleRepository.GetAll();
        //    return Json(workerroleList, JsonRequestBehavior.AllowGet);
        //}
    }
}

