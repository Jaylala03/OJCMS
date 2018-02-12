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
    public class QualityOfLifeController : BaseController
    {
        public QualityOfLifeController(IQualityOfLifeRepository qualityoflifeRepository,
            IQualityOfLifeSubCategoryRepository qualityoflifesubcategoryRepository,
            IQualityOfLifeCategoryRepository qualityoflifecategoryRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.qualityoflifeRepository = qualityoflifeRepository;
            this.qualityoflifecategoryRepository = qualityoflifecategoryRepository;
            this.qualityoflifesubcategoryRepository = qualityoflifesubcategoryRepository;
        }

        /// <summary>
        /// This action returns the list of QualityOfLife
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchQualityOfLife">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            QualityOfLife qualityoflife = new QualityOfLife();
            return View(qualityoflife);
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
            DataSourceResult result = qualityoflifeRepository.All.Select(qualityoflife => new { qualityoflife.ID, qualityoflife.CreateDate, qualityoflife.LastUpdateDate, qualityoflife.Name, qualityoflife.Description, qualityoflife.IsActive, QualityOfLifeCategoryName = qualityoflife.QualityOfLifeSubCategory.QualityOfLifeCategory.Name, QualityOfLifeSubCategoryName = qualityoflife.QualityOfLifeSubCategory.Name }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens qualityoflife editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">qualityoflife id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            QualityOfLife qualityoflife = null;
            if (id > 0)
            {
                //find an existing qualityoflife from database
                qualityoflife = qualityoflifeRepository.Find(id);
                if (qualityoflife == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Quality Of Life not found");
                }
                if (qualityoflife.QualityOfLifeSubCategory != null)
                {
                    qualityoflife.QualityOfLifeCategoryID = qualityoflife.QualityOfLifeSubCategory.QualityOfLifeCategoryID;
                }
            }
            else
            {
                //create a new instance if id is not provided
                qualityoflife = new QualityOfLife();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, qualityoflife));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="qualityoflife">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(QualityOfLife qualityoflife)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = qualityoflife.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    qualityoflifeRepository.InsertOrUpdate(qualityoflife);
                    qualityoflifeRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        qualityoflife.SuccessMessage = "Quality Of Life has been added successfully";
                    }
                    else
                    {
                        qualityoflife.SuccessMessage = "Quality Of Life has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    qualityoflife.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    qualityoflife.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        qualityoflife.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (qualityoflife.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (qualityoflife.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflife) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflife) });
            }
        }

        /// <summary>
        /// delete qualityoflife from database usign ajax operation
        /// </summary>
        /// <param name="id">qualityoflife id</param>
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
            //find the qualityoflife in database
            QualityOfLife qualityoflife = qualityoflifeRepository.Find(id);
            if (qualityoflife == null)
            {
                //set error message if it does not exist in database
                qualityoflife = new QualityOfLife();
                qualityoflife.ErrorMessage = "Quality Of Life not found";
            }
            else
            {
                try
                {
                    //delete qualityoflife from database
                    qualityoflifeRepository.Delete(qualityoflife);
                    qualityoflifeRepository.Save();
                    //set success message
                    qualityoflife.SuccessMessage = "Quality Of Life has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    qualityoflife.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    qualityoflife.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (qualityoflife.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflife) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, qualityoflife) });
            }
        }

    }
}

