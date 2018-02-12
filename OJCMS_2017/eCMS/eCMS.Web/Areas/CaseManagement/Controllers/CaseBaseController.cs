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
using eCMS.Shared;
using eCMS.Web.Controllers;
using System.Web.Mvc;
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseBaseController : BaseController
    {
        protected readonly ICaseRepository caseRepository;
        public CaseBaseController(IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            ,ICaseRepository caseRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.caseRepository = caseRepository;
        }
        public CaseBaseController(IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
           , ICaseRepository caseRepository, IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(workerroleactionpermissionRepository, workerroleactionpermissionnewRepository)
        {
            this.caseRepository = caseRepository;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object objCurrentActionName = string.Empty;
            this.RouteData.Values.TryGetValue("action", out objCurrentActionName);
            string currentActionName = objCurrentActionName.ToString(true).ToLower();

            ViewBag.HasAccessToAssignmentModule = false;
            ViewBag.HasAccessToIndividualModule = false;
            ViewBag.HasAccessToInitialContactModule = false;
            ViewBag.HasAccessToCaseAuditLogModule = true;

            int caseId = 0;
            currentActionName = currentActionName.ToLower();
            if (!currentActionName.Contains("ajax") && !currentActionName.Contains("icon") && !currentActionName.Contains("logo") && !currentActionName.Contains("photo") && !currentActionName.Contains("uploadfile") && !currentActionName.Contains("removefile"))
            {
                if (filterContext.ActionParameters != null && filterContext.ActionParameters.Count > 0)
                {
                    if (filterContext.ActionParameters.ContainsKey("caseid"))
                    {
                        caseId = filterContext.ActionParameters["caseid"].ToInteger(true);
                    }
                }
                if (caseId == 0)
                {
                    caseId = Request.QueryString["caseid"].ToInteger(true);
                }
                ViewBag.CaseID = caseId;
                if (Request.QueryString["casememberid"].IsNotNullOrEmpty())
                {
                    ViewBag.CaseMemberID = Request.QueryString["casememberid"].ToInteger(true);
                }
                else
                {
                    ViewBag.CaseMemberID = 0;
                }
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = "" });
                }
                if (!currentActionName.Contains("ajax"))
                {
                    if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                    {
                        Case varCase = caseRepository.Find(caseId);
                        if (varCase != null)
                        {
                            ViewBag.HasAccessToAssignmentModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorker, Constants.Actions.Index, true);
                            ViewBag.HasAccessToIndividualModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Index, true);
                            ViewBag.HasAccessToInitialContactModule = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.InitialContact, true);
                        }
                    }
                    else
                    {
                        ViewBag.HasAccessToAssignmentModule = true;
                        ViewBag.HasAccessToIndividualModule = true;
                        ViewBag.HasAccessToInitialContactModule = true;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
