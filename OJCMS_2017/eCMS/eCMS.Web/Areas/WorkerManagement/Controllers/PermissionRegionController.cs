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
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Linq;
using System.Data.Entity.Infrastructure;
using eCMS.Web.Controllers;

namespace eCMS.Web.Areas.WorkerManagement.Controllers
{
    public class PermissionRegionController : BaseController
    {
        public PermissionRegionController(
            IPermissionRegionRepository PermissionRegionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IPermissionSubProgramRepository PermissionSubProgramRepository,
            IPermissionJamatkhanaRepository permissionjamatkhanaRepository,
            ISubProgramRepository subprogramRepository,
            IJamatkhanaRepository jamatkhanaRepository,
            IProgramRepository programRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.permissionregionRepository = PermissionRegionRepository;
            this.permissionsubprogamRepository = PermissionSubProgramRepository;
            this.permissionjamatkhanaRepository = permissionjamatkhanaRepository;
            this.subprogramRepository = subprogramRepository;
            this.jamatkhanaRepository = jamatkhanaRepository;
            this.programRepository = programRepository;
        }

        /// <summary>
        /// This action returns the list of PermissionRegion
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchPermissionRegion">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ViewResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, PermissionRegion searchPermissionRegion, int workerId)
        {

            //if(searchPermissionRegion!=null)
            //{
            //	if (dsRequest.Filters == null || (dsRequest.Filters != null && dsRequest.Filters.Count == 0))
            //	{
            //		if (dsRequest.Filters == null)
            //		{
            //			dsRequest.Filters = new List<IFilterDescriptor>();
            //		}
            //	}
            //}
            //DataSourceRequest dsRequestTotalCountQuery = new DataSourceRequest();
            //dsRequestTotalCountQuery.Filters = dsRequest.Filters;
            //DataSourceResult totalCountQuery = PermissionRegionRepository.All.ToDataSourceResult(dsRequestTotalCountQuery);
            //ViewData["total"] = totalCountQuery.Data.AsQueryable().Count();
            //if (dsRequest.PageSize == 0)
            //{
            //    dsRequest.PageSize = Constants.CommonConstants.DefaultPageSize;
            //}
            //DataSourceResult result = PermissionRegionRepository.AllIncluding(workerId,PermissionRegion => PermissionRegion.CreatedByWorker, PermissionRegion => PermissionRegion.LastUpdatedByWorker, PermissionRegion => PermissionRegion.Worker, PermissionRegion => PermissionRegion.WorkerRole).ToDataSourceResult(dsRequest);
            //return View(result.Data);
            return View();
        }

        /// <summary>
        /// This action creates new PermissionRegion
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int permissionId)
        {
            //create a new instance of PermissionRegion
            PermissionRegion PermissionRegion = new PermissionRegion();
            PermissionRegion.PermissionID = permissionId;
            //return view result
            return View(PermissionRegion);
        }

        /// <summary>
        /// This action saves new PermissionRegion to database
        /// </summary>
        /// <param name="PermissionRegion">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(PermissionRegion PermissionRegion, int workerId)
        {
            PermissionRegion.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    permissionregionRepository.InsertOrUpdate(PermissionRegion);
                    permissionregionRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { workerId = workerId });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            PermissionRegion.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (PermissionRegion.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                PermissionRegion.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                PermissionRegion.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if operation is failed
            return View(PermissionRegion);
        }

        /// <summary>
        /// This action edits an existing PermissionRegion
        /// </summary>
        /// <param name="id">PermissionRegion id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id)
        {
            //find the existing PermissionRegion from database
            PermissionRegion PermissionRegion = permissionregionRepository.Find(id);
            //return editor view
            return View(PermissionRegion);
        }

        /// <summary>
        /// This action saves an existing PermissionRegion to database
        /// </summary>
        /// <param name="PermissionRegion">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(PermissionRegion PermissionRegion, int workerId)
        {
            PermissionRegion.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    permissionregionRepository.InsertOrUpdate(PermissionRegion);
                    permissionregionRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { workerId = workerId });
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            PermissionRegion.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (PermissionRegion.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                PermissionRegion.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                PermissionRegion.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if the operation is failed
            return View(PermissionRegion);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, string permissionId)
        {
            List<PermissionRegion> regionList = permissionregionRepository.FindAllByPermissionID(permissionId.ToInteger()).AsEnumerable().Select(item => new PermissionRegion() { ID = item.ID, ProgramName = item.Program.Name, RegionName = item.Region.Name }).ToList();

            if (regionList != null)
            {
                foreach (PermissionRegion region in regionList)
                {
                    List<PermissionSubProgram> subProgramList = permissionsubprogamRepository.FindAllByPermissionRegionID(region.ID);
                    
                    if (subProgramList != null)
                    {
                        foreach (PermissionSubProgram subProgram in subProgramList)
                        {
                            region.SubProgramNames = region.SubProgramNames.Concate(',', subProgram.SubProgram.Name);
                        }
                    }

                    List<PermissionJamatkhana> jamatkhanaList = permissionjamatkhanaRepository.FindAllByPermissionRegionID(region.ID);

                    if (jamatkhanaList != null)
                    {
                        foreach (PermissionJamatkhana jamatkhana in jamatkhanaList)
                        {
                            region.JamatkhanaNames = region.JamatkhanaNames.Concate(',', jamatkhana.Jamatkhana.Name);
                        }
                    }
                }
            }
            DataSourceResult result = regionList.ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action displays the details of an existing PermissionRegion on popup
        /// </summary>
        /// <param name="id">PermissionRegion id</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult DetailsAjax(int id)
        {
            PermissionRegion PermissionRegion = permissionregionRepository.Find(id);
            if (PermissionRegion == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Region not found");
            }
            return Content(this.RenderPartialViewToString(Constants.PartialViews.Details, PermissionRegion));
        }

        /// <summary>
        /// This action opens PermissionRegion editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">PermissionRegion id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string permissionId)
        {
            PermissionRegion PermissionRegion = null;
            if (id > 0)
            {
                //find an existing PermissionRegion from database
                PermissionRegion = permissionregionRepository.Find(id);
                if (PermissionRegion == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Region not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                PermissionRegion = new PermissionRegion();
                PermissionRegion.PermissionID = permissionId.ToInteger(true);
            }
            if (IsRegionalAdministrator)
            {
                //PermissionRegion.AllSubPrograms = subprogramRepository.FindAllByWordkerID(CurrentLoggedInWorker.ID, 0).ToList();
                PermissionRegion.AllSubPrograms = permissionsubprogamRepository.FindAllSubProgramByWordkerID(CurrentLoggedInWorker.ID, PermissionRegion.ProgramID).ToList();
                PermissionRegion.AllJamatkhanas = permissionjamatkhanaRepository.FindAllJamatkhanaByWordkerID(CurrentLoggedInWorker.ID, PermissionRegion.RegionID).ToList();
            }
            else
            {
                PermissionRegion.AllSubPrograms = subprogramRepository.FindAllByProgramID(PermissionRegion.ProgramID).ToList();// subprogramRepository.All.Where(item => item.IsActive == true && item.ProgramID == PermissionRegion.ProgramID).ToList();
                PermissionRegion.AllJamatkhanas = jamatkhanaRepository.FindAllJamatkhanaByRegionID(PermissionRegion.RegionID).ToList(); //jamatkhanaRepository.All.Where(item => item.IsActive == true && item.RegionID == PermissionRegion.RegionID).ToList();
            }

            PermissionRegion.AssignedSubPrograms = permissionsubprogamRepository.All.Where(item => item.PermissionRegionID == id).ToList();
            PermissionRegion.AssignedJamatkhanas = permissionjamatkhanaRepository.All.Where(item => item.PermissionRegionID == id).ToList();

            //ViewBag.Programs = programRepository.All.AsEnumerable().Select(o => new { ID = o.ID, Name = o.Name }).ToList();
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString("~/Areas/WorkerManagement/Views/Permission/_PermissionRegion.cshtml", PermissionRegion));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="PermissionRegion">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(PermissionRegion PermissionRegion)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = PermissionRegion.ID == 0;
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    PermissionRegion.SubProgramIDs = Request.Form["SubProgramIDs"].ToString(true).ToStringArray(',');
                    if (PermissionRegion.SubProgramIDs == null || (PermissionRegion.SubProgramIDs!=null && PermissionRegion.SubProgramIDs.Length == 0))
                    {
                        throw new CustomException("Please select at least one sub-program");
                    }

                    //PermissionRegion.JamatkhanaIDs = Request.Form["JamatkhanaIDs"].ToString(true).ToStringArray(',');
                    //if (PermissionRegion.JamatkhanaIDs == null || (PermissionRegion.JamatkhanaIDs != null && PermissionRegion.JamatkhanaIDs.Length == 0))
                    //{
                    //    throw new CustomException("Please select at least one jamatkhana");
                    //}

                    //set the id of the permission who has added/updated this record
                    PermissionRegion.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    //call repository function to save the data in database
                    permissionregionRepository.InsertOrUpdate(PermissionRegion);
                    permissionregionRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        PermissionRegion.SuccessMessage = "Region has been added successfully";
                    }
                    else
                    {
                        PermissionRegion.SuccessMessage = "Region has been updated successfully";
                    }
                }
                catch (DbUpdateException ex)
                {
                    PermissionRegion.ErrorMessage = "There is a problem adding data to database";
                }
                catch (CustomException ex)
                {
                    PermissionRegion.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    PermissionRegion.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        PermissionRegion.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (PermissionRegion.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (PermissionRegion.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, PermissionRegion) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, PermissionRegion) });
            }
        }

        /// <summary>
        /// delete PermissionRegion from database usign ajax operation
        /// </summary>
        /// <param name="id">PermissionRegion id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        public ActionResult DeleteAjax(int id)
        {
            //find the PermissionRegion in database
            BaseModel statusModel = new BaseModel();
            try
            {
                //delete PermissionRegion from database
                permissionregionRepository.Delete(id);
                permissionregionRepository.Save();
                //set success message
                statusModel.SuccessMessage = "Region has been deleted successfully";
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                statusModel.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return action status in json to display on a message bar
            if (statusModel.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
        }

    }
}

