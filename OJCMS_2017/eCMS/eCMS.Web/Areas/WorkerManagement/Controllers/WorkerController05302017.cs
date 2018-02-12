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
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace eCMS.Web.Areas.WorkerManagement.Controllers
{
    public class WorkerController05302017 : WorkerBaseController
    {
        public WorkerController05302017(IWorkerRepository workerRepository,
            IWorkerRoleRepository workerroleRepository,
            IProgramRepository programRepository,
            ISubProgramRepository subProgramRepository,
            IRegionRepository regionRepository,
            IWorkerInRoleRepository workerinroleRepository,
            IWorkerSubProgramRepository workersubprogramRepository,
            IRegionRoleRepository regionroleRepository,
            IRegionSubProgramRepository regionsubprogramRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.workerRepository = workerRepository;
            this.workerroleRepository = workerroleRepository;
            this.programRepository = programRepository;
            this.subprogramRepository = subProgramRepository;
            this.regionRepository = regionRepository;
            this.workerinroleRepository = workerinroleRepository;
            this.workersubprogramRepository = workersubprogramRepository;
            this.regionroleRepository = regionroleRepository;
            this.regionsubprogramRepository = regionsubprogramRepository;
        }

        /// <summary>
        /// This action returns the list of Worker
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchWorker">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, Worker searchWorker)
        {
            if (!ViewBag.HasAccessToWorkerModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            var worker = new Worker();
            return View(worker);
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
            //create a new instance of worker
            Worker worker = new Worker();
            worker.WorkerInRole = new WorkerInRole();
            worker.WorkerInRole.EffectiveFrom = DateTime.Today;
            worker.WorkerInRole.EffectiveTo = new DateTime(2080, 12, 30);
            worker.Password = "password";
            worker.ConfirmPassword = worker.Password;
            //return view result
            return View(worker);
        }

        /// <summary>
        /// This action saves new worker to database
        /// </summary>
        /// <param name="worker">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(Worker worker)
        {
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                  
                    worker.Password = CryptographyHelper.Encrypt(worker.Password);
                    worker.ConfirmPassword = worker.Password;
                    //call the repository function to save in database
                    workerRepository.InsertOrUpdate(worker, CurrentLoggedInWorker.ID);
                    workerRepository.Save();
                    if (worker.ID > 0)
                    {
                        //redirect to edit page after successful operation
                        return RedirectToAction(Constants.Actions.Edit, new { Id = worker.ID });
                    }
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            worker.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (worker.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                worker.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                worker.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if operation is failed
            return View(worker);
        }

        /// <summary>
        /// This action edits an existing worker
        /// </summary>
        /// <param name="id">worker id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id)
        {
            if (!ViewBag.HasAccessToWorkerModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //find the existing worker from database
            Worker worker = workerRepository.Find(id);
            try
            {
                worker.Password = CryptographyHelper.Decrypt(worker.Password);
            }
            catch { }
            worker.ConfirmPassword = worker.Password;
            worker.WorkerInRole = new WorkerInRole();
            worker.WorkerInRole.EffectiveFrom = DateTime.Today;
            worker.WorkerInRole.EffectiveTo = new DateTime(2080, 12, 30);
            worker.WorkerInRole.WorkerID = id;
            if (IsRegionalAdministrator)
            {
                worker.WorkerInRole.AllSubPrograms = subprogramRepository.FindAllByWordkerID(CurrentLoggedInWorker.ID,0).ToList();
            }
            else
            {
                worker.WorkerInRole.AllSubPrograms = subprogramRepository.All.Where(item => item.IsActive == true).ToList();
            }
            //ViewBag.Programs = programRepository.All.AsEnumerable().Select(o => new { ID = o.ID, Name = o.Name }).ToList();
            //ViewBag.SubPrograms = subprogramRepository.All.AsEnumerable().Select(o => new { ID = o.ID, Name = o.Name }).ToList();
            //ViewBag.Regions = regionRepository.All.AsEnumerable().Select(o => new { ID = o.ID, Name = o.Name }).ToList();
            //return editor view
            return View(worker);
        }

        /// <summary>
        /// This action saves an existing worker to database
        /// </summary>
        /// <param name="worker">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(Worker worker)
        {
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //if (worker.CreatedByworkerID != CurrentLoggedInWorker.ID)
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    worker.Password = CryptographyHelper.Encrypt(worker.Password);
                    worker.ConfirmPassword = worker.Password;
                    //call the repository function to save in database
                    workerRepository.InsertOrUpdate(worker, CurrentLoggedInWorker.ID);
                    workerRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index);
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            worker.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (worker.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                worker.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                worker.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return view with error message if the operation is failed
            return View(worker);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax(WorkerSearchViewModel searchViewModel, [DataSourceRequest] DataSourceRequest dsRequest)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            DataSourceResult result = workerRepository.Search(searchViewModel, dsRequest);
            foreach (WorkerListViewModel o in result.Data)
            {
                o.WorkerInRoleList = workerinroleRepository.FindAllByWorkerID(o.ID).AsEnumerable().Select(item => new WorkerInRole() { ID = item.ID, WorkerRoleName = item.WorkerRole.Name, ProgramName = item.Program.Name, RegionName = item.Region.Name, EffectiveFrom = item.EffectiveFrom, EffectiveTo = item.EffectiveTo }).ToList();
                if (o.WorkerInRoleList != null)
                {
                    foreach (WorkerInRole role in o.WorkerInRoleList)
                    {
                        List<WorkerSubProgram> subProgramList = workersubprogramRepository.FindAllByWorkerInRoleID(role.ID);
                        if (subProgramList != null)
                        {
                            foreach (WorkerSubProgram subProgram in subProgramList)
                            {
                                role.SubProgramNames = role.SubProgramNames.Concate(',', subProgram.SubProgram.Name);
                            }
                        }
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action displays the details of an existing worker on popup
        /// </summary>
        /// <param name="id">worker id</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult DetailsAjax(int id)
        {
            Worker worker = workerRepository.Find(id);
            if (worker == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Worker not found");
            }
            return Content(this.RenderPartialViewToString(Constants.PartialViews.Details, worker));
        }

        /// <summary>
        /// This action opens worker editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">worker id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            Worker worker = null;
            if (id > 0)
            {
                //find an existing worker from database
                worker = workerRepository.Find(id);
                if (worker == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Worker not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                worker = new Worker();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.EditorPopUp, worker));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="worker">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(Worker worker)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = worker.ID == 0;
         
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //if (!isNew)
                    //    if (worker.CreatedByworkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    worker.Password = CryptographyHelper.Encrypt(worker.Password);
                    worker.ConfirmPassword = worker.Password;
                    //call repository function to save the data in database
                    workerRepository.InsertOrUpdate(worker, CurrentLoggedInWorker.ID);
                    workerRepository.Save();
                    //set status message
                    if (worker.ErrorMessage == null)
                    {
                        if (isNew)
                        {
                            worker.SuccessMessage = "Worker has been added successfully";
                        }
                        else
                        {
                            worker.SuccessMessage = "Worker has been updated successfully";
                        }
                    }
                }
                catch (CustomException ex)
                {
                    worker.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    worker.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        worker.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (worker.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (worker.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, worker) });
            }
            else
            {
                return Json(new { success = true, workerid = worker.ID, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, worker) });
            }
        }

        /// <summary>
        /// delete worker from database usign ajax operation
        /// </summary>
        /// <param name="id">worker id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        public ActionResult DeleteAjax(int id)
        {
            Worker worker = new Worker();
            if (!ViewBag.HasAccessToWorkerModule)
            {
                worker.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, worker) }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                //delete worker from database
                workerRepository.Delete(id);
                workerRepository.Save();
                //set success message
                worker.SuccessMessage = "Worker has been deleted successfully";
            }
            catch (CustomException ex)
            {
                worker.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                worker.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return action status in json to display on a message bar
            if (worker.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, worker) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, worker) }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadWorkerStatusToFilterAjax()
        {
            var items = new List<CustomSelectedItems>();
            items.Add(new CustomSelectedItems() { ID = 1, Name = "Active" });
            items.Add(new CustomSelectedItems() { ID = 2, Name = "In Active" });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadWorkerSubProgramAjax(int? programID)
        {
            var items = new List<CustomSelectedItems>();
            items.Add(new CustomSelectedItems() { ID = 1, Name = "Active" });
            items.Add(new CustomSelectedItems() { ID = 2, Name = "In Active" });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadSubProgramEditorAjax(int programID, int workerID)
        {
            WorkerInRole workerInRole = new WorkerInRole();
            if (IsRegionalAdministrator)
            {
                workerInRole.AllSubPrograms = subprogramRepository.FindAllByWordkerID(CurrentLoggedInWorker.ID, programID).ToList();
            }
            else
            {
                workerInRole.AllSubPrograms = subprogramRepository.FindAllByProgramID(programID).ToList();
            }
            //workerInRole.AllSubPrograms = subprogramRepository.FindAllByProgramID(programID);
            //worker.AssignedSubPrograms = workersubprogramRepository.FindAllByWorkerID(workerID);
            return Content(this.RenderPartialViewToString("~/Areas/WorkerManagement/Views/Worker/_SubProgram.cshtml", workerInRole));
        }
    }
}

