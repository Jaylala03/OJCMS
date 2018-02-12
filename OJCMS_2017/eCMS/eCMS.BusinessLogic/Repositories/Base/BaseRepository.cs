//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using EasySoft.Helper;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using eCMS.Shared;

namespace eCMS.BusinessLogic.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly RepositoryContext context;

        public BaseRepository(RepositoryContext context)
        {
            this.context = context;
        }



        public void Remove(BaseModel baseModel)
        {
            context.Entry(baseModel).State = System.Data.Entity.EntityState.Detached;
        }

        public virtual IQueryable<T> All
        {
            get
            {
                return context.Set<T>().AsQueryable();
            }
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query;
        }

        public virtual T Find(int id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual void Delete(int id)
        {
            var entity = Find(id);
            Delete(entity);
        }

        public virtual void Delete(T entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
        }

        public virtual void InsertOrUpdate(BaseModel baseModel)
        {
            if (baseModel.ID == default(int))
            {
                baseModel.CreateDate = DateTime.Now;
                baseModel.LastUpdateDate = DateTime.Now;
                // New entity
                context.Entry(baseModel).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                baseModel.LastUpdateDate = DateTime.Now;
                // Existing entity
                context.Entry(baseModel).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public Worker CurrentLoggedInWorker
        {
            get
            {
                Worker user = WebHelper.CurrentSession.Content.LoggedInWorker;
                if (user == null)
                {
                    if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        CookieHelper newCookieHelper = new CookieHelper();
                        int userId = newCookieHelper.GetWorkerDataFromLoginCookie().ToInteger(true);
                        if (userId > 0)
                        {
                            //IWorkerRepository workerRepository = DependencyResolver.Current.GetService(typeof(IWorkerRepository)) as IWorkerRepository;
                            //user = workerRepository.Find(userId);
                            WebHelper.CurrentSession.Content.LoggedInWorker = user;
                        }
                    }
                    if (user == null)
                    {
                        user = new Worker();
                    }
                }
                return user;
            }
        }

        //public String CurrentLoggedInWorkerRoleIDs
        //{
        //    get
        //    {
        //        String roleIDs = WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs;
        //        if (roleIDs.IsNullOrEmpty())
        //        {
        //            roleIDs = string.Empty;
        //            List<WorkerInRole> workerRoles = context.WorkerInRole.Where(item => item.WorkerID == CurrentLoggedInWorker.ID && item.EffectiveFrom <= DateTime.Now && item.EffectiveTo >= DateTime.Now).ToList();
        //            if (workerRoles != null)
        //            {
        //                foreach (WorkerInRole workerRole in workerRoles)
        //                {
        //                    roleIDs = roleIDs.Concate(',', workerRole.WorkerRoleID.ToString());
        //                }
        //            }
        //            WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs = roleIDs;
        //        }
        //        return roleIDs;
        //    }
        //}

        public List<int> CurrentLoggedInWorkerRoleIDs
        {
            get
            {
                List<int> roleIDs = WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs;
                if (roleIDs == null)
                {
                    List<WorkerInRole> workerRoles = context.WorkerInRole.Where(item => item.WorkerID == CurrentLoggedInWorker.ID && item.EffectiveFrom <= DateTime.Now && item.EffectiveTo >= DateTime.Now).ToList();
                    if (workerRoles != null)
                    {
                        foreach (WorkerInRole workerRole in workerRoles)
                        {
                            roleIDs.Add(workerRole.WorkerRoleID);
                        }
                    }
                    WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs = roleIDs;
                }
                return roleIDs;
            }
        }
        public List<int> CurrentLoggedInWorkerRegionIDs
        {
            get
            {
                List<int> regionIDs = WebHelper.CurrentSession.Content.LoggedInWorkerRegionIDs;
                if (regionIDs == null)
                {
                    List<WorkerInRole> workerRoles = context.WorkerInRole.Where(item => item.WorkerID == CurrentLoggedInWorker.ID && item.EffectiveFrom <= DateTime.Now && item.EffectiveTo >= DateTime.Now).ToList();
                    if (workerRoles != null)
                    {
                        foreach (WorkerInRole workerRole in workerRoles)
                        {
                            regionIDs.Add(workerRole.RegionID);
                        }
                    }
                    WebHelper.CurrentSession.Content.LoggedInWorkerRegionIDs = regionIDs;
                }
                return regionIDs;
            }
        }

        public Boolean IsCurrentLoggedInWorkerAdministrator
        {
            get
            {
                //if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
                {
                    return true;
                }
                return false;
            }
        }

        public Boolean IsCurrentLoggedInWorkerRegionalAdministrator
        {
            get
            {
                //if (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1)
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalAdministratorRoleID) != -1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsDisposed
        {
            get
            {
                if (context == null)
                {
                    return true;
                }
                return context.IsDisposed;
            }
        }
    }
}
