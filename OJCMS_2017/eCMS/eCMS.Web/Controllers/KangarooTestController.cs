using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eCMS.Web.Controllers
{
    public class KangarooTestController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        // GET: KangarooTest
        public ActionResult Index()
        {
            ViewBag.CurrentUserName = "Rahim Bhatia";
            return View();
        }

        public async Task<ActionResult> TestKangaroo()
        {
            var values = new Dictionary<string, string>
            {
               { "grant_type", "password" },
               { "client_id", "10125650" },
               { "client_secret", "fbOAg8UOdXDHkTgzo5lPE8fAcYK9OC84vd22Z6sL" },
               { "username", "testing@cookculture.com" },
                 { "password", "1234" },
                 { "application_key", "testing@cookculture.com" },
                 { "scope", "*" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.kangaroorewards.com/oauth/token", content);

            var responseString = await response.Content.ReadAsStringAsync();

            ViewBag.CurrentUserName = "Rahim Bhatia";
            return View();
        }

    }
}