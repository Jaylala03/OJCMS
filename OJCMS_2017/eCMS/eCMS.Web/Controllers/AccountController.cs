//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Contact: Rahim Bhatia, sal@softwaregrid.com
//	  http://www.softwaregrid.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.ViewModels;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using Stripe;
namespace eCMS.Web.Controllers
{
    public class AccountController : BaseController
    {
        private WorkerAuthenticationManager workerAuthenticationManager;
        public AccountController(IWorkerRepository workerRepository, 
            IGenderRepository genderRepository, 
            ICountryRepository countryRepository, 
            IStateRepository stateRepository,
            WorkerAuthenticationManager workerAuthenticationManager,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.workerRepository = workerRepository;
            this.countryRepository = countryRepository;
            this.stateRepository = stateRepository;
            this.genderRepository = genderRepository;
            this.workerAuthenticationManager = workerAuthenticationManager;
        }

        public ActionResult Login()
        {
            Worker worker = workerAuthenticationManager.GetSavedWorkerNameAndPassword();
            if(worker.EmailAddress.IsNotNullOrEmpty())
            {
                worker.RememberMe = true;
            }
            return View(worker);
        }

        [HttpPost]
        public ActionResult Login(Worker worker)
        {
            try
            {
                Worker loggedInWorker =workerAuthenticationManager.AuthenticateWorker(worker.EmailAddress, worker.Password, false);
                if (loggedInWorker!=null)
                {
                    if (!loggedInWorker.LastPasswordChangeDate.HasValue || (loggedInWorker.LastPasswordChangeDate.HasValue && (DateTime.Now-loggedInWorker.LastPasswordChangeDate.Value).TotalDays>90))
                    {
                        if (!loggedInWorker.LastPasswordChangeDate.HasValue)
                        {
                            return RedirectToAction(Constants.Actions.ChangePassword, Constants.Controllers.Account, new { isFirstLogin=true });
                        }
                        else
                        {
                            return RedirectToAction(Constants.Actions.ChangePassword, Constants.Controllers.Account);
                        }
                    }
                    string returnUrl = string.Empty;
                    if (Request.UrlReferrer != null)
                    {
                        returnUrl = Request.UrlReferrer.Query.Replace("?ReturnUrl=", string.Empty);
                        returnUrl = HttpUtility.UrlDecode(returnUrl);
                    }
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (IsRegionalAdministrator)
                        {
                            return RedirectToAction(Constants.Actions.Index, Constants.Controllers.Worker, new { Area = Constants.Areas.WorkerManagement });
                        }
                        else
                        {
                            return RedirectToAction(Constants.Actions.Index, Constants.Controllers.Case, new { Area = Constants.Areas.CaseManagement });
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
                worker.ErrorMessage = Constants.Messages.UserLogin_UnknownError;
                ExceptionManager.Manage(ex);
            }
            return View(worker);
        }

        public ActionResult Logout()
        {
            LogoutProcess();
            return RedirectToAction(Constants.Actions.Login, Constants.Controllers.Account, new { Area = string.Empty });
        }

        private void LogoutProcess()
        {
            FormsAuthentication.SignOut();
            WebHelper.CurrentSession.Clear();
            WebHelper.CurrentSession.Content.Data = null;
            WebHelper.CurrentSession.Content.LoggedInWorker = null;
            WebHelper.CurrentSession.Content.RedirectUrl = null;
            CookieHelper newCookieHelper = new CookieHelper();
            newCookieHelper.ForgetMe();
            HttpContext.User = null;
        }

        [WorkerAuthorize]
        public ActionResult ChangePassword(bool? isFirstLogin)
        {
            ChangePasswordModel newChangePasswordModel = new ChangePasswordModel();
            if (isFirstLogin.HasValue && isFirstLogin.Value)
            {
                newChangePasswordModel.IsFirstLogin = true;
            }
            return View(newChangePasswordModel);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                BaseModel statusModel = workerRepository.ChangePassword(CurrentLoggedInWorker.ID, model.CurrentPassword,model.NewPassword,model.ConfirmPassword);
                model.ErrorMessage = statusModel.ErrorMessage;
                model.SuccessMessage = statusModel.SuccessMessage;
                if (model.SuccessMessage != null)
                {
                    return Redirect("/CaseManagement/Case");
                }
            }
            catch (CustomException ex)
            {
                model.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                model.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            Worker worker = new Worker();
            return View(worker);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(string EmailAddress)
        {
            Worker workerModel = new Worker();
            try
            {
                BaseModel statusModel = workerRepository.ForgotPassword(EmailAddress);
                workerModel.ErrorMessage = statusModel.ErrorMessage;
                workerModel.SuccessMessage = statusModel.SuccessMessage;
                if (statusModel.SuccessMessage.IsNotNullOrEmpty())
                {
                    workerModel.SuccessMessage = "Your password has been sent through email successfully.";
                }
                if (statusModel.ErrorMessage.IsNotNullOrEmpty())
                {
                    workerModel.ErrorMessage = "There is a problem while sending password.";
                }
            }
            catch (CustomException ex)
            {
                workerModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                workerModel.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return View(workerModel);
        }
    }
}

