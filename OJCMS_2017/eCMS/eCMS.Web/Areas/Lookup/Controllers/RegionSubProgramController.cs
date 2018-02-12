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
    public class RegionSubProgramController : BaseController
    {
        public RegionSubProgramController(IRegionRepository regionRepository, 
            IProgramRepository programRepository, 
            ISubProgramRepository subprogramRepository,
            IRegionSubProgramRepository regionsubprogramRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.programRepository = programRepository;
            this.regionRepository = regionRepository;
            this.subprogramRepository = subprogramRepository;
            this.regionsubprogramRepository = regionsubprogramRepository;
        }

        /// <summary>
        /// This action returns the list of RegionSubProgram
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchRegionSubProgram">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index()
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            RegionSubProgramModel newRegionSubProgramModel = new RegionSubProgramModel();
            newRegionSubProgramModel.AllRegions = regionRepository.All.ToList();
            return View(newRegionSubProgramModel);
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
            DataSourceResult result = regionsubprogramRepository.FindAllForList().ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens regionsubprogram editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">regionsubprogram id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int subprogramID)
        {
            RegionSubProgramModel newRegionSubProgramModel = new RegionSubProgramModel();
            newRegionSubProgramModel.SubProgramID = subprogramID;
            newRegionSubProgramModel.AllRegions = regionRepository.All.ToList();
            newRegionSubProgramModel.AssignedRegions = regionsubprogramRepository.FindAllBySubProgramID(subprogramID).ToList();
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, newRegionSubProgramModel));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="regionsubprogram">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(RegionSubProgramModel regionsubprogram)
        {
            try
            {
                //call repository function to save the data in database
                regionsubprogramRepository.InsertOrUpdate(regionsubprogram.SubProgramID, Request.Form["SelectedRegion"].ToString(true));
                regionsubprogramRepository.Save();
                //set status message
                regionsubprogram.SuccessMessage = "Data has been savedcsuccessfully";
            }
            catch (CustomException ex)
            {
                regionsubprogram.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                regionsubprogram.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return the status message in json
            if (regionsubprogram.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, regionsubprogram) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, regionsubprogram) });
            }
        }

        /// <summary>
        /// delete regionsubprogram from database usign ajax operation
        /// </summary>
        /// <param name="id">regionsubprogram id</param>
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
            //find the regionsubprogram in database
            RegionSubProgram regionsubprogram = regionsubprogramRepository.Find(id);
            if (regionsubprogram == null)
            {
                //set error message if it does not exist in database
                regionsubprogram = new RegionSubProgram();
                regionsubprogram.ErrorMessage = "RegionSubProgram not found";
            }
            else
            {
                try
                {
                    //delete regionsubprogram from database
                    regionsubprogramRepository.Delete(regionsubprogram);
                    regionsubprogramRepository.Save();
                    //set success message
                    regionsubprogram.SuccessMessage = "Region Role has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    regionsubprogram.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    regionsubprogram.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (regionsubprogram.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, regionsubprogram) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, regionsubprogram) });
            }
        }

    }
}

