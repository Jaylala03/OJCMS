using System.Web.Mvc;

namespace eCMS.Web.Areas.CaseManagement
{
    public class CaseManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CaseManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CaseManagement_default",
                "CaseManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
