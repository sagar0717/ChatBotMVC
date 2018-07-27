
using ChatBotMVC.ViewModels;
using DialogMVC.Business;
using DialogMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Web;
using System.Web.Mvc;

namespace ChatBotMVC.Controllers
{
    [Authorize(Roles = "Editor,Approver")]
    public class DynamicRulesController : Controller
    {
        private Configure configure = new DialogMVC.Business.Configure();

        // GET: DyanamicRules
        public ActionResult Index(string search)
        {
            if (search == null || search == "")
            {
                return View(configure.LoadDynamicRules.ToList());
            }
            else
            {
                List<DyanamicRule> FilterRules = configure.SearchDyRuleByStatus(search);
                return View(FilterRules);
            }

        }

        [HttpPost]
        public ActionResult Index(FormCollection form, int? id)
        {
            string strDDLValue = Request.Form["Status"].ToString();
            var dyrule = configure.FixedSearchByRuleId((int)id);
            configure.ApproveRejectRule(dyrule, strDDLValue);
            return RedirectToAction("Index");
        }

        
        public ActionResult Create()
        {
            List<SelectListItem> columnList = new List<SelectListItem>();
            var columns = typeof(TopicInformationViewModel).GetProperties()
                        .Select(property => property.Name)
                        .ToList();
            columnList = columns.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            ViewBag.TopicInformationList = columnList;
            return View();
        }

        
        [HttpPost]
        public ActionResult Create([Bind(Include = "Query, Response, Keyword, Match")] DyanamicRule dynamic)
        {
            List<SelectListItem> columnList = new List<SelectListItem>();
            if (!ModelState.IsValid)
            {
                Create();
                return View(dynamic);
            }
            bool rule = configure.SaveDynamicRule(dynamic, User.Identity.Name);
            if (rule == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "This Query Already Exists.";
                Create();
                return View();
            }
        }
        //Edits a rule based on the id entered
        public ActionResult Edit(int? id)
        {

            List<SelectListItem> columnList = new List<SelectListItem>();
            var columns = typeof(TopicInformationViewModel).GetProperties()
                        .Select(property => property.Name)
                        .ToList();
            columnList = columns.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            ViewBag.TopicInformationList = columnList;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dynamic = configure.DynamicSearchByRuleId((int)id);
            if (dynamic == null)
            {
                return HttpNotFound();
            }
            return View(dynamic);
        }

        [HttpPost]
        public ActionResult Edit(DyanamicRule dynamic)
        {
            if (!ModelState.IsValid)
            {

                List<SelectListItem> columnList = new List<SelectListItem>();
                var columns = typeof(TopicInformationViewModel).GetProperties()
                            .Select(property => property.Name)
                            .ToList();
                columnList = columns.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
                ViewBag.TopicInformationList = columnList;
                return View(dynamic);
            }
            bool rule = configure.SaveDynamicRule(dynamic, User.Identity.Name);
            if (rule == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "This Query Already Exists.";

                List<SelectListItem> columnList = new List<SelectListItem>();
                var columns = typeof(TopicInformationViewModel).GetProperties()
                            .Select(property => property.Name)
                            .ToList();
                columnList = columns.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
                ViewBag.TopicInformationList = columnList;
                return View();

            }

        }
        // GET: Fixedrule/Delete/5
        // Retrieve details of a contact to confirm deletion
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dynamic = configure.DynamicSearchByRuleId((int)id);
            if (dynamic == null)
            {
                return HttpNotFound();
            }
            return View(dynamic);
        }

        // POST: PersonalContacts/Delete/5
        // Delete a contact
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var dynamic = configure.DynamicSearchByRuleId((int)id);

            if (dynamic == null)
            {
                return HttpNotFound();
            }
            configure.DeleteDynamicRule(dynamic);
            return RedirectToAction("Index");
        }
    }
}