using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Test()
        {
            return View();
        }


        public ActionResult Home()
        {
            return View();
        }


        [HttpGet]
        public string send(string D,string t,string m)
        {
            return GITAPI.dbFunctions.SendNotification(D, t, m,"");
        }
    }
}
