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
using eCMS.DataLogic.ViewModels;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using eCMS.Web.Controllers;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Data.Entity.Infrastructure;
namespace eCMS.Web.Areas.WorkerManagement.Controllers
{
    public class PermissionController : BaseController
    {
        public PermissionController(IPermissionRepository permissionRepository,
            IProgramRepository programRepository,
            ISubProgramRepository subProgramRepository,
            IJamatkhanaRepository jamatkhanaRepository,
            IRegionRepository regionRepository,
            IActionMethodRepository actionMethodRepository,
            IPermissionRegionRepository permissionregionRepository,
            IPermissionSubProgramRepository permissionsubprogamRepository,
            IPermissionJamatkhanaRepository permissionjamatkhanaRepository,
            IPermissionActionRepository permissionactionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.permissionRepository = permissionRepository;
            this.programRepository = programRepository;
            this.subprogramRepository = subProgramRepository;
            this.jamatkhanaRepository = jamatkhanaRepository;
            this.regionRepository = regionRepository;
            this.actionMethodRepository = actionMethodRepository;
            this.permissionregionRepository = permissionregionRepository;
            this.permissionsubprogamRepository = permissionsubprogamRepository;
            this.permissionjamatkhanaRepository = permissionjamatkhanaRepository;
            this.permissionactionRepository = permissionactionRepository;
        }

        /// <summary>
        /// This action returns the list of Permission
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchWorker">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToWorkerModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            var permission = new Permission();
            return View(permission);
        }

        /// <summary>
        /// This action creates new worker
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create()
        {
            if (!ViewBag.HasAccessToWorkerModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of permission
            Permission permission = new Permission();
            permission.PermissionRegion = new PermissionRegion();

            if (IsRegionalAdministrator)
            {
                permission.AllActionMethods = actionMethodRepository.All.Where(item => item.CreatedByWorkerID == CurrentLoggedInWorker.ID).ToList();
            }
            else
            {
                permission.AllActionMethods = actionMethodRepository.All.ToList();
            }

            //return view result
            return View(permission);
        }

        /// <summary>
        /// This action saves new permission to database
        /// </summary>
        /// <param name="permission">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(Permission permission)
        {
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    permissionRepository.InsertOrUpdate(permission);
                    permissionRepository.Save();
                    if (permission.ID > 0)
                    {
                        //redirect to edit page after successful operation
                        return RedirectToAction(Constants.Actions.Edit, new { Id = permission.ID });
                    }
                }
                else
                {
                    if (IsRegionalAdministrator)
                    {
                        permission.AllActionMethods = actionMethodRepository.All.Where(item => item.CreatedByWorkerID == CurrentLoggedInWorker.ID).ToList();
                    }
                    else
                    {
                        permission.AllActionMethods = actionMethodRepository.All.ToList();
                    }
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            permission.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (permission.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                permission.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                permission.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if operation is failed
            return View(permission);
        }

        /// <summary>
        /// This action edits an existing permission
        /// </summary>
        /// <param name="id">permission id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id)
        {
            if (!ViewBag.HasAccessToWorkerModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //find the existing permission from database
            Permission permission = permissionRepository.Find(id);

            permission.PermissionRegion = new PermissionRegion();
            permission.PermissionRegion.PermissionID = id;

            if (IsRegionalAdministrator)
            {
                //permission.PermissionRegion.AllSubPrograms = permissionsubprogamRepository.FindAllSubProgramByWordkerID(CurrentLoggedInWorker.ID, 0).ToList();
                //permission.PermissionRegion.AllJamatkhanas = permissionjamatkhanaRepository.FindAllJamatkhanaByWordkerID(CurrentLoggedInWorker.ID, 0).ToList();
                permission.AllActionMethods = actionMethodRepository.All.Where(item => item.CreatedByWorkerID == CurrentLoggedInWorker.ID).ToList();
            }
            else
            {
                //permission.PermissionRegion.AllSubPrograms = subprogramRepository.All.Where(item => item.IsActive == true).ToList();
                //permission.PermissionRegion.AllJamatkhanas = jamatkhanaRepository.All.Where(item => item.IsActive == true).ToList();
                permission.AllActionMethods = actionMethodRepository.All.ToList();
            }
            permission.AssignedPermissionActions = actionMethodRepository.FindAllByPermissionID(id).ToList();
            //ViewBag.Programs = programRepository.All.AsEnumerable().Select(o => new { ID = o.ID, Name = o.Name }).ToList();
            //ViewBag.SubPrograms = subprogramRepository.All.AsEnumerable().Select(o => new { ID = o.ID, Name = o.Name }).ToList();
            //ViewBag.Regions = regionRepository.All.AsEnumerable().Select(o => new { ID = o.ID, Name = o.Name }).ToList();
            //return editor view
            return View(permission);
        }

        /// <summary>
        /// This action saves an existing permission to database
        /// </summary>
        /// <param name="permission">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(Permission permission)
        {
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    permissionRepository.InsertOrUpdate(permission);
                    permissionRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index);
                }
                else
                {
                    if (IsRegionalAdministrator)
                    {
                        permission.AllActionMethods = actionMethodRepository.All.Where(item => item.CreatedByWorkerID == CurrentLoggedInWorker.ID).ToList();
                    }
                    else
                    {
                        permission.AllActionMethods = actionMethodRepository.All.ToList();
                    }
                    permission.AssignedPermissionActions = actionMethodRepository.FindAllByPermissionID(permission.ID).ToList();
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            permission.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (permission.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                permission.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                permission.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if the operation is failed
            return View(permission);
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

            DataSourceResult result = permissionRepository.All.Select(permission => new PermissionListViewModel()
            {
                ID = permission.ID,
                CreateDate = permission.CreateDate,
                LastUpdateDate = permission.LastUpdateDate,
                Name = permission.Name,
                Description = permission.Description,
                IsActive = permission.IsActive
            }).ToDataSourceResult(dsRequest);
            //DataSourceResult result = permissionRepository.All.ToDataSourceResult(dsRequest);

            foreach (PermissionListViewModel o in result.Data)
            {
                o.PermissionRegionList = permissionregionRepository.FindAllByPermissionID(o.ID).AsEnumerable().Select(item => new PermissionRegion() { ID = item.ID, ProgramName = item.Program.Name, RegionName = item.Region.Name }).ToList();
                if (o.PermissionRegionList != null)
                {
                    foreach (PermissionRegion region in o.PermissionRegionList)
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
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action displays the details of an existing permission on popup
        /// </summary>
        /// <param name="id">permission id</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult DetailsAjax(int id)
        {
            Permission permission = permissionRepository.Find(id);
            if (permission == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Permission not found");
            }
            return Content(this.RenderPartialViewToString(Constants.PartialViews.Details, permission));
        }

        /// <summary>
        /// This action opens permission editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">permission id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            Permission permission = null;
            if (id > 0)
            {
                //find an existing permission from database
                permission = permissionRepository.Find(id);
                if (permission == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Permission not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                permission = new Permission();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.EditorPopUp, permission));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="permission">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(Permission permission)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = permission.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    permissionRepository.InsertOrUpdate(permission);
                    permissionRepository.Save();
                    //set status message
                    if (permission.ErrorMessage == null)
                    {
                        if (isNew)
                        {
                            permission.SuccessMessage = "Permission has been added successfully";
                        }
                        else
                        {
                            permission.SuccessMessage = "Permission has been updated successfully";
                        }
                    }
                }
                catch (CustomException ex)
                {
                    permission.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    permission.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        permission.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (permission.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (permission.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, permission) });
            }
            else
            {
                return Json(new { success = true, permissionid = permission.ID, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, permission) });
            }
        }

        /// <summary>
        /// delete permission from database usign ajax operation
        /// </summary>
        /// <param name="id">permission id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        public ActionResult DeleteAjax(int id)
        {
            Permission permission = new Permission();
            if (!ViewBag.HasAccessToWorkerModule)
            {
                permission.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, permission) }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                //delete permission from database
                permissionRepository.Delete(id);
                permissionRepository.Save();
                //set success message
                permission.SuccessMessage = "Permission has been deleted successfully";
            }
            catch (CustomException ex)
            {
                permission.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (DbUpdateException ex)
            {
                permission.ErrorMessage = "There is a problem deleting data to database";
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                permission.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return action status in json to display on a message bar
            if (permission.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, permission) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, permission) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadSubProgramEditorAjax(int programID, int permissionID)
        {
            PermissionRegion permissionregion = new PermissionRegion();
            if (IsRegionalAdministrator)
            {
                //permissionregion.AllSubPrograms = subprogramRepository.FindAllByWordkerID(CurrentLoggedInWorker.ID, programID).ToList();
                permissionregion.AllSubPrograms = permissionsubprogamRepository.FindAllSubProgramByWordkerID(CurrentLoggedInWorker.ID, programID).ToList();
            }
            else
            {
                permissionregion.AllSubPrograms = subprogramRepository.FindAllByProgramID(programID).ToList();
            }

            return Content(this.RenderPartialViewToString("~/Areas/WorkerManagement/Views/Permission/_SubProgram.cshtml", permissionregion));
        }

        public ActionResult LoadJamatkhanaEditorAjax(int regionID, int permissionID)
        {
            PermissionRegion permissionregion = new PermissionRegion();
            if (IsRegionalAdministrator)
            {
                permissionregion.AllJamatkhanas = permissionjamatkhanaRepository.FindAllJamatkhanaByWordkerID(CurrentLoggedInWorker.ID, regionID).ToList();
            }
            else
            {
                permissionregion.AllJamatkhanas = jamatkhanaRepository.FindAllJamatkhanaByRegionID(regionID).ToList();
            }

            return Content(this.RenderPartialViewToString("~/Areas/WorkerManagement/Views/Permission/_Jamatkhana.cshtml", permissionregion));
        }
    }
}

