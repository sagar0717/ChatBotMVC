using ChatBotMVC.ViewModels;
using DialogMVC.Business;
using DialogMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;

namespace ChatBotMVC.Controllers
{
    public class ReportController : Controller
    {
       
        private ConfigureReport configureReport = new ConfigureReport();
        //Displays individual reports
        [Authorize(Roles = "Editor,Approver")]
        public ActionResult IndividualReport()
        {
            if (User.IsInRole("Editor"))
            {
                var allRulesByUser = configureReport.GetIndRules(User.Identity.Name);
                return View(allRulesByUser);
            }
            else
            {
                var allRules = configureReport.GetAllRules();
                return View(allRules);
            }          
        }
        //Displays overall reports
        [Authorize(Roles = "Approver")]
        public ActionResult OverallReport()
        {
            var result = configureReport.GetSystemReportData();
            return View(result);
        }
    }
}