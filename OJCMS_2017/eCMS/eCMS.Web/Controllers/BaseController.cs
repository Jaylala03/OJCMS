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
using eCMS.DataLogic.ViewModels;
using eCMS.Shared;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCMS.Web.Controllers
{
    public class BaseController : Controller
    {
        //protected IWorkerProgramRepository workerprogramRepository;
        protected IWorkerSubProgramRepository workersubprogramRepository;
        //protected IWorkerRegionRepository workerregionRepository;
        protected IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        protected IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        protected ISubProgramRepository subprogramRepository;
        protected IProgramRepository programRepository;
        protected IJamatkhanaRepository jamatkhanaRepository;
        protected IIntakeMethodRepository intakemethodRepository;
        protected IReferralSourceRepository referralsourceRepository;
        protected IHearingSourceRepository hearingsourceRepository;
        protected ICaseStatusRepository casestatusRepository;
        protected IRegionRepository regionRepository;

        protected IRegionRoleRepository regionroleRepository;
        protected IRegionSubProgramRepository regionsubprogramRepository;

        protected IWorkerRepository workerRepository;
        protected IWorkerRoleRepository workerroleRepository;

        protected ICountryRepository countryRepository;
        protected ICurrencyRepository currencyRepository;
        protected IStateRepository stateRepository;
        protected IGenderRepository genderRepository;
        protected IActivityTypeRepository activitytypeRepository;
        protected ITimeSpentRepository timespentRepository;
        protected IContactMethodRepository contactmethodRepository;
        protected IRelationshipStatusRepository relationshipStatusRepository;
        protected ILanguageRepository languageRepository;
        protected IEthnicityRepository ethnicityRepository;
        protected ICaseMemberEthinicity caseEthinicityRepository;
        protected IMaritalStatusRepository maritalStatusRepository;
        protected IContactMediaRepository contactmediaRepository;
        protected ICaseMemberRepository casememberRepository;
        protected ICaseProgressNoteMembersRepository caseProgressNoteMembersRepository;
        protected ICaseWorkerRepository caseworkerRepository;
        protected IWorkerRolePermissionRepository workerRolePermissionRepository;
        protected IWorkerRolePermissionNewRepository workerRolePermissionNewRepository;
        protected IWorkerInRoleRepository workerinroleRepository;
        protected IWorkerInRoleNewRepository workerinrolenewRepository;
        protected IProfileTypeRepository profiletypeRepository;
        protected IHighestLevelOfEducationRepository highestlevelofeducationRepository;
        protected IGPARepository gpaRepository;
        protected IAnnualIncomeRepository annualincomeRepository;
        protected ISavingsRepository savingsRepository;
        protected IHousingQualityRepository housingqualityRepository;
        protected IImmigrationCitizenshipStatusRepository immigrationcitizenshipstatusRepository;
        protected ICaseMemberProfileRepository casememberprofileRepository;

        protected IAssessmentTypeRepository assessmenttypeRepository;
        protected IMemberStatusRepository memberstatusRepository;
        protected IRiskTypeRepository risktypeRepository;
        protected IReasonsForDischargeRepository reasonsfordischargeRepository;
        protected IQualityOfLifeRepository qualityoflifeRepository;
        protected IQualityOfLifeSubCategoryRepository qualityoflifesubcategoryRepository;
        protected IQualityOfLifeCategoryRepository qualityoflifecategoryRepository;
        protected ICaseAuditLogRepository caseAuditLogRepository;
        protected ICaseTrainingRepository caseTrainingRepository;

        protected IPermissionRepository permissionRepository;
        protected IPermissionRegionRepository permissionregionRepository;
        protected IPermissionSubProgramRepository permissionsubprogamRepository;
        protected IPermissionJamatkhanaRepository permissionjamatkhanaRepository;
        protected IPermissionActionRepository permissionactionRepository;
        protected IActionMethodRepository actionMethodRepository;
        protected IWorkerRolePermissionNewRepository wokerRolePermissionNewRepository;

        public BaseController(IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
        {
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
        }
        public BaseController(IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository, IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
        {
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
        }
        /// <summary>
        /// The user detail who is currently logged in, store at the time when user logs in.
        /// </summary>
        public Worker CurrentLoggedInWorker
        {
            get
            {
                Worker user = WebHelper.CurrentSession.Content.LoggedInWorker;
                if (user == null)
                {
                    if (HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                    {
                        CookieHelper newCookieHelper = new CookieHelper();
                        int userId = newCookieHelper.GetWorkerDataFromLoginCookie().ToInteger(true);
                        if (userId > 0)
                        {
                            if (workerRepository == null)
                            {
                                workerRepository = DependencyResolver.Current.GetService(typeof(IWorkerRepository)) as IWorkerRepository;
                            }
                            user = workerRepository.Find(userId);


                            WebHelper.CurrentSession.Content.LoggedInWorker = user;
                        }
                    }
                    if (user == null)
                    {
                        user = new Worker();
                    }
                }
                return user;
            }
        }

        //public String CurrentLoggedInWorkerRoleIDs
        //{
        //    get
        //    {
        //        String roleIDs = WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs;

        //        if (roleIDs.IsNullOrEmpty())
        //        {
        //            if (workerinroleRepository == null)
        //            {
        //                workerinroleRepository = DependencyResolver.Current.GetService(typeof(IWorkerInRoleRepository)) as IWorkerInRoleRepository;
        //            }

        //            roleIDs = string.Empty;

        //            List<WorkerInRole> workerRoles = workerinroleRepository.FindAllActiveByWorkerID(CurrentLoggedInWorker.ID);

        //            if (workerRoles != null)
        //            {
        //                foreach (WorkerInRole workerRole in workerRoles)
        //                {
        //                    if (!roleIDs.Contains(workerRole.WorkerRoleID.ToString()))
        //                    {
        //                        roleIDs = roleIDs.Concate(',', workerRole.WorkerRoleID.ToString());
        //                    }
        //                }
        //            }

        //            WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs = roleIDs;
        //        }

        //        return roleIDs;
        //    }
        //}

        //<JL:Edit:06/01/2017>
        public List<int> CurrentLoggedInWorkerRoleIDs
        {
            get
            {
                List<int> roleIDs = WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs;

                if (roleIDs == null)
                {
                    if (workerinrolenewRepository == null)
                    {
                        workerinrolenewRepository = DependencyResolver.Current.GetService(typeof(IWorkerInRoleNewRepository)) as IWorkerInRoleNewRepository;
                    }

                    roleIDs = new List<int>();

                    List<WorkerInRoleNew> workerRoles = workerinrolenewRepository.FindAllActiveByWorkerID(CurrentLoggedInWorker.ID);

                    if (workerRoles != null)
                    {
                        foreach (WorkerInRoleNew workerRole in workerRoles)
                        {
                            //if (roleIDs == null)
                            //{
                            //    roleIDs.Add(workerRole.WorkerRoleID);
                            //}
                            //else 
                            if (roleIDs.IndexOf(workerRole.WorkerRoleID) == -1)
                            {
                                roleIDs.Add(workerRole.WorkerRoleID);
                            }
                        }
                    }

                    WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs = roleIDs;
                }

                return roleIDs;
            }
        }
        //</JL:Edit:06/01/2017>

        //<JL:Edit:06/02/2017>
        //public String CurrentLoggedInWorkerRegionIDs
        //{
        //    get
        //    {
        //        String regionIDs = WebHelper.CurrentSession.Content.LoggedInWorkerRegionIDs;
        //        if (regionIDs.IsNullOrEmpty())
        //        {
        //            if (workerinroleRepository == null)
        //            {
        //                workerinroleRepository = DependencyResolver.Current.GetService(typeof(IWorkerInRoleRepository)) as IWorkerInRoleRepository;
        //            }
        //            regionIDs = string.Empty;
        //            List<WorkerInRole> workerRoles = workerinroleRepository.FindAllActiveByWorkerID(CurrentLoggedInWorker.ID);
        //            if (workerRoles != null)
        //            {
        //                foreach (WorkerInRole workerRole in workerRoles)
        //                {
        //                    regionIDs = regionIDs.Concate(',', workerRole.RegionID.ToString());
        //                }
        //            }
        //            WebHelper.CurrentSession.Content.LoggedInWorkerRegionIDs = regionIDs;
        //        }
        //        return regionIDs;
        //    }
        //}

        public List<int> CurrentLoggedInWorkerRegionIDs
        {
            get
            {
                List<int> regionIDs = WebHelper.CurrentSession.Content.LoggedInWorkerRegionIDs;
                if (regionIDs == null)
                {
                    if (workerinrolenewRepository == null)
                    {
                        workerinrolenewRepository = DependencyResolver.Current.GetService(typeof(IWorkerInRoleNewRepository)) as IWorkerInRoleNewRepository;
                    }
                    regionIDs = null;
                    regionIDs = workerinrolenewRepository.FindAllActiveRegionByWorkerID();
                    //List<WorkerInRoleNew> workerRoles = workerinrolenewRepository.FindAllActiveByWorkerID(CurrentLoggedInWorker.ID);
                    //if (workerRoles != null)
                    //{
                    //    foreach (WorkerInRoleNew workerRole in workerRoles)
                    //    {
                    //        regionIDs = regionIDs.Concate(',', workerRole.RegionID.ToString());
                    //    }
                    //}
                    WebHelper.CurrentSession.Content.LoggedInWorkerRegionIDs = regionIDs;
                }
                return regionIDs;
            }
        }
        //<JL:Edit:06/02/2017>

        public bool IsRegionalAdministrator
        {
            get
            {
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1)
                {
                    return true;
                }
                return false;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object objCurrentControllerName = string.Empty;
            this.RouteData.Values.TryGetValue("controller", out objCurrentControllerName);
            object objCurrentActionName = string.Empty;
            this.RouteData.Values.TryGetValue("action", out objCurrentActionName);
            object currentAreaName = string.Empty;
            this.RouteData.Values.TryGetValue("Areas", out currentAreaName);
            if (this.RouteData.DataTokens.ContainsKey("area"))
            {
                currentAreaName = this.RouteData.DataTokens["area"].ToString();
            }
            string currentActionName = objCurrentActionName.ToString(true);
            string currentControllerName = objCurrentControllerName.ToString(true);
            ViewBag.HasAccessToOtherConfigurationData = false;
            ViewBag.CurrentActionName = currentActionName;
            ViewBag.CurrentControllerName = currentControllerName;
            ViewBag.CurrentAreaName = currentAreaName;

            ViewBag.HasAccessToWorkerModule = false;
            ViewBag.HasAccessToReportModule = false;
            ViewBag.HasAccessToAdminModule = false;
            ViewBag.HasAccessToCaseManagementModule = true;
            ViewBag.IsRegionalAdministrator = false;

            if (CurrentLoggedInWorker != null)
            {
                ViewBag.CurrentWorkerID = CurrentLoggedInWorker.ID;
                //ViewBag.CurrentWorkerRoleID = CurrentLoggedInWorker.UserRoleID;
                ViewBag.CurrentWorkerName = CurrentLoggedInWorker.FirstName + " " + CurrentLoggedInWorker.LastName;
            }
            currentActionName = currentActionName.ToLower();
            if (!currentActionName.Contains("ajax"))
            {
                //<JL:Comment:06/13/2017>
                //if (WebHelper.CurrentSession.Content.RegionVisibility == VisibilityStatus.UnDefined && workerRolePermissionRepository != null)
                if (WebHelper.CurrentSession.Content.RegionVisibility == VisibilityStatus.UnDefined && workerRolePermissionNewRepository != null)
                {
                    VisibilityStatus regionVisiblity = VisibilityStatus.UnDefined;
                    VisibilityStatus programVisiblity = VisibilityStatus.UnDefined;
                    VisibilityStatus subProgramVisiblity = VisibilityStatus.UnDefined;
                    VisibilityStatus caseVisiblity = VisibilityStatus.UnDefined;
                    workerRolePermissionNewRepository.FindVisiblity(CurrentLoggedInWorker.ID, ref regionVisiblity, ref programVisiblity, ref subProgramVisiblity, ref caseVisiblity);
                    WebHelper.CurrentSession.Content.RegionVisibility = regionVisiblity;
                    WebHelper.CurrentSession.Content.ProgramVisibility = programVisiblity;
                    WebHelper.CurrentSession.Content.SubProgramVisibility = subProgramVisiblity;
                    WebHelper.CurrentSession.Content.CaseVisibility = caseVisiblity;
                }
            }

            ViewBag.CurrentLoggedInWorkerRoleIDs = CurrentLoggedInWorkerRoleIDs;
            
            if (CurrentLoggedInWorkerRoleIDs != null && CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
            {
                ViewBag.HasAccessToWorkerModule = true;
                ViewBag.HasAccessToReportModule = true;
                ViewBag.HasAccessToAdminModule = true;
            }
            
            //<JL:Add:06/11/2017>
            if(workerroleactionpermissionnewRepository != null)
            { 
                bool HasWorkerMenuPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.WorkerManagement, Constants.Controllers.Worker, string.Empty, true);
                if (HasWorkerMenuPermission)
                {
                     ViewBag.HasAccessToWorkerModule = true; 
                }
                bool HasReportMenuPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.Reporting, Constants.Controllers.Report, string.Empty, true);
                if (HasReportMenuPermission)
                {
                    ViewBag.HasAccessToReportModule = true;
                }
            }
            //</JL:Add:06/11/2017>

            if (CurrentLoggedInWorkerRoleIDs != null && CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1)
            {
                ViewBag.HasAccessToWorkerModule = true;
                ViewBag.HasAccessToReportModule = true;
                ViewBag.HasAccessToAdminModule = false;
                ViewBag.HasAccessToCaseManagementModule = false;
                ViewBag.IsRegionalAdministrator = true;
                ViewBag.HasAccessToOtherConfigurationData = true;
            }

            base.OnActionExecuting(filterContext);
        }

        #region Load DropDownList Asynchronously

        /// <summary>
        /// Load Worker DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadWorkerListAjax(int caseID)
        {
            var worker = workerRepository.FindAllForAssignmentByCaseID(caseID);//.Select(item => new { ID = item.ID, Name = item.FirstName + " " + item.LastName });
            return Json(worker, JsonRequestBehavior.AllowGet);
        }

        public int GetPrimaryWorkerOfTheCase(int caseID)
        {
            int workerId = 0;
            var worker = workerRepository.FindByPrimaryAndCaseID(caseID);
            if (worker != null)
            {
                workerId = worker.WorkerID;
            }
            return workerId;
        }
        /// <summary>
        /// Load Worker role  DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadWorkerRoleAjax()
        {
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
            {
                return Json(workerroleRepository.AllActiveForDropDownList.Where(item => item.Value != "1"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                //return Json(workerroleRepository.AllActiveForDropDownList.Where(item => item.Value != "1" && item.Value != SiteConfigurationReader.RegionalAdministratorRoleID.ToString() && item.Value != SiteConfigurationReader.RegionalManagerRoleID.ToString()), JsonRequestBehavior.AllowGet);
                return Json(workerroleRepository.GetWorkerRoleByWorkerID(), JsonRequestBehavior.AllowGet);
            }
        }        /// <summary>
        /// Load Permission DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadPermissionAjax()
        {
            return Json(permissionRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Load Program DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadProgramAjax()
        {
            if (IsRegionalAdministrator)
            {
                //return Json(programRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
                return Json(programRepository.NewFindAllByWorkerID(CurrentLoggedInWorker.ID).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(programRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadRegionAjax()
        {
            if (IsRegionalAdministrator)
            {
                //return Json(regionRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID, 0).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
                return Json(regionRepository.NewFindAllByWorkerID(CurrentLoggedInWorker.ID, 0).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(regionRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadRegionWithAllOptionAjax()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "All Regions", Value = "-1" });

            if (IsRegionalAdministrator)
            {
                List<DropDownViewModel> listvm = new List<DropDownViewModel>();
                listvm = regionRepository.NewFindAllByWorkerID(CurrentLoggedInWorker.ID, 0);

                foreach (DropDownViewModel item in listvm)
                {
                    list.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
                }
                //return Json(regionRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID, 0).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
                //return Json(regionRepository.NewFindAllByWorkerID(CurrentLoggedInWorker.ID, 0).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SelectListItem> listvm = new List<SelectListItem>();
                listvm = regionRepository.AllActiveForDropDownList;
                foreach (SelectListItem item in listvm)
                {
                    list.Add(new SelectListItem() { Text = item.Text, Value = item.Value });
                }
                //return Json(regionRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadProfileTypeAjax()
        {
            return Json(profiletypeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadHighestLevelOfEducationAjax()
        {
            return Json(highestlevelofeducationRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadGPAAjax()
        {
            return Json(gpaRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadAnnualIncomeAjax()
        {
            return Json(annualincomeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSavingsAjax()
        {
            return Json(savingsRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadHousingQualityAjax()
        {
            return Json(housingqualityRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadImmigrationCitizenshipStatusAjax()
        {
            return Json(immigrationcitizenshipstatusRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Sub-Program DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadSubProgramAjax(int programID)
        {
            return Json(subprogramRepository.FindAllForDropDownListByProgramID(programID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Worker DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadWorkerByRoleAjax(int roleID)
        {
            return Json(workerRepository.FindAllByRoleID(roleID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Case Worker DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadCaseWorkerListAjax(int caseID, int? caseMemberID, int? CaseMemberIds)
        {
            if (CaseMemberIds.HasValue && !caseMemberID.HasValue && CaseMemberIds.Value > 0)
            {
                caseMemberID = CaseMemberIds;
            }
            if (caseMemberID.HasValue && caseMemberID.Value > 0)
            {
                var caseWorker = workerRepository.FindAllByCaseMemberID(caseMemberID.Value);//.Select(item => new { ID = item.ID, Name = item.FirstName + " " + item.LastName });
                return Json(caseWorker, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var caseWorker = workerRepository.FindAllByCaseID(caseID);//.Select(item => new { ID = item.ID, Name = item.FirstName + " " + item.LastName });
                return Json(caseWorker, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Load Time Spent DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadTimeSpentListAjax()
        {
            return Json(timespentRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Case Member DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadCaseMemberListAjax(int caseID)
        {
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
            {
                var data = casememberRepository.FindAllByCaseIDForDropDownList(caseID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = casememberRepository.FindAllByCaseIDAndWorkerIDForDropDownList(caseID, CurrentLoggedInWorker.ID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Load Program DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadWorkerProgramsAjax()
        {
            if (programRepository == null)
            {
                programRepository = DependencyResolver.Current.GetService(typeof(IProgramRepository)) as IProgramRepository;
            }

            if (WebHelper.CurrentSession.Content.ProgramVisibility == VisibilityStatus.All)
            {
                return Json(programRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
            }
            else if (WebHelper.CurrentSession.Content.ProgramVisibility == VisibilityStatus.Assigned || WebHelper.CurrentSession.Content.ProgramVisibility == VisibilityStatus.UnDefined)
            {
                //return Json(programRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID).Where(item => item.IsActive == true).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
                return Json(programRepository.NewFindAllByWorkerID(CurrentLoggedInWorker.ID).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SelectListItem> items = new List<SelectListItem>();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Load Sub-Program DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadWorkerSubProgramsAjax(int? programID, int? regionID)
        {
            if (!programID.HasValue)
            {
                programID = 0;
            }
            if (!regionID.HasValue)
            {
                regionID = 0;
            }
            if (WebHelper.CurrentSession.Content.SubProgramVisibility == VisibilityStatus.All)
            {
                //var data = subprogramRepository.FindAllForDropDownListByProgramIDAndWorkerIDAndRegionID(programID.Value, 0, 0);
                var data = subprogramRepository.AllByRegionAndProgram(programID.Value, 0, 0);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (WebHelper.CurrentSession.Content.SubProgramVisibility == VisibilityStatus.Assigned || WebHelper.CurrentSession.Content.SubProgramVisibility == VisibilityStatus.UnDefined)
            {
                //var data=subprogramRepository.FindAllForDropDownListByProgramIDAndWorkerIDAndRegionID(programID.Value, CurrentLoggedInWorker.ID,regionID.Value);
                var data = subprogramRepository.AllByRegionAndProgram(programID.Value, CurrentLoggedInWorker.ID, regionID.Value);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SelectListItem> items = new List<SelectListItem>();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Load Region DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadWorkerRegionsAjax(int? programID)
        {
            if (!programID.HasValue)
            {
                programID = 0;
            }
            if (WebHelper.CurrentSession.Content.RegionVisibility == VisibilityStatus.All)
            {
                return Json(regionRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
            }
            else if (WebHelper.CurrentSession.Content.RegionVisibility == VisibilityStatus.Assigned || WebHelper.CurrentSession.Content.RegionVisibility == VisibilityStatus.UnDefined)
            {
                //var data = regionRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID, programID.Value).Where(item => item.IsActive == true).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
                var data = regionRepository.NewFindAllByWorkerID(CurrentLoggedInWorker.ID, programID.Value).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SelectListItem> items = new List<SelectListItem>();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }




        /// <summary>
        /// Load Case Status DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadCaseStatusAjax()
        {
            return Json(casestatusRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Worker Region Multiselect Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadWorkerRegionByWorkerRoleAjax(string workerRoleIDs)
        {
            if (regionroleRepository == null)
            {
                regionroleRepository = DependencyResolver.Current.GetService(typeof(IRegionRoleRepository)) as IRegionRoleRepository;
            }
            var result = regionroleRepository.FindAllByWorkerRoleIDs(workerRoleIDs).GroupBy(item => new { item.RegionID, item.Region.Name }).ToList().AsEnumerable().Select(o => new { ID = o.Key.RegionID, Name = o.Key.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadRegionByWorkerRoleIDAjax(int workerRoleID)
        {
            if (regionroleRepository == null)
            {
                regionroleRepository = DependencyResolver.Current.GetService(typeof(IRegionRoleRepository)) as IRegionRoleRepository;
            }
            var result = regionroleRepository.FindAllByWorkerRoleID(workerRoleID).GroupBy(item => new { item.RegionID, item.Region.Name }).ToList().AsEnumerable().Select(o => new { ID = o.Key.RegionID, Name = o.Key.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Worker Sub Program Multiselect Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadWorkerSubProgramByWorkerRoleAjax(string regionIDs, string programIDs)
        {
            if (regionsubprogramRepository == null)
            {
                regionsubprogramRepository = DependencyResolver.Current.GetService(typeof(IRegionSubProgramRepository)) as IRegionSubProgramRepository;
            }
            if (regionIDs.IsNullOrEmpty())
            {
                regionIDs = "0";
            }
            if (programIDs.IsNullOrEmpty())
            {
                programIDs = "0";
            }
            int[] arrayRegionIDs = regionIDs.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            int[] arrayProgramIDs = programIDs.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var result = regionsubprogramRepository.FindAllByRegionIDsAndProgramIDs(arrayRegionIDs, arrayProgramIDs).GroupBy(item => new { item.SubProgramID, item.SubProgram.Name }).ToList().AsEnumerable().Select(o => new { ID = o.Key.SubProgramID, Name = o.Key.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Worker Sub Program Multiselect Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadMultiSubProgramByPermission(string regionIDs, string programIDs)
        {
            if (subprogramRepository == null)
            {
                subprogramRepository = DependencyResolver.Current.GetService(typeof(ISubProgramRepository)) as ISubProgramRepository;
            }
            if (regionIDs.IsNullOrEmpty())
            {
                regionIDs = "0";
            }
            if (programIDs.IsNullOrEmpty())
            {
                programIDs = "0";
            }
            int[] arrayRegionIDs = regionIDs.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            int[] arrayProgramIDs = programIDs.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var result = subprogramRepository.AllByRegionIDsAndProgramIDs(arrayRegionIDs,  arrayProgramIDs);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Multi Region Multiselect Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadMultiRegionByPermission(string programIDs)
        {
            if (regionRepository == null)
            {
                regionRepository = DependencyResolver.Current.GetService(typeof(IRegionRepository)) as IRegionRepository;
            }
            if (programIDs.IsNullOrEmpty())
            {
                programIDs = "0";
            }
            int[] arrayProgramIDs = programIDs.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var result = regionRepository.FindAllByPrograms(CurrentLoggedInWorker.ID, arrayProgramIDs);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Jamatkhana DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadMultiJamatkhanasAjax(string programIDs,string regionIDs)
        {
            if (jamatkhanaRepository == null)
            {
                jamatkhanaRepository = DependencyResolver.Current.GetService(typeof(IJamatkhanaRepository)) as IJamatkhanaRepository;
            }
            //return Json(jamatkhanaRepository.FindAllByRegionID(regionID), JsonRequestBehavior.AllowGet);
            return Json(jamatkhanaRepository.FindAllByWorkerRegionIDs(programIDs,regionIDs), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadRegionBySubProgramIDAjax(int subProgramID)
        {
            if (regionsubprogramRepository == null)
            {
                regionsubprogramRepository = DependencyResolver.Current.GetService(typeof(IRegionSubProgramRepository)) as IRegionSubProgramRepository;
            }
            var result = regionsubprogramRepository.FindAllBySubProgramID(subProgramID).GroupBy(item => new { item.RegionID, item.Region.Name }).ToList().AsEnumerable().Select(o => new { ID = o.Key.RegionID, Name = o.Key.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Jamatkhana DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadJamatkhanasAjax(int regionID)
        {
            //return Json(jamatkhanaRepository.FindAllByRegionID(regionID), JsonRequestBehavior.AllowGet);
            return Json(jamatkhanaRepository.FindAllByWorkerRegionID(regionID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Intake Method DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadIntakeMethodsAjax()
        {
            return Json(intakemethodRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Referral Sources DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadReferralSourcesAjax()
        {
            return Json(referralsourceRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Hearing Sources DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadHearingSourcesAjax()
        {
            return Json(hearingsourceRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Member Status DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadMemberStatusAjax()
        {
            return Json(memberstatusRepository.AllActiveOpenForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Relationship Status DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadRelationshipStatusAjax()
        {
            return Json(relationshipStatusRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Language DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadLanguagesAjax()
        {
            return Json(languageRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Gender DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadGendersAjax()
        {
            return Json(genderRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Ethnicity DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadEthnicityAjax()
        {
            return Json(ethnicityRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Contact Media DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadContactMediaAjax()
        {
            return Json(contactmediaRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Marital Status DropDownList Asynchronously
        /// </summary>
        /// <returns>Data in JSON</returns>
        public JsonResult LoadMaritalStatusAjax()
        {
            return Json(maritalStatusRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadActivityTypeAjax()
        {
            return Json(activitytypeRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadContactMethodAjax()
        {
            return Json(contactmethodRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadQualityOfLifeCategoryAjax()
        {
            return Json(qualityoflifecategoryRepository.AllForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadQualityOfLifeSubCategoryAjax(int categoryId)
        {
            return Json(qualityoflifesubcategoryRepository.FindAllByQualityOfLifeCategoryID(categoryId).ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
