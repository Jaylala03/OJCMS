//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public interface IBaseLookupRepository<T>:IBaseRepository<T> where T : LookupBaseModel
    {
        List<SelectListItem> AllActiveForDropDownList { get; }
        List<SelectListItem> AllForDropDownList { get; }
    }
}
