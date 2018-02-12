//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using System;
using System.Configuration;
using System.Web;

namespace eCMS.Shared
{
    public static class SiteConfigurationReader
    {
        public static string WebRoot
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    if (HttpContext.Current.Request.ApplicationPath != "/")
                    {
                        return HttpContext.Current.Request.Url.Scheme+"://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
                    }
                    else
                    {
                        return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
                    }
                }
                else
                {
                    string webRoot = GetAppSettingsString("WebRoot");
                    if (webRoot.IsNotNullOrEmpty())
                    {
                        return webRoot;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }

        private static string databaseConnectionString = string.Empty;
        public static string DatabaseConnectionString
        {
            get
            {
                if (databaseConnectionString.IsNullOrEmpty())
                {
                    databaseConnectionString = ConfigurationManager.ConnectionStrings["DataStorageObjects"].ConnectionString;
                }
                return databaseConnectionString;
            }
        }

        public static string GetAppSettingsString(string keyName)
        {
            try
            {
                return ConfigurationManager.AppSettings.Get(keyName);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static Int32 GetAppSettingsInteger(string keyName)
        {
            try
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get(keyName));
            }
            catch
            {
                return 0;
            }
        }

        public static Boolean EnableEmailSystem
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("EnableEmailSystem").ToBoolean();
            }
        }

        public static Boolean EnableHttpErrorLog
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("EnableHttpErrorLog").ToBoolean();
            }
        }

        public static String ImageLocation
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("ImageLocation");
            }
        }

        public static String FromEmail
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("FromEmail");
            }
        }

        public static String FromEmailName
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("FromEmailName");
            }
        }

        public static Int32 RegionalAdministratorRoleID
        {
            get
            {
                return GetAppSettingsInteger("RegionalAdministratorRoleID");
            }
        }

        public static Int32 RegionalManagerRoleID
        {
            get
            {
                return GetAppSettingsInteger("RegionalManagerRoleID");
            }
        }
    }
}
