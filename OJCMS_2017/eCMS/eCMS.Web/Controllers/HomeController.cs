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
using System;
using System.Web.Mvc;
using System.Linq;
//using Stripe;

namespace eCMS.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICaseActionRepository caseactionRepository;
        public HomeController(IWorkerRepository workerRepository,
            IWorkerRolePermissionRepository workerRolePermissionRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            ICaseActionRepository caseactionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.workerRepository = workerRepository;
            this.workerRolePermissionRepository = workerRolePermissionRepository;
            this.caseactionRepository = caseactionRepository;
        }

        [WorkerAuthorize]
        public ActionResult Index(string id)
        {
            if (CurrentLoggedInWorker == null || (CurrentLoggedInWorker != null && CurrentLoggedInWorker.ID == 0))
            {
                return RedirectToAction(Constants.Actions.Logout, Constants.Controllers.Account, new { Area = String.Empty });

            }
            if (IsRegionalAdministrator)
            {
                return RedirectToAction(Constants.Actions.Index, Constants.Controllers.Worker, new { Area = Constants.Areas.WorkerManagement });
            }
            else
            {
                return RedirectToAction(Constants.Actions.Index, Constants.Controllers.Case, new { Area = Constants.Areas.CaseManagement });
                // return View();
            }
        }

        public ActionResult Error()
        {
            BaseModel baseModel = new BaseModel();
            baseModel.ErrorMessage = WebHelper.CurrentSession.Content.ErrorMessage;
            if (baseModel.ErrorMessage.IsNullOrEmpty())
            {
                baseModel.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return View(baseModel);
        }

        public ActionResult AccessDenied()
        {
            BaseModel baseModel = new BaseModel();
            baseModel.ErrorMessage = WebHelper.CurrentSession.Content.ErrorMessage;
            if (baseModel.ErrorMessage.IsNullOrEmpty())
            {
                baseModel.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            if (Request.UrlReferrer != null)
            {
                baseModel.FancyBoxLink = Request.UrlReferrer.ToString();
            }
            else
            {
                if (IsRegionalAdministrator)
                {
                    baseModel.FancyBoxLink = Url.Action(Constants.Actions.Index, Constants.Controllers.Worker, new { Area = Constants.Areas.WorkerManagement });
                }
                else
                {
                    baseModel.FancyBoxLink = Url.Action(Constants.Actions.Index, Constants.Controllers.Case, new { Area = Constants.Areas.CaseManagement });
                }
            }
            return View(baseModel);
        }

        public ActionResult Adjust()
        {
//            string str = @"[Comments=<p style=/""margin-left:72.0pt;/"">
//	Rukhshana received Food hamper for this month as well as grocery gift card with gift boxes. She is doing fine.</p>
//<p style=/""margin-left:72.0pt;/"">
//	Naz was denied AISH benefits. She is gathering recent medical reports to submit to AISH and will request to file another appeal.&nbsp;</p>
//]
//[Circle of Support=]";
//            str = str.Replace("</p>", Environment.NewLine);
//            str = str.RemoveHTML();
            var caseActionList = caseactionRepository.All.ToList();
            foreach (CaseAction caseAction in caseActionList)
            {
                caseAction.Action = caseAction.Action.Replace("</p>", Environment.NewLine);
                caseAction.Action = caseAction.Action.RemoveHTML();
                caseactionRepository.Update(caseAction);
            }
            return Content(String.Empty);
        }
    }
}
