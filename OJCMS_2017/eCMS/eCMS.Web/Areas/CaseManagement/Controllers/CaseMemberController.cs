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
    public class CaseMemberController : CaseBaseController
    {
        public CaseMemberController(IWorkerRepository workerRepository, 
            ICaseRepository caseRepository, 
            IRelationshipStatusRepository relationshipstatusRepository, 
            ILanguageRepository languageRepository, 
            IGenderRepository genderRepository, 
            IEthnicityRepository ethnicityRepository,
            ICaseMemberEthinicity caseEthinicityRepository,
            IMaritalStatusRepository maritalstatusRepository,
            IMemberStatusRepository memberstatusRepository,
            ICaseMemberRepository casememberRepository,
            ICaseWorkerRepository caseworkerRepository,
           IWorkerRoleRepository workerRoleRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository
           )
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.relationshipStatusRepository = relationshipstatusRepository;
            this.languageRepository = languageRepository;
            this.genderRepository = genderRepository;
            this.ethnicityRepository = ethnicityRepository;
            this.caseEthinicityRepository = caseEthinicityRepository;
            this.maritalStatusRepository = maritalstatusRepository;
            this.memberstatusRepository = memberstatusRepository;
            this.casememberRepository = casememberRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.workerroleRepository = workerRoleRepository;
        }

        /// <summary>
        /// This action returns the list of CaseMember
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseMember">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, CaseMember searchCaseMember, int caseID)
        {            
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseMember caseMember = new CaseMember();
            if (varCase != null)
            {
                caseMember.CaseID = caseID;
                //caseMember.FirstName = varCase.FirstName;
                //caseMember.LastName = varCase.LastName;
                //caseMember.MemberStatusID = varCase.CaseStatusID;
                //caseMember.EnrollDate = varCase.EnrollDate;
                ViewBag.DisplayID = varCase.DisplayID;
            }
            else
            {
                caseMember.EnrollDate = DateTime.Now;
                caseMember.MemberStatusID = 1;
            }
            return View(caseMember);
        }

        [WorkerAuthorize]
        public ActionResult IndexRead(int caseID)
        {
            var varCase = caseRepository.Find(caseID);
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            CaseMember caseMember = new CaseMember();
            if (varCase != null)
            {
                caseMember.CaseID = caseID;
                ViewBag.DisplayID = varCase.DisplayID;
            }
            else
            {
                caseMember.EnrollDate = DateTime.Now;
                caseMember.MemberStatusID = 1;
            }
            return View(caseMember);
        }

        /// <summary>
        /// This action creates new casemember
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int caseID)
        {
            //create a new instance of casemember
            var varCase = caseRepository.Find(caseID);
            if (varCase == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case not found");
            }
            CaseMember caseMember = new CaseMember();
            caseMember.CaseID = caseID;
            ViewBag.DisplayID = varCase.DisplayID;
            caseMember.EnrollDate = DateTime.Now;
            caseMember.MemberStatusID = 1;
            caseMember.IsActive = true;
            return View(caseMember);
        }

        /// <summary>
        /// This action saves new casemember to database
        /// </summary>
        /// <param name="casemember">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(CaseMember caseMember, int caseId)
        {
            if (caseMember.CaseID == 0 && caseId > 0)
            {
                caseMember.CaseID = caseId;
            }
            caseMember.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    casememberRepository.InsertOrUpdate(caseMember);
                    casememberRepository.Save();
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseMember.CaseID });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            caseMember.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (caseMember.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                caseMember.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                caseMember.ErrorMessage = Constants.Messages.UnhandelledError;
            }               
            //return view with error message if operation is failed
            return View(caseMember);
        }

        /// <summary>
        /// This action edits an existing casemember
        /// </summary>
        /// <param name="id">casemember id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id)
        {
            CaseMember casemember = casememberRepository.Find(id);
            return View(casemember);
        }

        [WorkerAuthorize]
        public ActionResult Read(int id, int caseID)
        {
            //find the existing varCase from database
            Case varCase = caseRepository.Find(caseID);
            if (varCase == null)
            {
                return RedirectToAction(Constants.Actions.Create, Constants.Controllers.Case, new { area = Constants.Areas.CaseManagement });
            }

            ViewBag.HasAccessToAssignmentModule = false;
            ViewBag.HasAccessToIndividualModule = false;
            ViewBag.HasAccessToInitialContactModule = false;
            ViewBag.HasAccessToCaseAuditLogModule = true;
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
            {
                ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
            }
            else
            {
                ViewBag.HasAccessToAssignmentModule = true;
                ViewBag.HasAccessToIndividualModule = true;
                ViewBag.HasAccessToInitialContactModule = true;
            }

            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(caseID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Read, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            CaseMember casemember = casememberRepository.Find(id);
            return View(casemember);
        }

        /// <summary>
        /// This action saves an existing casemember to database
        /// </summary>
        /// <param name="casemember">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(CaseMember casemember, int caseId)
        {
            if (casemember.CaseID == 0 && caseId > 0)
            {
                casemember.CaseID = caseId;
            }
            casemember.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (casemember.ID > 0 && casemember.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    //return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}
                    //call the repository function to save in database
                    casememberRepository.InsertOrUpdate(casemember);
                    casememberRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { caseId = caseId });
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            casemember.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (casemember.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                casemember.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                casemember.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            
            //return view with error message if the operation is failed
            return View(casemember);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }

            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Delete, true);
            bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Read, true);
            bool IsUserAdminWorker = CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1;
            bool IsUserRegionalManager = workerroleRepository.IsWorkerRegionalAdmin() > 0 ? true:false ;//CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1;

            FilterDescriptor newDesc = new FilterDescriptor("CaseID", FilterOperator.IsEqualTo, caseId);
            dsRequest.Filters.Add(newDesc);
            
            var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseId);
            
            List<CaseWorker> caseworker = caseworkerRepository.FindAllByCaseID(caseId).Where(x => x.WorkerID == CurrentLoggedInWorker.ID).ToList();
            
            DataSourceResult result = casememberRepository.AllIncluding(caseId.ToInteger(true), casemember => casemember.CreatedByWorker,
                casemember => casemember.LastUpdatedByWorker, casemember => casemember.Case, casemember => casemember.RelationshipStatus,
                casemember => casemember.Language, casemember => casemember.Gender, casemember => casemember.Ethnicity,
                casemember => casemember.MaritalStatus, casemember => casemember.MemberStatus).OrderBy(item => item.LastUpdateDate).AsEnumerable().Select(casemember =>
                    new CaseMember()
                    {
                        ID=casemember.ID,
                        CreateDate=casemember.CreateDate,
                        LastUpdateDate=casemember.LastUpdateDate,
                        CreatedByWorkerID=casemember.CreatedByWorkerID,
                        CreatedByWorkerName = casemember.CreatedByWorker.FirstName + " " + casemember.CreatedByWorker.LastName,
                        LastUpdatedByWorkerID=casemember.LastUpdatedByWorkerID,
                        LastUpdatedByWorkerName = casemember.LastUpdatedByWorker.FirstName + " " + casemember.LastUpdatedByWorker.LastName,
                        IsArchived=casemember.IsArchived,
                        CaseID=casemember.CaseID,
                        FirstName=casemember.FirstName,
                        LastName=casemember.LastName,
                        EnrollDate=casemember.EnrollDate,
                        RelationshipStatusID=casemember.RelationshipStatusID,
                        RelationshipStatusName = casemember.RelationshipStatus!=null ? casemember.RelationshipStatus.Name : String.Empty,
                        LanguageID=casemember.LanguageID,
                        LanguageName = casemember.Language!=null ? casemember.Language.Name : String.Empty,
                        DateOfBirth=casemember.DateOfBirth,
                        GenderID=casemember.GenderID,
                        GenderName = casemember.Gender!=null ? casemember.Gender.Name : String.Empty,
                        EthnicityID=casemember.EthnicityID,
                        EthnicityName = casemember.Ethnicity != null ? casemember.Ethnicity.Name : caseEthinicityRepository.FindEthnicityNames(casemember.ID),
                        MaritalStatusID=casemember.MaritalStatusID,
                        MaritalStatusName = casemember.MaritalStatus!=null ? casemember.MaritalStatus.Name : String.Empty,
                        MemberStatusID=casemember.MemberStatusID,
                        MemberStatusName = casemember.MemberStatus!=null ? casemember.MemberStatus.Name : String.Empty,
                        IsPrimary=casemember.IsPrimary,
                        HasPermissionToEdit = (((caseworker == null || caseworker.Count() == 0) && casemember.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && !IsUserAdminWorker && !IsUserRegionalManager) ? "display:none;" : String.Empty),
                        //HasPermissionToEdit = (((caseworker != null && caseworker.Count() > 0) 
                        //&& casemember.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID 
                        //&& IsUserNotAdminWorker && IsUserNotRegionalManager) ? "display:none;" : String.Empty),
                        HasPermissionToRead = hasReadPermission ? "" : "display:none;",
                        HasPermissionToDelete = hasDeletePermission ? "" : "display:none;"
                        //HasPermissionToEdit = casemember.MemberStatus!=null ? casemember.MemberStatus.Name.Contains("Closed") ? "display:none;" : String.Empty : String.Empty
                    }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action displays the details of an existing casemember on popup
        /// </summary>
        /// <param name="id">casemember id</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult DetailsAjax(int id)
        {
            CaseMember casemember = casememberRepository.Find(id);
            if (casemember == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Member not found");
            }
            return Content(this.RenderPartialViewToString(Constants.PartialViews.Details, casemember));
        }

        /// <summary>
        /// This action opens casemember editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">casemember id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string caseId)
        {
            CaseMember casemember = null;
            if (id > 0)
            {
                //find an existing casemember from database
                Case varCase = caseRepository.Find(Convert.ToInt32(caseId));
                bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(Convert.ToInt32(caseId), CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Edit, true);
                if (!hasAccess)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                }

                casemember = casememberRepository.Find(id);
                if (casemember == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Member not found");
                }
                else
                {
                    string Ethnicity=caseEthinicityRepository.FindEthnicity(id);
                    if (!string.IsNullOrEmpty(Ethnicity))
                    {
                        casemember.CaseEthinicity = Ethnicity.Split(',');
                    }
                    else
                    {
                        casemember.CaseEthinicity=new string[] {casemember.EthnicityID.ToString()};
                    }
                    
                }
            }
            else
            {
                //create a new instance if id is not provided
                casemember = new CaseMember();
                casemember.CaseID = caseId.ToInteger(true);
                casemember.IsActive = true;
            }        
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, casemember));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="casemember">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
       // [UpdateRoleAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseMember casemember, int caseId = 0, bool isredirect = false)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = casemember.ID == 0;
            if (casemember.CaseID == 0 && caseId > 0)
            {
                casemember.CaseID = caseId;
            }
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    if (casemember.MemberStatusID == null || casemember.MemberStatusID == 0)
                    {
                        casemember.ErrorMessage = "Please select Status";
                    }
                    else
                    {
                        //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                        //if (!isNew)
                        //{
                        //    var primaryWorkerID = GetPrimaryWorkerOfTheCase(casemember.CaseID);
                        //    List<CaseWorker> caseworker = caseworkerRepository.FindAllByCaseID(caseId).Where(x => x.WorkerID == CurrentLoggedInWorker.ID).ToList();
                        //    if ((caseworker == null || caseworker.Count() == 0) && casemember.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID!=CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                        //    {
                        //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        //        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                        //        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                        //    }
                        //}
                        //</JL:Comment:07/08/2017>

                        //set the id of the worker who has added/updated this record
                        casemember.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                        //call repository function to save the data in database
                        casememberRepository.InsertOrUpdate(casemember);
                        casememberRepository.Save();
                        if (casemember.CaseEthinicity != null && casemember.CaseEthinicity.Length > 0)
                        {
                            CaseMemberEthinicity ethinicityModel = new CaseMemberEthinicity();
                            ethinicityModel.CaseID = casemember.ID;
                            string EthinicityIds = string.Join(",", casemember.CaseEthinicity);
                            if (!String.IsNullOrEmpty(EthinicityIds))
                            {
                                ethinicityModel.EthinicityID = EthinicityIds;
                                ethinicityModel.CreateDate = DateTime.Now;
                                caseEthinicityRepository.InsertOrUpdate(ethinicityModel);
                                caseEthinicityRepository.Save();
                            }
                        }
                        //set status message
                        if (isNew)
                        {
                            casemember.SuccessMessage = "Case individual has been added successfully";
                        }
                        else
                        {
                            casemember.SuccessMessage = "Case individual has been updated successfully";
                        }
                    }
                }
                catch (CustomException ex)
                {
                    casemember.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    casemember.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        casemember.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (casemember.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (casemember.ErrorMessage.IsNotNullOrEmpty())
            {
              
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemember) });
            }
            else
            {
                string[] BlankArray = { };
                casemember.CaseEthinicity = BlankArray;
                if (isredirect)
                {
                    return Json(new { success = true, url = Url.Action(Constants.Actions.InitialContact, Constants.Controllers.CaseProgressNote, new { caseID = casemember.CaseID }) });
                }
                else
                {
                    return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemember) });
                }
            }
        }

        /// <summary>
        /// delete casemember from database usign ajax operation
        /// </summary>
        /// <param name="id">casemember id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
           
            //find the varCase in database
            BaseModel statusModel = new BaseModel();
            try
            {
                bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Delete, true);
                if (!hasAccess)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                }

                CaseMember caseMember = casememberRepository.Find(id);
                //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                //var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseMember.CaseID);
                //if (caseMember.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                //{
                //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                //    return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                //    //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                //}
                //</JL:Comment:07/08/2017>
                //delete varCase from database
                casememberRepository.Delete(id);
                casememberRepository.Save();
                //Audit Log
             

                //set success message
                statusModel.SuccessMessage = "Case individual has been deleted successfully";
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                {
                    statusModel.SuccessMessage = "Case individual has been deleted successfully";
                }
                else
                {
                    ExceptionManager.Manage(ex);
                    statusModel.ErrorMessage = Constants.Messages.UnhandelledError;
                }
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

        public ActionResult GetDisplayIDAjax(int id)
        {
            CaseMember caseMember = casememberRepository.Find(id);
            if (caseMember != null)
            {
                return Json(new { data = caseMember.DisplayID }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}

