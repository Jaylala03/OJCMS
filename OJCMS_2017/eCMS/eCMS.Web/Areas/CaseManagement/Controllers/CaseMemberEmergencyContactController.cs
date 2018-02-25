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
using System.Text.RegularExpressions;
using System.Web.Mvc;
namespace eCMS.Web.Areas.CaseManagement.Controllers
{
    public class CaseMemberEmergencyContactController : CaseBaseController
    {
        private readonly ICaseMemberEmergencyContactRepository casememberEmergencyContactRepository;
        public CaseMemberEmergencyContactController(IWorkerRepository workerRepository, 
            ICaseMemberRepository casememberRepository, 
            ICaseMemberEmergencyContactRepository casememberEmergencyContactRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseRepository caseRepository)
            : base(workerroleactionpermissionRepository, caseRepository, workerroleactionpermissionnewRepository)
        {                           
            this.workerRepository = workerRepository;
            this.casememberRepository = casememberRepository;
            this.contactmediaRepository = contactmediaRepository;
            this.casememberEmergencyContactRepository = casememberEmergencyContactRepository;
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

                DataSourceResult result = casememberEmergencyContactRepository.AllIncluding(
                    caseMemberID.ToInteger(true), 
                    casemembercontact => casemembercontact.CreatedByWorker, 
                    casemembercontact => casemembercontact.LastUpdatedByWorker, 
                    casemembercontact => casemembercontact.CaseMember)
                    .Select(
                    casemembercontact => new 
                    { 
                        casemembercontact.ID, 
                        casemembercontact.CreateDate, 
                        casemembercontact.LastUpdateDate, 
                        casemembercontact.CreatedByWorkerID, 
                        CreatedByWorkerName = casemembercontact.CreatedByWorker.FirstName + " " + casemembercontact.CreatedByWorker.LastName, 
                        casemembercontact.LastUpdatedByWorkerID, 
                        LastUpdatedByWorkerName = casemembercontact.LastUpdatedByWorker.FirstName + " " + casemembercontact.LastUpdatedByWorker.LastName,
                        casemembercontact.IsArchived,
                        casemembercontact.CaseMemberID,
                        casemembercontact.ContactNumber, 
                        casemembercontact .ContactName
                    }).ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Save([DataSourceRequest] DataSourceRequest request, CaseMemberEmergencyContact casemembercontact, int caseMemberID)
        {
            bool isNew = casemembercontact.ID == 0;

            if (ModelState.IsValid)
            {

                try
                {

                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Regex nonNumericRegex = new Regex(@"\D");
                    Match match = regex.Match(casemembercontact.ContactNumber);
                    if (!match.Success)
                    {

                        if (nonNumericRegex.IsMatch(casemembercontact.ContactNumber))
                        {
                            casemembercontact.ErrorMessage = "Please enter valid phone.";
                            return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
                        }
                    }
                 
                    casemembercontact.LastUpdateDate = DateTime.Now;
                    casemembercontact.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    casememberEmergencyContactRepository.InsertOrUpdate(casemembercontact);
                    casememberEmergencyContactRepository.Save();

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

            return Json(new[] { casemembercontact }.ToDataSourceResult(request, ModelState));
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, CaseMemberEmergencyContact casemembercontact)
        {
            //find the casemembercontact in database
            if (casemembercontact == null)
            {
                //set error message if it does not exist in database
                casemembercontact = new CaseMemberEmergencyContact();
                casemembercontact.ErrorMessage = "CaseMemberEmergencyContact not found";
            }
            else
            {
                try
                {
                    //delete casemembercontact from database
                    casememberEmergencyContactRepository.Delete(casemembercontact);
                    casememberEmergencyContactRepository.Save();
                    //set success message
                    casemembercontact.SuccessMessage = "Contact has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    casemembercontact.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 for information on understanding and handling optimistic concurrency exceptions.")
                    {
                        casemembercontact.SuccessMessage = "Contact has been deleted successfully";
                    }
                    else
                    {
                        ExceptionManager.Manage(ex);
                        casemembercontact.ErrorMessage = Constants.Messages.UnhandelledError;
                    }
                }
            }
            //return action status in json to display on a message bar
            //if (casemembercontact.ErrorMessage.IsNotNullOrEmpty())
            //{
            //    return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
            //}
            //else
            //{
            //    return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, casemembercontact) });
            //}

            return Json(new[] { casemembercontact }.ToDataSourceResult(request, ModelState));
        }
    }
}

