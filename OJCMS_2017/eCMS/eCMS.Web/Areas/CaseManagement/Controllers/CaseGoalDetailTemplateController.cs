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
    public class CaseGoalDetailTemplateController : CaseBaseController
    {
        private readonly ICaseGoalDetailTemplateRepository CaseGoalDetailTemplateRepository;
        private readonly IIndicatorTypeRepository indicatorTypeRepository;
        
        public CaseGoalDetailTemplateController(ICaseGoalDetailTemplateRepository CaseGoalDetailTemplateRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository, 
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {            
            this.CaseGoalDetailTemplateRepository = CaseGoalDetailTemplateRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
        }

        /// <summary>
        /// This action returns the list of CaseGoalDetailTemplate
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseGoalDetailTemplate">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseGoalDetailTemplate CaseGoalDetailTemplate = new CaseGoalDetailTemplate();
            return View(CaseGoalDetailTemplate);
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
            DataSourceResult result = CaseGoalDetailTemplateRepository.All.Select(CaseGoalDetailTemplate => new { CaseGoalDetailTemplate.ID, IndicatorTypeName = CaseGoalDetailTemplate.IndicatorType.Name, CaseGoalDetailTemplate.CreateDate, CaseGoalDetailTemplate.LastUpdateDate, CaseGoalDetailTemplate.IndicatorTypeID, CaseGoalDetailTemplate.Name, CaseGoalDetailTemplate.Description }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadIndicatorTypeListAjax()
        {
            IQueryable<IndicatorType> indicatorList;

            indicatorList = indicatorTypeRepository.GetAll();

            return Json(indicatorList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens CaseGoalDetailTemplate editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">CaseGoalDetailTemplate id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            CaseGoalDetailTemplate CaseGoalDetailTemplate = null;
            if (id > 0)
            {
                //find an existing CaseGoalDetailTemplate from database
                CaseGoalDetailTemplate = CaseGoalDetailTemplateRepository.Find(id);
                if (CaseGoalDetailTemplate == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Template not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                CaseGoalDetailTemplate = new CaseGoalDetailTemplate();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, CaseGoalDetailTemplate));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="CaseGoalDetailTemplate">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseGoalDetailTemplate CaseGoalDetailTemplate)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = CaseGoalDetailTemplate.ID == 0;
            CaseGoalDetailTemplate.CreatedByWorkerID = CurrentLoggedInWorker.ID;
            CaseGoalDetailTemplate.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            //validate data
            if (ModelState.IsValid)
            {
                try
                {
                    //call repository function to save the data in database
                    CaseGoalDetailTemplate.Name = CaseGoalDetailTemplate.Name == null ? string.Empty : CaseGoalDetailTemplate.Name;
                    CaseGoalDetailTemplate.Description = CaseGoalDetailTemplate.Description == null ? string.Empty : CaseGoalDetailTemplate.Description;

                    CaseGoalDetailTemplateRepository.InsertOrUpdate(CaseGoalDetailTemplate);
                    CaseGoalDetailTemplateRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        CaseGoalDetailTemplate.SuccessMessage = "Template added successfully";
                    }
                    else
                    {
                        CaseGoalDetailTemplate.SuccessMessage = "Template modified successfully";
                    }
                }
                catch (CustomException ex)
                {
                    CaseGoalDetailTemplate.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    CaseGoalDetailTemplate.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        CaseGoalDetailTemplate.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (CaseGoalDetailTemplate.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (CaseGoalDetailTemplate.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, CaseGoalDetailTemplate) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, CaseGoalDetailTemplate) });
            }
        }

        /// <summary>
        /// delete CaseGoalDetailTemplate from database usign ajax operation
        /// </summary>
        /// <param name="id">CaseGoalDetailTemplate id</param>
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
            //find the CaseGoalDetailTemplate in database
            CaseGoalDetailTemplate CaseGoalDetailTemplate = CaseGoalDetailTemplateRepository.Find(id);
            if (CaseGoalDetailTemplate == null)
            {
                //set error message if it does not exist in database
                CaseGoalDetailTemplate = new CaseGoalDetailTemplate();
                CaseGoalDetailTemplate.ErrorMessage = "Template not found";
            }
            else
            {
                try
                {
                    //delete CaseGoalDetailTemplate from database
                    CaseGoalDetailTemplateRepository.Delete(CaseGoalDetailTemplate);
                    CaseGoalDetailTemplateRepository.Save();
                    //set success message
                    CaseGoalDetailTemplate.SuccessMessage = "Template has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    CaseGoalDetailTemplate.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    CaseGoalDetailTemplate.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (CaseGoalDetailTemplate.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, CaseGoalDetailTemplate) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, CaseGoalDetailTemplate) });
            }
        }
    }
}