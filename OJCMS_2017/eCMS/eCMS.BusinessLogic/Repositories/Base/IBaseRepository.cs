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
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        T Find(int id);
        void Delete(int id);
        void Delete(T entity);
        void Save();
        bool IsDisposed { get; }
        void Remove(BaseModel baseModel);
        void InsertOrUpdate(BaseModel baseModel);
    }
}
