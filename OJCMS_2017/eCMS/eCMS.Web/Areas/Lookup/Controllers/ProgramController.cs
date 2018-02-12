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
    public class ProgramController : BaseController
    {
        public ProgramController(IProgramRepository programRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.programRepository = programRepository;
        }

        /// <summary>
        /// This action returns the list of Program
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchProgram">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of program
            Program program = new Program();
            //return view result
            return View(program);
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
            DataSourceResult result = programRepository.All.Select(program => new { program.ID, program.CreateDate, program.LastUpdateDate, program.Name, program.Description, program.IsActive }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens program editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">program id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            Program program = null;
            if (id > 0)
            {
                //find an existing program from database
                program = programRepository.Find(id);
                if (program == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Program not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                program = new Program();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, program));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="program">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(Program program)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = program.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    programRepository.InsertOrUpdate(program);
                    programRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        program.SuccessMessage = "Program has been added successfully";
                    }
                    else
                    {
                        program.SuccessMessage = "Program has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    program.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    program.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        program.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (program.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (program.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = program.ErrorMessage });
            }
            else
            {
                return Json(new { success = true, data = program.SuccessMessage });
            }
        }

        /// <summary>
        /// delete program from database usign ajax operation
        /// </summary>
        /// <param name="id">program id</param>
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
            //find the program in database
            Program program = programRepository.Find(id);
            if (program == null)
            {
                //set error message if it does not exist in database
                program = new Program();
                program.ErrorMessage = "Program not found";
            }
            else
            {
                try
                {
                    //delete program from database
                    programRepository.Delete(program);
                    programRepository.Save();
                    //set success message
                    program.SuccessMessage = "Program has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    program.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    program.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (program.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, program) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, program) });
            }
        }

    }
}

