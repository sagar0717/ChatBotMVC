using ChatBotMVC.ViewModels;
using DialogMVC.Business;
using DialogMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ChatBotMVC.Controllers
{
    [Authorize(Roles = "Editor,Approver")]
    public class FixedRulesController : Controller
    {
        //private ConfigureRules rules = new ConfigureRules();
        private Configure configure = new DialogMVC.Business.Configure();
        // GET: FixedRules
        public ActionResult Index(string search)
        {
            if (search == null || search == "")
            {
                return View(configure.LoadFixedrules.ToList());
            }
            else
            {
                List<Fixedrule> FilterRules = configure.SearchFxRuleByStatus(search);
                return View(FilterRules);
            }
        }
        [HttpPost]
        public ActionResult Index(FormCollection form, int? id)
        {
            string strDDLValue = Request.Form["Status"].ToString();
            var fixrule = configure.FixedSearchByRuleId((int)id);
            bool statusUpdated = configure.ApproveRejectRule(fixrule, strDDLValue);
            if (statusUpdated)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Error occured !";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        //Creates a new rule
        [HttpPost]
        public ActionResult Create([Bind(Include = "Query, Response")] Fixedrule fixedrule)
        {
            if (!ModelState.IsValid)
            {
                return View(fixedrule);
            }
            bool fixRuleSave = configure.SaveFixedRule(fixedrule, User.Identity.Name);
            if (fixRuleSave == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "This Query Already Exists.";
                return View();
            }
        }
        //Edits a rule based on id provided
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var fixrule = configure.FixedSearchByRuleId((int)id);
            if (fixrule == null)
            {
                return HttpNotFound();
            }
            return View(fixrule);
        }
        //Edit response to edit button
        [HttpPost]
        public ActionResult Edit(Fixedrule fixrule)
        {
            if (!ModelState.IsValid)
            {
                return View(fixrule);
            }
            bool fixRuleEdit = configure.EditFixedRule(fixrule, User.Identity.Name);
            if (fixRuleEdit)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "This Query Already Exists.";
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
            var fixrule = configure.FixedSearchByRuleId((int)id);
            if (fixrule == null)
            {
                return HttpNotFound();
            }
            return View(fixrule);
        }

        // POST: PersonalContacts/Delete/5
        // Delete a contact
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var fixrule = configure.FixedSearchByRuleId((int)id);

            if (fixrule == null)
            {
                return HttpNotFound();
            }
            bool fixRuleDelete = configure.DeleteFixedRule(fixrule);
            if (fixRuleDelete)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Error Occured !";
                return View();
            }
        }
    }
}