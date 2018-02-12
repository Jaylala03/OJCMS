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
namespace eCMS.Web.Areas.Lookup.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository)
            : base(workerroleactionpermissionRepository)
        {
        }

        /// <summary>
        /// This action returns the list of Region
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchRegion">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}

