using Autofac;
using Autofac.Integration.Mvc;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.ExceptionLoging;
using eCMS.Web.DependencyInjection;
using eCMS.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eCMS.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static Collection<ModuleBase> _modules;

        protected void Application_Start()
        {
            OnStart(new Action<ContainerBuilder>[] { RegisterServices, RegisterControllers, RegisterModelBinders, RegisterModelBinderProvider, RegisterConnectionString });
        }

        protected virtual void OnStart(params Action<ContainerBuilder>[] registerServices)
        {

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            _modules = new Collection<ModuleBase>();
            RegisterModules(_modules);

            var containerBuilder = new ContainerBuilder();

            foreach (var register in registerServices)
                register(containerBuilder);

            foreach (var module in _modules)
            {
                module.RegisterComponents(containerBuilder);
                module.RegisterRoutes(RouteTable.Routes);
            }

            var container = containerBuilder.Build();

            var repositoryContextInitializers = container.Resolve<IEnumerable<IRepositoryContextInitializer>>();
            foreach (var item in repositoryContextInitializers)
                item.InitializeDatabase();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected virtual void RegisterServices(ContainerBuilder containerBuilder) { }

        protected virtual void RegisterModules(Collection<ModuleBase> modules)
        {
            modules.Add(new eCMS.Web.DependencyInjection.Module());
        }

        protected virtual void RegisterControllers(ContainerBuilder containerBuilder)
        {
            foreach (var module in _modules)
            {
                containerBuilder.RegisterControllers(module.GetType().Assembly);
            }

            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());
        }

        protected virtual void RegisterModelBinders(ContainerBuilder containerBuilder)
        {
            foreach (var module in _modules)
            {
                containerBuilder.RegisterModelBinders(module.GetType().Assembly);
            }

            containerBuilder.RegisterModelBinders(Assembly.GetExecutingAssembly());
        }

        protected virtual void RegisterModelBinderProvider(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModelBinderProvider();
        }

        protected virtual void RegisterConnectionString(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(
                container =>
                new ConnectionString(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString)).As
                <IConnectionString>();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            Exception exc = Server.GetLastError();
            // Handle HTTP errors
            if (exc != null && exc.GetType() == typeof(HttpException))
            {
                if (SiteConfigurationReader.EnableHttpErrorLog)
                {
                    ExceptionManager.Manage(exc);
                }
                // The Complete Error Handling Example generates
                // some errors using URLs with "NoCatch" in them;
                // ignore these here to simulate what would happen
                // if a global.asax handler were not implemented.
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;

                //Redirect HTTP errors to HttpError page
                //WebHelper.CurrentSession.Content.ErrorMessage = ExceptionManager.BuildErrorStack(exc);
                //UrlHelper url = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
                //Response.Redirect(url.Action(Constants.Actions.Error, Constants.Controllers.Home, new { Area="" }));
            }
            else
            {
                // Log the exception and notify system operators
                if (exc != null)
                {
                    if (exc.GetType() == typeof(CustomException))
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = exc.Message;
                    }
                    else
                    {
                        WebHelper.CurrentSession.Content.ErrorMessage = ExceptionManager.BuildErrorStack(exc);
                    }
                    ExceptionManager.Manage(exc);
                }
                try
                {
                    if (HttpContext.Current!=null && HttpContext.Current.Request!=null && HttpContext.Current.Request.Url!=null && !HttpContext.Current.Request.Url.ToString().Contains("Error"))
                    {
                        UrlHelper url = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
                        Response.Redirect(url.Action(Constants.Actions.Error, Constants.Controllers.Home, new { Area = "" }));
                    }
                }
                catch { }
            }

            // Clear the error from the server
            Server.ClearError();
        }
    }
}