using System.Web.Mvc;

namespace eCMS.Web.Areas.WorkerManagement
{
    public class WorkerManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WorkerManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WorkerManagement_default",
                "WorkerManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
