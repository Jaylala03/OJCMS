//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using EasySoft.Helper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using eCMS.Shared;

namespace eCMS.BusinessLogic.Helpers
{
    public sealed partial class WebHelper
    {
        public static class CurrentSession
        {
            /// <summary>
            /// Id of the current session
            /// </summary>
            public static string Id
            {
                get
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    {
                        return HttpContext.Current.Session.SessionID;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            /// <summary>
            /// Current session timout
            /// </summary>
            public static int Timeout
            {
                get
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    {
                        return HttpContext.Current.Session.Timeout;
                    }
                    else
                    {
                        return int.MinValue;
                    }
                }
            }

            /// <summary>
            /// Clear session
            /// </summary>
            [DebuggerStepThrough()]
            public static void Clear()
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.Clear();
                }
            }
            /// <summary>
            /// Restart current session
            /// </summary>
            [DebuggerStepThrough()]
            public static void Restart()
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.Abandon();
                }
            }
            /// <summary>
            /// Get an item from the collection of objects stored in session.
            /// </summary>
            /// <param name="key">key name of the item</param>
            /// <returns>object found by the supplied key</returns>
            [DebuggerStepThrough()]
            public static object Get(string key)
            {
                if (HttpContext.Current != null && HttpContext.Current.Session!=null)
                {
                    return HttpContext.Current.Session[BuildFullKey(key)];
                }
                else
                {
                    return null;
                }
            }
            /// <summary>
            /// Add a new item to the collection of objects stored in session
            /// </summary>
            /// <param name="key">unique key name of the item</param>
            /// <param name="value">object to store in the session</param>
            [DebuggerStepThrough()]
            public static void Set(string key,object value)
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (value == null)
                    {
                        HttpContext.Current.Session.Remove(BuildFullKey(key));
                    }
                    else
                    {
                        HttpContext.Current.Session[BuildFullKey(key)] = value;
                    }
                }
            }
            /// <summary>
            /// Remove an item from the collection of the objects stored in session.
            /// </summary>
            /// <param name="key">key name of the item</param>
            [DebuggerStepThrough()]
            public static void Remove(string key)
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.Remove(BuildFullKey(key));
                }
            }
            /// <summary>
            /// Check if any item exists in the collection of the objects stored in session
            /// </summary>
            /// <param name="key">key to search</param>
            /// <returns>found or not found</returns>
            [DebuggerStepThrough()]
            public static bool Contains(string key)
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session[BuildFullKey(key)] == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Build an unique key to store a item in session
            /// </summary>
            /// <param name="localKey"></param>
            /// <returns></returns>
            [DebuggerStepThrough()]
            private static string BuildFullKey(string localKey)
            {
                const string SESSION_KEY = "Web.UI.";

                if (localKey.IndexOf(SESSION_KEY) > -1)
                {
                    return localKey;
                }
                else
                {
                    return SESSION_KEY + localKey;
                }
            }

            /// <summary>
            /// Search a string item in the collection of objects stored in session by the supplied key name.
            /// </summary>
            /// <param name="key">key to search</param>
            /// <returns>string found</returns>
            [DebuggerStepThrough()]
            public static string GetString(string key)
            {
                string fullKey = BuildFullKey(key);
                if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[fullKey] != null)
                {
                    return HttpContext.Current.Session[fullKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            public static List<int> GetCurrentLoggedInWorkerRole_RegionIDs(string key)
            {
                string fullKey = BuildFullKey(key);
                if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[fullKey] != null)
                {
                    return (List<int>)HttpContext.Current.Session[fullKey];
                }
                else
                {
                    return null;
                }
            }
            [DebuggerStepThrough()]
            public static string GetNullString(string key)
            {
                string fullKey = BuildFullKey(key);
                if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[fullKey] != null)
                {
                    return HttpContext.Current.Session[fullKey].ToString();
                }
                else
                {
                    return null;
                }
            }

            public static class Content
            {
                public static object Data
                {
                    get
                    {
                        return (object)Get("Data");
                    }
                    set
                    {
                        Set("Data", value);
                    }
                }

                public static Worker LoggedInWorker
                {
                    get
                    {
                        return (Worker)Get("LoggedInWorker");
                    }
                    set
                    {
                        Set("LoggedInWorker", value);
                    }
                }

                public static VisibilityStatus RegionVisibility
                {
                    get
                    {
                        return (VisibilityStatus)Get("RegionVisibility").ToInteger(true);
                    }
                    set
                    {
                        Set("RegionVisibility", value);
                    }
                }

                public static VisibilityStatus ProgramVisibility
                {
                    get
                    {
                        return (VisibilityStatus)Get("ProgramVisibility").ToInteger(true);
                    }
                    set
                    {
                        Set("ProgramVisibility", value);
                    }
                }

                public static VisibilityStatus SubProgramVisibility
                {
                    get
                    {
                        return (VisibilityStatus)Get("SubProgramVisibility").ToInteger(true);
                    }
                    set
                    {
                        Set("SubProgramVisibility", value);
                    }
                }

                public static VisibilityStatus CaseVisibility
                {
                    get
                    {
                        return (VisibilityStatus)Get("CaseVisibility").ToInteger(true);
                    }
                    set
                    {
                        Set("CaseVisibility", value);
                    }
                }

                public static List<WorkerRoleActionPermission> WorkerRoleActionPermissionList
                {
                    get
                    {
                        return (List<WorkerRoleActionPermission>)Get("WorkerRoleActionPermissionList");
                    }
                    set
                    {
                        Set("WorkerRoleActionPermissionList", value);
                    }
                }

                public static List<WorkerRoleActionPermissionNew> WorkerRoleActionPermissionListNew
                {
                    get
                    {
                        return (List<WorkerRoleActionPermissionNew>)Get("WorkerRoleActionPermissionListNew");
                    }
                    set
                    {
                        Set("WorkerRoleActionPermissionListNew", value);
                    }
                }

                //public static string LoggedInWorkerRoleIDs
                //{
                //    get
                //    {
                //        return GetString("LoggedInWorkerRoleIDs");
                //    }
                //    set
                //    {
                //        Set("LoggedInWorkerRoleIDs", value);
                //    }
                //}

                public static List<int> LoggedInWorkerRoleIDs
                {
                    get
                    {
                        return GetCurrentLoggedInWorkerRole_RegionIDs("LoggedInWorkerRoleIDs");
                    }
                    set
                    {
                        Set("LoggedInWorkerRoleIDs", value);
                    }
                }

                //public static string LoggedInWorkerRegionIDs
                //{
                //    get
                //    {
                //        return GetString("LoggedInWorkerRegionIDs");
                //    }
                //    set
                //    {
                //        Set("LoggedInWorkerRegionIDs", value);
                //    }
                //}

                public static List<int> LoggedInWorkerRegionIDs
                {
                    get
                    {
                        return GetCurrentLoggedInWorkerRole_RegionIDs("LoggedInWorkerRegionIDs");
                    }
                    set
                    {
                        Set("LoggedInWorkerRegionIDs", value);
                    }
                }
                public static string SuccessMessage
                {
                    get
                    {
                        return GetString("SuccessMessage");
                    }
                    set
                    {
                        Set("SuccessMessage", value);
                    }
                }

                public static string ErrorMessage
                {
                    get
                    {
                        return GetString("ErrorMessage");
                    }
                    set
                    {
                        Set("ErrorMessage", value);
                    }
                }

                public static string RedirectUrl
                {
                    get
                    {
                        return GetString("RedirectUrl");
                    }
                    set
                    {
                        Set("RedirectUrl", value);
                    }
                }
            }
        }
    }
}