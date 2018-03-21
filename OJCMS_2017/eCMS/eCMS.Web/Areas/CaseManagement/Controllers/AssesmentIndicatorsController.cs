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
using eCMS.ExceptionLoging;
using eCMS.Shared;
using eCMS.Web.Areas.CaseManagement.Controllers;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using eCMS.BusinessLogic;
using System.Configuration;
using eCMS.DataLogic.ViewModels;
using eCMS.Web.Controllers;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class AssesmentIndicatorsController : CaseBaseController
    {
        private readonly IAssesmentIndicatorsRepository assesmentIndicatorsRepository;
        private readonly IIndicatorTypeRepository indicatorTypeRepository;
        
        public AssesmentIndicatorsController(IAssesmentIndicatorsRepository assesmentIndicatorsRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository, 
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {            
            this.assesmentIndicatorsRepository = assesmentIndicatorsRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
        }

        /// <summary>
        /// This action returns the list of AssesmentIndicators
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchAssesmentIndicators">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            AssesmentIndicators assesmentIndicators = new AssesmentIndicators();
            return View(assesmentIndicators);
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
            DataSourceResult result = assesmentIndicatorsRepository.All.Select(assesmentIndicators => new { assesmentIndicators.ID, IndicatorTypeName = assesmentIndicators.IndicatorType.Name, assesmentIndicators.CreateDate, assesmentIndicators.LastUpdateDate, assesmentIndicators.IndicatorTYpeID, assesmentIndicators.Description1, assesmentIndicators.Description2 }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadIndicatorTypeListAjax()
        {
            IQueryable<IndicatorType> indicatorList;

            indicatorList = indicatorTypeRepository.GetAll();

            return Json(indicatorList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens AssesmentIndicator editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">AssesmentIndicator id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            AssesmentIndicators assesmentIndicators = null;
            if (id > 0)
            {
                //find an existing assesmentIndicator from database
                assesmentIndicators = assesmentIndicatorsRepository.Find(id);
                if (assesmentIndicators == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Assesment Indicator not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                assesmentIndicators = new AssesmentIndicators();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, assesmentIndicators));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="AssesmentIndicator">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(AssesmentIndicators assesmentIndicators)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = assesmentIndicators.ID == 0;
            assesmentIndicators.CreatedByWorkerID = CurrentLoggedInWorker.ID;
            assesmentIndicators.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            //validate data
            if (ModelState.IsValid)
            {
                try
                {
                    //call repository function to save the data in database
                    assesmentIndicators.Description1 = assesmentIndicators.Description1 == null ? string.Empty : assesmentIndicators.Description1;
                    assesmentIndicators.Description2 = assesmentIndicators.Description2 == null ? string.Empty : assesmentIndicators.Description2;
                    assesmentIndicators.Description3 = assesmentIndicators.Description3 == null ? string.Empty : assesmentIndicators.Description3;
                    assesmentIndicators.Description4 = assesmentIndicators.Description4 == null ? string.Empty : assesmentIndicators.Description4;
                    assesmentIndicators.Description5 = assesmentIndicators.Description5 == null ? string.Empty : assesmentIndicators.Description5;
                    assesmentIndicators.Description6 = assesmentIndicators.Description6 == null ? string.Empty : assesmentIndicators.Description6;
                    assesmentIndicators.Description7 = assesmentIndicators.Description7 == null ? string.Empty : assesmentIndicators.Description7;

                    assesmentIndicatorsRepository.InsertOrUpdate(assesmentIndicators);
                    assesmentIndicatorsRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        assesmentIndicators.SuccessMessage = "Assesment Indicator has been added successfully";
                    }
                    else
                    {
                        assesmentIndicators.SuccessMessage = "Assesment Indicator has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    assesmentIndicators.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    assesmentIndicators.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        assesmentIndicators.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (assesmentIndicators.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (assesmentIndicators.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, assesmentIndicators) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, assesmentIndicators) });
            }
        }

        /// <summary>
        /// delete AssesmentIndicator from database usign ajax operation
        /// </summary>
        /// <param name="id">AssesmentIndicator id</param>
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
            //find the AssesmentIndicator in database
            AssesmentIndicators assesmentIndicators = assesmentIndicatorsRepository.Find(id);
            if (assesmentIndicators == null)
            {
                //set error message if it does not exist in database
                assesmentIndicators = new AssesmentIndicators();
                assesmentIndicators.ErrorMessage = "Assesment Indicator not found";
            }
            else
            {
                try
                {
                    //delete AssesmentIndicator from database
                    assesmentIndicatorsRepository.Delete(assesmentIndicators);
                    assesmentIndicatorsRepository.Save();
                    //set success message
                    assesmentIndicators.SuccessMessage = "Assesment Indicator has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    assesmentIndicators.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    assesmentIndicators.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (assesmentIndicators.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, assesmentIndicators) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, assesmentIndicators) });
            }
        }
    }
}