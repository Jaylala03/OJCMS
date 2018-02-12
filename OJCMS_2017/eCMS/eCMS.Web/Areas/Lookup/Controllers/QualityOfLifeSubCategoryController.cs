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
    public class QualityOfLifeSubCategoryController : BaseController
    {
        public QualityOfLifeSubCategoryController(IQualityOfLifeSubCategoryRepository qualityoflifesubcategoryRepository,
            IQualityOfLifeCategoryRepository qualityoflifecategoryRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.qualityoflifesubcategoryRepository = qualityoflifesubcategoryRepository;
            this.qualityoflifecategoryRepository = qualityoflifecategoryRepository;
        }

        /// <summary>
        /// This action returns the list of QualityOfLifeSubCategory
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchQualityOfLifeSubCategory">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            QualityOfLifeSubCategory qualityoflifesubcategory = new QualityOfLifeSubCategory();
            return View(qualityoflifesubcategory);
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
            DataSourceResult result = qualityoflifesubcategoryRepository.All.Select(qualityoflifesubcategory => new { qualityoflifesubcategory.ID, qualityoflifesubcategory.CreateDate, qualityoflifesubcategory.LastUpdateDate, qualityoflifesubcategory.Name, qualityoflifesubcategory.Description, qualityoflifesubcategory.IsActive,QualityOfLifeCategoryName=qualityoflifesubcategory.QualityOfLifeCategory.Name }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens qualityoflifesubcategory editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">qualityoflifesubcategory id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            QualityOfLifeSubCategory qualityoflifesubcategory = null;
            if (id > 0)
            {
                //find an existing qualityoflifesubcategory from database
                qualityoflifesubcategory = qualityoflifesubcategoryRepository.Find(id);
                if (qualityoflifesubcategory == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "QualityOfLifeSubCategory not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                qualityoflifesubcategory = new QualityOfLifeSubCategory();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, qualityoflifesubcategory));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="qualityoflifesubcategory">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(QualityOfLifeSubCategory qualityoflifesubcategory)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = qualityoflifesubcategory.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    qualityoflifesubcategoryRepository.InsertOrUpdate(qualityoflifesubcategory);
                    qualityoflifesubcategoryRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        qualityoflifesubcategory.SuccessMessage = "QualityOfLifeSubCategory has been added successfully";
                    }
                    else
                    {
                        qualityoflifesubcategory.SuccessMessage = "QualityOfLifeSubCategory has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    qualityoflifesubcategory.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    qualityoflifesubcategory.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        qualityoflifesubcategory.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (qualityoflifesubcategory.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (qualityoflifesubcategory.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflifesubcategory) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflifesubcategory) });
            }
        }

        /// <summary>
        /// delete qualityoflifesubcategory from database usign ajax operation
        /// </summary>
        /// <param name="id">qualityoflifesubcategory id</param>
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
            //find the qualityoflifesubcategory in database
            QualityOfLifeSubCategory qualityoflifesubcategory = qualityoflifesubcategoryRepository.Find(id);
            if (qualityoflifesubcategory == null)
            {
                //set error message if it does not exist in database
                qualityoflifesubcategory = new QualityOfLifeSubCategory();
                qualityoflifesubcategory.ErrorMessage = "QualityOfLifeSubCategory not found";
            }
            else
            {
                try
                {
                    //delete qualityoflifesubcategory from database
                    qualityoflifesubcategoryRepository.Delete(qualityoflifesubcategory);
                    qualityoflifesubcategoryRepository.Save();
                    //set success message
                    qualityoflifesubcategory.SuccessMessage = "QualityOfLifeSubCategory has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    qualityoflifesubcategory.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    qualityoflifesubcategory.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (qualityoflifesubcategory.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflifesubcategory) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflifesubcategory) });
            }
        }

    }
}

