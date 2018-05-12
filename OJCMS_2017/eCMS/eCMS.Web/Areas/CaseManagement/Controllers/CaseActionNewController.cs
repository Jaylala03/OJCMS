using eCMS.Shared;
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
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseActionNewController : CaseBaseController
    {
        private readonly ICaseGoalNewRepository caseGoalNewRepository;
        private readonly ICaseInitialAssessmentRepository caseInitialAssessmentRepository;
        private readonly IGoalActionWorkNoteRepository goalActionWorkNoteRepository;
        private readonly IGoalAssigneeRoleRepository goalassigneeroleRepository;
        private readonly ICaseActionNewRepository caseactionnewRepository;

        public CaseActionNewController(ICaseGoalNewRepository CaseGoalNewRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , ICaseInitialAssessmentRepository caseInitialAssessmentRepository, ICaseMemberRepository casememberRepository,
            ICaseWorkerNoteRepository caseWorkerNoteRepository, IGoalActionWorkNoteRepository goalActionWorkNoteRepository,
            IContactMediaRepository contactmediaRepository, IGoalStatusRepository goalstatusRepository,
            IGoalAssigneeRoleRepository goalassigneeroleRepository, ICaseActionNewRepository caseactionnewRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {
            this.caseGoalNewRepository = CaseGoalNewRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
            this.casememberRepository = casememberRepository;
            this.goalActionWorkNoteRepository = goalActionWorkNoteRepository;
            this.contactmediaRepository = contactmediaRepository;
            this.goalstatusRepository = goalstatusRepository;
            this.goalassigneeroleRepository = goalassigneeroleRepository;
            this.caseactionnewRepository = caseactionnewRepository;
        }

        [WorkerAuthorize]
        // GET: CaseManagement/CaseActionNew
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, Case searchCase, int caseId, int caseGoalId)
        {
            CaseGoalNew casegoalnew = null;
            if (caseGoalId > 0)
            {
                //find an existing casemember from database
                Case varCase = caseRepository.Find(Convert.ToInt32(caseId));
                ViewBag.DisplayID = varCase.DisplayID;
                ViewBag.CaseID = caseId;
                bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(Convert.ToInt32(caseId), CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, varCase.ProgramID, varCase.RegionID, varCase.SubProgramID, varCase.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Edit, true);
                if (!hasAccess)
                {
                    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                }

                casegoalnew = caseGoalNewRepository.Find(caseGoalId);
                if (casegoalnew == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Member not found");
                }
                else
                {
                }
            }
            else
            {
                //create a new instance if id is not provided
                casegoalnew = new CaseGoalNew();
                casegoalnew.CaseID = caseId.ToInteger(true);
            }

            ViewBag.ContactMediaList = contactmediaRepository.AllActiveForDropDownList;
            casegoalnew.GoalActionWorkNote = new GoalActionWorkNote();

            return View(casegoalnew);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, int caseGoalId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }

            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Delete, true);
            bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMember, Constants.Actions.Read, true);
            bool IsUserAdminWorker = CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1;
            //bool IsUserRegionalManager = workerroleRepository.IsWorkerRegionalAdmin() > 0 ? true : false;//CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1;

            //FilterDescriptor newDesc = new FilterDescriptor("CaseID", FilterOperator.IsEqualTo, caseId);
            //dsRequest.Filters.Add(newDesc);

            //var primaryWorkerID = GetPrimaryWorkerOfTheCase(caseId);

            //List<CaseGoalNew> caseGoalNew = caseGoalNewRepository.CaseGoalNewByCaseID(caseId);
            //return Json(caseGoalNew, JsonRequestBehavior.AllowGet);

            DataSourceResult result = caseactionnewRepository.CaseGoalWorkNote(caseGoalId).AsEnumerable().ToDataSourceResult(dsRequest);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadGoalStatusAjax()
        {
            return Json(goalstatusRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadGoalAssigneeRoleAjax()
        {
            return Json(goalassigneeroleRepository.AllActiveForDropDownList, JsonRequestBehavior.AllowGet);
        }

    }
}