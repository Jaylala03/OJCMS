//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System.IO;

namespace eCMS.Shared
{
    public static class Extensions
    {
        public static string ToDisplayStyle(this bool value)
        {
            if (value)
            {
                return string.Empty;
            }
            else
            {
                return "display:none;";
            }
        }

        public static bool ContainsAny(this string str, params string[] values)
        {
            if (!string.IsNullOrEmpty(str) || values.Length > 0)
            {
                foreach (string value in values)
                {
                    if (str.Contains(value))
                        return true;
                }
            }

            return false;
        }

        public static bool ContainsAll(this string str, params string[] values)
        {
            if (!string.IsNullOrEmpty(str) || values.Length > 0)
            {
                foreach (string value in values)
                {
                    if (!str.Contains(value))
                        return false;
                }
            }

            return true;
        }

        public static bool IsImage(this string str)
        {
            FileInfo file = new FileInfo(str);
            string fileExtension = file.Extension.ToLower();
            if (fileExtension == ".jpeg" || fileExtension == ".gif" || fileExtension == ".png" || fileExtension == ".jpg")
            {
                return true;
            }
            return false;
        }

        public static bool HasValue(this int value)
        {
            if (value > 0)
            {
                return true;
            }
            return false;
        }

        public static bool HasValue(this int? value)
        {
            if (value > 0)
            {
                return true;
            }
            return false;
        }

        public static int ToValue(this int value)
        {
            return value;
        }

        public static int ToValue(this int? value)
        {
            if (value.HasValue)
            {
                return value.Value;
            }
            return 0;
        }
    }
}
