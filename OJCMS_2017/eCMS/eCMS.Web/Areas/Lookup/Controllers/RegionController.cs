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
    public class RegionController : BaseController
    {
        public RegionController(ICountryRepository countryRepository,
            IRegionRepository regionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.countryRepository = countryRepository;
            this.regionRepository = regionRepository;
        }

        /// <summary>
        /// This action returns the list of Region
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchRegion">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            Region region = new Region();
            region.CountryID = 7;
            return View(region);
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
            DataSourceResult result = regionRepository.AllIncluding(region => region.Country).Select(region => new { region.ID, region.CreateDate, region.LastUpdateDate, region.Name, region.Description, region.IsActive }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens region editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">region id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            Region region = null;
            if (id > 0)
            {
                //find an existing region from database
                region = regionRepository.Find(id);
                if (region == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Region not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                region = new Region();
                region.CountryID = 7;
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, region));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="region">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(Region region)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = region.ID == 0;
            region.CountryID = 7;
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    regionRepository.InsertOrUpdate(region);
                    regionRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        region.SuccessMessage = "Region has been added successfully";
                    }
                    else
                    {
                        region.SuccessMessage = "Region has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    region.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    region.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        region.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (region.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (region.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, region) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, region) });
            }
        }

        /// <summary>
        /// delete region from database usign ajax operation
        /// </summary>
        /// <param name="id">region id</param>
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
            //find the region in database
            Region region = regionRepository.Find(id);
            if (region == null)
            {
                //set error message if it does not exist in database
                region = new Region();
                region.ErrorMessage = "Region not found";
            }
            else
            {
                try
                {
                    //delete region from database
                    regionRepository.Delete(region);
                    regionRepository.Save();
                    //set success message
                    region.SuccessMessage = "Region has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    region.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    region.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (region.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, region) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, region) });
            }
        }

    }
}

