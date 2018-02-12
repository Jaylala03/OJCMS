//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories;
using eCMS.DataLogic.Models;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Helpers
{
    public class WorkerAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var routeData = ((MvcHandler)httpContext.Handler).RequestContext.RouteData;
                object currentAreaName = string.Empty;
                routeData.Values.TryGetValue("area", out currentAreaName);
                object currentControllerName = string.Empty;
                routeData.Values.TryGetValue("controller", out currentControllerName);
                object currentActionName = string.Empty;
                routeData.Values.TryGetValue("action", out currentActionName);
                //if (currentControllerName.ToString(true).ToLower() == Constants.CommonConstants.DefaultControllerName.ToLower() && currentActionName.ToString(true).ToLower() == Constants.CommonConstants.DefaultActionName.ToLower())
                //{
                //    return true;
                //}
                Worker loggedInWorker = WebHelper.CurrentSession.Content.LoggedInWorker;
                if (loggedInWorker == null)
                {
                    IWorkerRepository workerRepository = DependencyResolver.Current.GetService(typeof(IWorkerRepository)) as IWorkerRepository;

                    loggedInWorker = workerRepository.Find(httpContext.User.Identity.Name);
                    WebHelper.CurrentSession.Content.LoggedInWorker = loggedInWorker;
                }
                if (loggedInWorker != null)
                {
                    return true;
                }
            }
            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }

    public class UpdateRoleAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var routeData = ((MvcHandler)httpContext.Handler).RequestContext.RouteData;
                object currentAreaName = string.Empty;
                routeData.Values.TryGetValue("area", out currentAreaName);
                object currentControllerName = string.Empty;
                routeData.Values.TryGetValue("controller", out currentControllerName);
                object currentActionName = string.Empty;
                routeData.Values.TryGetValue("action", out currentActionName);
                //if (currentControllerName.ToString(true).ToLower() == Constants.CommonConstants.DefaultControllerName.ToLower() && currentActionName.ToString(true).ToLower() == Constants.CommonConstants.DefaultActionName.ToLower())
                //{
                //    return true;
                //}
                Worker loggedInWorker = WebHelper.CurrentSession.Content.LoggedInWorker;
                if (loggedInWorker == null)
                {
                    IWorkerRepository workerRepository = DependencyResolver.Current.GetService(typeof(IWorkerRepository)) as IWorkerRepository;
                    loggedInWorker = workerRepository.Find(httpContext.User.Identity.Name);
                    WebHelper.CurrentSession.Content.LoggedInWorker = loggedInWorker;
                }
                if (loggedInWorker != null)
                {
                    if (loggedInWorker.ID == loggedInWorker.CreatedByworkerID)
                        return true;
                    else
                        return false;

                }
            }
            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }

}
