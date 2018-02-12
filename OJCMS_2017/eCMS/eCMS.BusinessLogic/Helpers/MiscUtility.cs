//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.DataLogic.Models.Lookup;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
namespace eCMS.BusinessLogic.Helpers
{
    public static class MiscUtility
    {
        public static IEnumerable<SelectListItem> GetAreas()
        {
            List<SelectListItem> listArea = new List<SelectListItem>();
            Assembly assembly = Assembly.Load("ATS");

            IEnumerable<String> areas = assembly.GetTypes()
                                                .Select(t => t.Namespace)
                                                .Where(t => t != null && t.Contains("Areas") && t.Contains("Controllers"))
                                                .Distinct();
            foreach (String area in areas)
            {
                SelectListItem currentItem = new SelectListItem();
                currentItem.Text = area.Substring("Areas.", ".Controllers");
                currentItem.Value = currentItem.Text;
                listArea.Add(currentItem);
            }
            listArea = listArea.OrderBy(item => item.Text).ToList();
            return listArea;
        }

        public static IEnumerable<SelectListItem> GetControllers(string areaName)
        {
            List<SelectListItem> listControllers = new List<SelectListItem>();
            if (areaName.IsNotNullOrEmpty())
            {
                Assembly assembly = Assembly.Load("ATS");

                IEnumerable<Type> controllerTypes = from t in assembly.GetExportedTypes()
                                                    where typeof(IController).IsAssignableFrom(t) && t.Namespace.Contains(areaName)
                                                    orderby t.Name ascending
                                                    select t;
                foreach (var controllerType in controllerTypes)
                {
                    SelectListItem currentItem = new SelectListItem();
                    currentItem.Text = controllerType.Name.Replace("Controller", string.Empty);
                    currentItem.Value = currentItem.Text;
                    listControllers.Add(currentItem);
                }
            }

            return listControllers;
        }

        public static IEnumerable<SelectListItem> GetActions(string controllerName)
        {
            List<SelectListItem> listActions = new List<SelectListItem>();
            if (controllerName.IsNotNullOrEmpty())
            {
                Assembly assembly = Assembly.Load("ATS");

                IEnumerable<Type> controllerTypes = from t in assembly.GetExportedTypes()
                                                    where typeof(IController).IsAssignableFrom(t) && t.Name.Contains(controllerName+"Controller")
                                                    select t;
                foreach (var controllerType in controllerTypes)
                {
                    List<string> actionNames = new List<string>();
                    MethodInfo[] mi = controllerType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                    foreach (MethodInfo m in mi)
                    {
                        if (m.IsPublic)
                        {
                            if (typeof(ActionResult).IsAssignableFrom(m.ReturnParameter.ParameterType))
                            {
                                bool isNotExist = true;
                                foreach (SelectListItem item in listActions)
                                {
                                    if (item.Text == m.Name)
                                    {
                                        isNotExist = false;
                                        break;
                                    }
                                }
                                if (isNotExist && !m.Name.Contains("Delete"))
                                {
                                    SelectListItem currentItem = new SelectListItem();
                                    currentItem.Text = m.Name;
                                    currentItem.Value = currentItem.Text;
                                    listActions.Add(currentItem);
                                }
                            }
                        }
                    }
                }
            }
            listActions = listActions.OrderBy(item=>item.Text).ToList();
            return listActions;
        }

        public static string CreateDirectory(string directoryName)
        {
            try
            {
                directoryName = HttpContext.Current.Server.MapPath(directoryName);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                return directoryName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GenerateRandomString(int length)
        {
            string[] Characters = new string[82] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "=", "<", ">", ",", ".", "{", "}" };

            Random RandChar = new Random();
            string randomStr = "";
            for (int i = 0; i < length; i++)
            {
                randomStr += Characters[Math.Abs(RandChar.Next(-81, 81))];
            }

            return randomStr;
        }

        public static string GetLocation(string strCityName, string strState)
        {
            string strLocation = strCityName.ToString(true).Trim();
            if (strState.IsNotNullOrEmpty())
            {
                strLocation = strLocation.Concate(", ", strState.ToString(true).Trim());
            }
            return strLocation;
        }

        public static string GetLocation(string strCityName, string strState, string strCountry)
        {
            string strLocation = strCityName.ToString(true).Trim();
            if (strState.IsNotNullOrEmpty())
            {
                strLocation = strLocation.Concate(", ", strState.ToString(true).Trim());
            }
            if (strCountry.IsNotNullOrEmpty())
            {
                strLocation = strLocation.Concate(", ", strCountry.ToString(true).Trim());
            }
            return strLocation;
        }

        public static string GetLocation(string strCityName, State state, Country country)
        {
            string strLocation = strCityName.ToString(true).Trim();
            if (state != null && state.Name.IsNotNullOrEmpty())
            {
                strLocation = strLocation.Concate(", ", state.Name.Trim());
            }
            if (country != null && country.Name.IsNotNullOrEmpty())
            {
                strLocation = strLocation.Concate(", ", country.Name.Trim());
            }
            return strLocation;
        }

        public static List<SelectListItem> ToList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            List<SelectListItem> list = new List<SelectListItem>();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                SelectListItem newSelectListItem = new SelectListItem();
                newSelectListItem.Text = GetDescription(value);
                newSelectListItem.Value = Convert.ToString((short)Enum.Parse(type, value.ToString()));
                list.Add(newSelectListItem);
            }

            return list;
        }

        public static string GetDescription(Enum value)
        {
            try
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                string description = value.ToString();
                FieldInfo fieldInfo = value.GetType().GetField(description);
                EnumDescriptionAttribute[] attributes = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    description = attributes[0].Description;
                }

                return description;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public static string GetCasePersonalizedId(string regionCode,string muridFirstName,string muridLastName,int caseID,string DisplayID)
        {
           
                if (regionCode.IsNullOrEmpty())
                {
                    regionCode = "NA";
                }
                string id = regionCode;
                if (muridFirstName.IsNullOrEmpty() && muridLastName.IsNullOrEmpty())
                {
                    id = id + "-NA";
                }
                else
                {
                    if (muridFirstName.IsNotNullOrEmpty())
                    {
                        id = id + "-" + muridFirstName.GetFirstChar();
                    }
                    if (muridLastName.IsNotNullOrEmpty())
                    {
                        id = id + muridLastName.GetFirstChar();
                    }
                }
                if (caseID == 0)
                {
                    id = id + "-" + DateTime.Now.ToString("hhmm");
                }
                else
                {
                    //id = id + "-" + caseID.ToString().PadLeft(4, '0');
                    if (!string.IsNullOrEmpty(DisplayID))
                    {
                        var casenumberArray = DisplayID.Split('-');
                        var caseNumber = casenumberArray[casenumberArray.Length - 1];
                        id = id + "-" + caseNumber;
                    }
                    
                }
            
            return id.ToUpper();
        }
    }

    public static class Extensions
    {
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                string partialViewContent=sw.GetStringBuilder().ToString();
                return partialViewContent;
            }
        }

        public static string RenderPartialViewToString(this Controller controller, ViewResult viewResult, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                string partialViewContent = sw.GetStringBuilder().ToString();
                return partialViewContent;
            }
        }
    }
}
