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
    public class QualityOfLifeCategoryController : BaseController
    {
        public QualityOfLifeCategoryController(IQualityOfLifeCategoryRepository qualityoflifecategoryRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.qualityoflifecategoryRepository = qualityoflifecategoryRepository;
        }

        /// <summary>
        /// This action returns the list of QualityOfLifeCategory
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchQualityOfLifeCategory">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of qualityoflifecategory
            QualityOfLifeCategory qualityoflifecategory = new QualityOfLifeCategory();
            //return view result
            return View(qualityoflifecategory);
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
            DataSourceResult result = qualityoflifecategoryRepository.All.Select(qualityoflifecategory => new { qualityoflifecategory.ID, qualityoflifecategory.CreateDate, qualityoflifecategory.LastUpdateDate, qualityoflifecategory.Name, qualityoflifecategory.Description, qualityoflifecategory.IsActive }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens qualityoflifecategory editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">qualityoflifecategory id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            QualityOfLifeCategory qualityoflifecategory = null;
            if (id > 0)
            {
                //find an existing qualityoflifecategory from database
                qualityoflifecategory = qualityoflifecategoryRepository.Find(id);
                if (qualityoflifecategory == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "QualityOfLifeCategory not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                qualityoflifecategory = new QualityOfLifeCategory();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, qualityoflifecategory));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="qualityoflifecategory">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(QualityOfLifeCategory qualityoflifecategory)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = qualityoflifecategory.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    qualityoflifecategoryRepository.InsertOrUpdate(qualityoflifecategory);
                    qualityoflifecategoryRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        qualityoflifecategory.SuccessMessage = "QualityOfLifeCategory has been added successfully";
                    }
                    else
                    {
                        qualityoflifecategory.SuccessMessage = "QualityOfLifeCategory has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    qualityoflifecategory.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    qualityoflifecategory.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        qualityoflifecategory.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (qualityoflifecategory.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (qualityoflifecategory.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = qualityoflifecategory.ErrorMessage });
            }
            else
            {
                return Json(new { success = true, data = qualityoflifecategory.SuccessMessage });
            }
        }

        /// <summary>
        /// delete qualityoflifecategory from database usign ajax operation
        /// </summary>
        /// <param name="id">qualityoflifecategory id</param>
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
            //find the qualityoflifecategory in database
            QualityOfLifeCategory qualityoflifecategory = qualityoflifecategoryRepository.Find(id);
            if (qualityoflifecategory == null)
            {
                //set error message if it does not exist in database
                qualityoflifecategory = new QualityOfLifeCategory();
                qualityoflifecategory.ErrorMessage = "QualityOfLifeCategory not found";
            }
            else
            {
                try
                {
                    //delete qualityoflifecategory from database
                    qualityoflifecategoryRepository.Delete(qualityoflifecategory);
                    qualityoflifecategoryRepository.Save();
                    //set success message
                    qualityoflifecategory.SuccessMessage = "QualityOfLifeCategory has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    qualityoflifecategory.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    qualityoflifecategory.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (qualityoflifecategory.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflifecategory) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflifecategory) });
            }
        }

    }
}

