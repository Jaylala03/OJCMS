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
using eCMS.Web.Areas.CaseManagement.Controllers;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using eCMS.BusinessLogic;
using System.Configuration;
using eCMS.DataLogic.ViewModels;
using eCMS.Web.Controllers;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseInitialAssessmentController : CaseBaseController
    {
        private readonly ICaseInitialAssessmentRepository caseInitialAssessmentRepository;
        
        
        public CaseInitialAssessmentController(ICaseInitialAssessmentRepository caseInitialAssessmentRepository, ICaseRepository caseRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository, IIndicatorTypeRepository indicatorTypeRepository, 
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {            
            this.caseInitialAssessmentRepository = caseInitialAssessmentRepository;
            this.indicatorTypeRepository = indicatorTypeRepository;
        }

        /// <summary>
        /// This action returns the list of CaseInitialAssessment
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchCaseInitialAssessment">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            
            List<InitialAssessmentVM> objllist = caseInitialAssessmentRepository.GetAllIndicators();

            return View(objllist);
        }        
    }
}