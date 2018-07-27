using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DialogMVC.Business;
using DialogMVC.Data;

namespace ChatBotMVC.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        //Repsonse to button click Submit
        [HttpPost]
        public ActionResult Index(Rules rules)
        {
            SystemRules Rules = new SystemRules();
            string response = Rules.ResponseToQuery(Rules, rules.UserRequest);
            rules.SystemResponse = response;
            return View(rules);
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}