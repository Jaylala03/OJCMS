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
using System.Web.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseMemberContactController : CaseBaseController
    {
        private readonly ICaseMemberContactRepository casemembercontactRepository;
        public CaseMemberContactController(IWorkerRepository workerRepository, 
            ICaseMemberRepository casememberRepository, 
            IContactMediaRepository contactmediaRepository,
            ICaseMemberContactRepository casemembercontactRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseRepository caseRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {                           
            this.workerRepository = workerRepository;
            this.casememberRepository = casememberRepository;
            this.contactmediaRepository = contactmediaRepository;
            this.casemembercontactRepository = casemembercontactRepository;
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, string caseMemberID)  		        
        {
			if (dsRequest.Filters == null)
			{
				dsRequest.Filters = new List<IFilterDescriptor>();
			}
            FilterDescriptor newDesc = new FilterDescriptor("CaseMemberID", FilterOperator.IsEqualTo, caseMemberID);
				dsRequest.Filters.Add(newDesc);
                DataSourceResult result = casemembercontactRepository.AllIncluding(caseMemberID.ToInteger(true), casemembercontact => casemembercontact.CreatedByWorker, casemembercontact => casemembercontact.LastUpdatedByWorker, casemembercontact => casemembercontact.CaseMember, casemembercontact => casemembercontact.ContactMedia).Select(casemembercontact => new { casemembercontact.ID, casemembercontact.CreateDate, casemembercontact.LastUpdateDate, casemembercontact.CreatedByWorkerID, CreatedByWorkerName = casemembercontact.CreatedByWorker.FirstName + " " + casemembercontact.CreatedByWorker.LastName, casemembercontact.LastUpdatedByWorkerID, LastUpdatedByWorkerName = casemembercontact.LastUpdatedByWorker.FirstName + " " + casemembercontact.LastUpdatedByWorker.LastName, casemembercontact.IsArchived, casemembercontact.CaseMemberID, casemembercontact.ContactMediaID, ContactMediaName = casemembercontact.ContactMedia.Name, casemembercontact.Contact, casemembercontact.Comments, casemembercontact.EmergencyContactNumber, casemembercontact .EmergencyContactName}).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens casemembercontact editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">casemembercontact id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string caseMemberID)
        {
            CaseMemberContact caseMemberContact = null;
            if (id > 0)
            {
                //find an existing casemembercontact from database
                caseMemberContact = casemembercontactRepository.Find(id);
                if (caseMemberContact == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Member Contact not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                caseMemberContact = new CaseMemberContact();
                caseMemberContact.CaseMemberID = caseMemberID.ToInteger(true);
            }
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, caseMemberContact));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="casemembercontact">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(CaseMemberContact casemembercontact)
        {
            bool isNew = casemembercontact.ID == 0; 
            
            if (ModelState.IsValid)
            {

                try
                {
                    //<JL:Comment:No need to check access again on post. On edit we are already checking permission.>
                    //if (casemembercontact.ID > 0)
                    //{
                    //    var casemembercontact1 = casemembercontactRepository.Find(casemembercontact.ID);

                    //    var primaryWorkerID = GetPrimaryWorkerOfTheCase(casemembercontact1.CaseMember.CaseID);
                    //    casemembercontact1 = new CaseMemberContact();
                    //    if (casemembercontact.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    //    {
                    //        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                    //        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //    }
                    //}
                    //</JL:Comment:07/08/2017>

                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                     Regex nonNumericRegex = new Regex(@"\D");
                    Match match = regex.Match(casemembercontact.Contact);
                    if (!match.Success)
                    {
                       
                        if (nonNumericRegex.IsMatch(casemembercontact.Contact))
                        {
                            casemembercontact.ErrorMessage = "Please enter valid email or phone.";
                            return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
                        }
                    }
                    if (nonNumericRegex.IsMatch(casemembercontact.EmergencyContactNumber))
                    {
                        casemembercontact.ErrorMessage = "Please enter valid contact number.";
                        return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
                    }
                    casemembercontact.LastUpdateDate = DateTime.Now;
                    casemembercontact.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;                    
                    casemembercontactRepository.InsertOrUpdate(casemembercontact);
                    casemembercontactRepository.Save();

                   
                    //set status message
                    if (isNew)
                    {
                        casemembercontact.SuccessMessage = "Contact has been added successfully";
                    }
                    else
                    {
                        casemembercontact.SuccessMessage = "Contact has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    casemembercontact.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    casemembercontact.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        casemembercontact.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (casemembercontact.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (casemembercontact.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
            }
        }

        /// <summary>
        /// delete casemembercontact from database usign ajax operation
        /// </summary>
        /// <param name="id">casemembercontact id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            //find the casemembercontact in database
            CaseMemberContact casemembercontact = casemembercontactRepository.Find(id);
            if (casemembercontact == null)
            {
                //set error message if it does not exist in database
                casemembercontact = new CaseMemberContact();
                casemembercontact.ErrorMessage = "CaseMemberContact not found";
            }
            else
            {
                try
                {
                    var primaryWorkerID = GetPrimaryWorkerOfTheCase(casemembercontact.CaseMember.CaseID);
                    if (casemembercontact.ID > 0 && casemembercontact.CreatedByWorkerID != CurrentLoggedInWorker.ID && primaryWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1))
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                        return Json(new { success = true, url = Url.Action(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty }) });
                        //return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    }
                    //delete casemembercontact from database
                    casemembercontactRepository.Delete(casemembercontact);
                    casemembercontactRepository.Save();
                    //set success message
                    casemembercontact.SuccessMessage = "Case Member Contact has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    casemembercontact.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        casemembercontact.SuccessMessage = "Case Member Contact has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        casemembercontact.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            if (casemembercontact.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
            }
        }


        [WorkerAuthorize]
        public ActionResult ListAjax(int caseMemberID)
        {
            CaseMemberContact caseMemberContact = new CaseMemberContact();
            caseMemberContact.CaseMemberID = Convert.ToInt32(caseMemberID);
            caseMemberContact.ID = 0;
            return Content(this.RenderPartialViewToString(Constants.Actions.EditorPopUp, caseMemberContact));
        }

        //[WorkerAuthorize]
        //public ActionResult EditorPopUpListAjax(int id, string caseMemberID)
        //{
        //    CaseMemberContact caseMemberContact = null;
        //    if (id > 0)
        //    {
        //        //find an existing casemembercontact from database
        //        caseMemberContact = casemembercontactRepository.Find(id);
        //        if (caseMemberContact == null)
        //        {
        //            //throw an exception if id is provided but data does not exist in database
        //            return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Case Member Contact not found");
        //        }
        //    }
        //    else
        //    {
        //        //create a new instance if id is not provided
        //        caseMemberContact = new CaseMemberContact();
        //        caseMemberContact.CaseMemberID = caseMemberID.ToInteger(true);
        //    }

        //    return Content(this.RenderPartialViewToString(Constants.PartialViews.EditorPopUpList, caseMemberContact));
        //}

    }
}

