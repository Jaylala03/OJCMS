//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class BaseLookupRepository<T> : BaseRepository<T>,IBaseLookupRepository<T> where T : LookupBaseModel
    {
        public BaseLookupRepository(RepositoryContext context)
            :base(context)
        {
        }

        public virtual List<SelectListItem> AllActiveForDropDownList 
        {
            get
            {
                return context.Set<T>().AsQueryable().Where(item => item.IsActive == true).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
        }

        public virtual List<SelectListItem> AllForDropDownList
        {
            get
            {
                return context.Set<T>().AsQueryable().OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
        }

    }
}
