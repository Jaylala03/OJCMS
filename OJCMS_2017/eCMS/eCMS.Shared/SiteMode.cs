using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eCMS.Shared
{
    public static class SiteMode
    {
        public static string mode()
        {
            string mode = "Test System";
            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            String currentHost = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
           
            string myHostTest = SiteConfigurationReader.GetAppSettingsString("Test");
            string myHostLive = SiteConfigurationReader.GetAppSettingsString("Live");           

            if (currentHost== myHostTest)
            {
                mode = "Test System";            
            }
            if (currentHost == myHostLive)
            {
                mode = "Live System";
            }
            return mode;
        }

        public static string liveOrTestLink()
        {
            string link ="<a href='{0}' >{1}</a>";

            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            String currentHost = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

            string myHostTest = SiteConfigurationReader.GetAppSettingsString("Test");
            string myHostLive = SiteConfigurationReader.GetAppSettingsString("Live");

            //if (currentHost == myHostTest)
            //{
            //    link = string.Format(link, myHostLive,"go to Live System");
            //}
            if (currentHost == myHostLive)
            {
                link = string.Format(link, myHostTest, string.Empty);
            }
            else
            {
                link = string.Format(link, myHostLive, "Click here to access the LIVE OJCMS site");
            }
            return link;
        }
    }
}
