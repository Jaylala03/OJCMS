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
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseSupportCircleController : CaseBaseController
    {
        private readonly ICaseSupportCircleRepository casesupportcircleRepository;
        public CaseSupportCircleController(IWorkerRepository workerRepository, 
            ICaseRepository caseRepository,
            ICaseSupportCircleRepository casesupportcircleRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.casesupportcircleRepository = casesupportcircleRepository;
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseID)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            //FilterDescriptor newDesc = new FilterDescriptor("CaseID", FilterOperator.IsEqualTo, caseId);
            //dsRequest.Filters.Add(newDesc);
            DataSourceResult result = casesupportcircleRepository.AllIncluding().Where(item=>item.CaseID==caseID).OrderBy(item=>item.LastUpdateDate).Select(casesupportcircle => new { casesupportcircle.ID, 
                casesupportcircle.CreatedByWorkerID, casesupportcircle.CaseID, casesupportcircle.Institution, casesupportcircle.Resource, 
                casesupportcircle.Relationship, casesupportcircle.ContactInformation, casesupportcircle.Comments }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens casesupportcircle editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">casesupportcircle id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string caseId)
        {
            CaseSupportCircle casesupportcircle = null;
            if (id > 0)
            {
                //find an existing casesupportcircle from database
                casesupportcircle = casesupportcircleRepository.Find(id);
                if (casesupportcircle == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Support Circle not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                casesupportcircle = new CaseSupportCircle();
                casesupportcircle.CaseID = caseId.ToInteger(true);
            }

            ViewBag.PossibleCreatedByWorkers = workerRepository.All;
            ViewBag.PossibleLastUpdatedByWorkers = workerRepository.All;
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, casesupportcircle));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="casesupportcircle">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseSupportCircle casesupportcircle)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = casesupportcircle.ID == 0;
           
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    if (casesupportcircle.CreatedByWorkerID != 0)
                        if (!isNew && casesupportcircle.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                        {
                            WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                            return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                            //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                        }
                    casesupportcircle.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    casesupportcircleRepository.InsertOrUpdate(casesupportcircle);
                    casesupportcircleRepository.Save();
                    if (isNew)
                    {
                        casesupportcircle.SuccessMessage = "Case Support Circle has been added successfully";
                    }
                    else
                    {
                        casesupportcircle.SuccessMessage = "Case Support Circle has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    casesupportcircle.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    casesupportcircle.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        casesupportcircle.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (casesupportcircle.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            if (casesupportcircle.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesupportcircle) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesupportcircle) });
            }
        }

        /// <summary>
        /// delete casesupportcircle from database usign ajax operation
        /// </summary>
        /// <param name="id">casesupportcircle id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the casesupportcircle in database
            CaseSupportCircle casesupportcircle = casesupportcircleRepository.Find(id);
            if (casesupportcircle == null)
            {
                //set error message if it does not exist in database
                casesupportcircle = new CaseSupportCircle();
                casesupportcircle.ErrorMessage = "CaseSupportCircle not found";
            }
            else
            {
                try
                {
                    if (casesupportcircle.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    }
                    //delete casesupportcircle from database
                    casesupportcircleRepository.Delete(casesupportcircle);
                    casesupportcircleRepository.Save();
                    //set success message
                    casesupportcircle.SuccessMessage = "Case Support Circle has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    casesupportcircle.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        casesupportcircle.SuccessMessage = "Case Support Circle has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        casesupportcircle.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (casesupportcircle.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesupportcircle) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, casesupportcircle) });
            }
        }

       
    }
}

