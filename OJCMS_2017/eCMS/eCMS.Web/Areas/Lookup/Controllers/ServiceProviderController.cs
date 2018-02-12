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
    public class ServiceProviderController : BaseController
    {
        private readonly IServiceProviderRepository serviceproviderRepository;
        public ServiceProviderController(IServiceProviderRepository serviceproviderRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
              IRegionRepository regionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.serviceproviderRepository = serviceproviderRepository;
            this.regionRepository = regionRepository;
        }

        /// <summary>
        /// This action returns the list of ServiceProvider
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchServiceProvider">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule && !ViewBag.HasAccessToOtherConfigurationData)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of serviceprovider
            ServiceProvider serviceprovider = new ServiceProvider();
            //return view result
            return View(serviceprovider);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, ServiceProvider searchProvider)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            //var data = regionRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID, 0).Where(item => item.IsActive == true).Select(m=>m.ID).ToList();
            DataSourceResult result = serviceproviderRepository.FindAllByRegion(searchProvider,dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens serviceprovider editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">serviceprovider id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            ServiceProvider serviceprovider = null;
            if (id > 0)
            {
                //find an existing serviceprovider from database
                serviceprovider = serviceproviderRepository.Find(id);
                if (serviceprovider == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "ServiceProvider not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                serviceprovider = new ServiceProvider();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, serviceprovider));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="serviceprovider">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(ServiceProvider serviceprovider)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = serviceprovider.ID == 0;
            if (serviceprovider.RegionID == null || serviceprovider.RegionID == 0)
            {
                serviceprovider.ErrorMessage = "Region is required";
               
            }
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //var data = regionRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID, 0).Where(item => item.IsActive == true).Select(m=>m.ID).ToList();
                    //if (!data.Contains(Convert.ToInt32(serviceprovider.RegionID)))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //call repository function to save the data in database
                    serviceproviderRepository.InsertOrUpdate(serviceprovider);
                    serviceproviderRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        serviceprovider.SuccessMessage = "ServiceProvider has been added successfully";
                    }
                    else
                    {
                        serviceprovider.SuccessMessage = "ServiceProvider has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    serviceprovider.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    serviceprovider.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        serviceprovider.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (serviceprovider.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (serviceprovider.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = serviceprovider.ErrorMessage });
            }
            else
            {
                return Json(new { success = true, data = serviceprovider.SuccessMessage });
            }
        }

        /// <summary>
        /// delete serviceprovider from database usign ajax operation
        /// </summary>
        /// <param name="id">serviceprovider id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            if (!ViewBag.HasAccessToAdminModule && !ViewBag.HasAccessToOtherConfigurationData)
            {
                BaseModel baseModel = new BaseModel();
                baseModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, baseModel) }, JsonRequestBehavior.AllowGet);
            }
            //find the serviceprovider in database
            ServiceProvider serviceprovider = serviceproviderRepository.Find(id);
            if (serviceprovider == null)
            {
                //set error message if it does not exist in database
                serviceprovider = new ServiceProvider();
                serviceprovider.ErrorMessage = "ServiceProvider not found";
            }
            else
            {
                try
                {
                    //delete serviceprovider from database
                    serviceproviderRepository.Delete(serviceprovider);
                    serviceproviderRepository.Save();
                    //set success message
                    serviceprovider.SuccessMessage = "ServiceProvider has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    serviceprovider.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    serviceprovider.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (serviceprovider.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, serviceprovider) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, serviceprovider) });
            }
        }


       

    }
}

