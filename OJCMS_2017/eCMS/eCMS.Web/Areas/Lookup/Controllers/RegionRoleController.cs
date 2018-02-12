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
using eCMS.DataLogic.ViewModels;
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
    public class RegionRoleController : BaseController
    {
        public RegionRoleController(IRegionRepository regionRepository, 
            IWorkerRoleRepository workerroleRepository,
            IRegionRoleRepository regionroleRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.regionRepository = regionRepository;
            this.workerroleRepository = workerroleRepository;
            this.regionroleRepository = regionroleRepository;
        }

        /// <summary>
        /// This action returns the list of RegionRole
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchRegionRole">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, RegionRole searchRegionRole)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            RegionRoleModel newRegionRoleModel = new RegionRoleModel();
            newRegionRoleModel.AllRegions = regionRepository.All.ToList();
            return View(newRegionRoleModel);
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
            DataSourceResult result = regionroleRepository.FindAllForList().ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens regionrole editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">regionrole id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int workerRoleID)
        {
            RegionRoleModel newRegionRoleModel = new RegionRoleModel();
            newRegionRoleModel.WorkerRoleID = workerRoleID;
            newRegionRoleModel.AllRegions = regionRepository.All.ToList();
            newRegionRoleModel.AssignedRegions = regionroleRepository.FindAllByWorkerRoleID(workerRoleID).ToList();
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, newRegionRoleModel));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="regionrole">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(RegionRoleModel regionrole)
        {
            try
            {
                //call repository function to save the data in database
                regionroleRepository.InsertOrUpdate(regionrole.WorkerRoleID, Request.Form["SelectedRegion"].ToString(true));
                regionroleRepository.Save();
                //set status message
                regionrole.SuccessMessage = "Data has been savedcsuccessfully";
            }
            catch (CustomException ex)
            {
                regionrole.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                regionrole.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return the status message in json
            if (regionrole.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, regionrole) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, regionrole) });
            }
        }

        /// <summary>
        /// delete regionrole from database usign ajax operation
        /// </summary>
        /// <param name="id">regionrole id</param>
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
            //find the regionrole in database
            RegionRole regionrole = regionroleRepository.Find(id);
            if (regionrole == null)
            {
                //set error message if it does not exist in database
                regionrole = new RegionRole();
                regionrole.ErrorMessage = "RegionRole not found";
            }
            else
            {
                try
                {
                    //delete regionrole from database
                    regionroleRepository.Delete(regionrole);
                    regionroleRepository.Save();
                    //set success message
                    regionrole.SuccessMessage = "Region Role has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    regionrole.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    regionrole.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (regionrole.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, regionrole) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, regionrole) });
            }
        }

    }
}

