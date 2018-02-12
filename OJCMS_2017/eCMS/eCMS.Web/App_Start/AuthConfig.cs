using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;

namespace eCMS.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "hHKbaCKOBbXdCIKr2lPA",
                consumerSecret: "mTzP3Eql4R0w7Ab9taf09kgVvxS8PoMUX0qx0q76Z4");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "556813924396028",
                appSecret: "f18ef48a28257e02adbcd69857cbd318");
        }
    }
}
