using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories;
using eCMS.DataLogic.Models;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseTrainingController : CaseBaseController
    {

        public CaseTrainingController(IWorkerRepository workerRepository, 
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
             IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseRepository caseRepository,
            ICaseTrainingRepository trainingRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
             this.caseTrainingRepository = trainingRepository;
        }

        [WorkerAuthorize]
        // GET: CaseManagement/CaseTraining
        public ActionResult Index()
        {
            ViewBag.HasPermissionToAdd = false;
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
            {
                ViewBag.HasPermissionToAdd = true;
            }
            return View();
        }


        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the varCase in database
            BaseModel statusModel = new BaseModel();
            TrainingModule varTraining = caseTrainingRepository.Find(id);

            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
            {
                statusModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
            try
            {
                //delete varCase from database
                caseTrainingRepository.Delete(id);
                caseTrainingRepository.Save();



                //set success message
                statusModel.SuccessMessage = "Training Module has been deleted successfully";
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                {
                    statusModel.SuccessMessage = "Training Module has been deleted successfully";
                }
                else
                {
                    ExceptionManager.Manage(ex);
                    statusModel.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (!string.IsNullOrEmpty(statusModel.ErrorMessage))
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
        }

        [WorkerAuthorize]
        // GET: CaseManagement/CaseTraining
        public FileResult Download(string fileName,string filePath)
        {
            return File(filePath, "application/force-download", fileName);
        }

        public ActionResult AddModule()
        {
            CaseMember casemember = null;
            return Content(this.RenderPartialViewToString(Constants.PartialViews.AddModule, casemember));
        }
        [HttpPost]
        public ActionResult AddModule(FormCollection fc)
        {
            TrainingModule trainingModule = new TrainingModule();
            string fileLocation = fc["fileLocation"];
            try
            {
                var moduleType = fc["moduletype"];
                if (moduleType == "1")
                {
                    trainingModule.FileName = fc["fileName"];
                    trainingModule.FileType = Convert.ToInt32(moduleType);
                }
                else
                {
                    HttpPostedFileBase pfb = Request.Files["moduleFile"];
                    if (pfb != null && pfb.ContentLength > 0)
                    {
                        string uploadedfilename = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(pfb.FileName);
                        string filePath = System.IO.Path.Combine(HttpContext.Server.MapPath("~/Training/"),
                                           uploadedfilename);
                        pfb.SaveAs(filePath);
                        fileLocation = "/Training/" + uploadedfilename;
                    }
                    trainingModule.FileName = fc["fileName1"];
                    trainingModule.FileType = Convert.ToInt32(moduleType);
                }
                
                trainingModule.FileLocation = fileLocation;
                caseTrainingRepository.InsertOrUpdate(trainingModule);
                caseTrainingRepository.Save();
                trainingModule.SuccessMessage="Record saved successfully.";
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                trainingModule.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return RedirectToAction("Index");
        
        }

        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult ModuleAjax(TrainingModule trainingModule, [DataSourceRequest] DataSourceRequest dsRequest)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            var result = caseTrainingRepository.Search(dsRequest);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}