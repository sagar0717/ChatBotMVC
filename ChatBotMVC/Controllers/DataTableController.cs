using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DialogMVC.Business;
using DialogMVC.Data;

namespace ChatBotMVC.Controllers
{
    [Authorize(Roles = "DataMaintainer")]
    public class DataTableController : Controller
    {
        private ManageData manageData = new DialogMVC.Business.ManageData();

        // GET: DataTable
        public ActionResult Index()
        {
            return View(manageData.LoadData.ToList());
        }


        // GET: DataTable/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DataTable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WeekNumber,Topic")] TopicInformation topicInformation)
        {
            if (!ModelState.IsValid)
            {
                return View(topicInformation);
            }

            bool dataSaved = manageData.SaveData(topicInformation, User.Identity.Name);
            if (dataSaved)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Duplicate Primary Key - Week Number";
                return View(topicInformation);
            }
        }

        // GET: DataTable/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicInformation topicInformation = manageData.FindDataRowById((int)id);
            if (topicInformation == null)
            {
                return HttpNotFound();
            }
            return View(topicInformation);
        }

        // POST: DataTable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WeekNumber,Topic")] TopicInformation topicInformation)
        {
            if (!ModelState.IsValid)
            {
                return View(topicInformation);
            }
            bool dataEdit = manageData.EditData(topicInformation, User.Identity.Name);
            if (dataEdit)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Error occured while editing !";
                return View(topicInformation);
            }
        }

        // GET: DataTable/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicInformation topicInformation = manageData.FindDataRowById((int)id);
            if (topicInformation == null)
            {
                return HttpNotFound();
            }
            return View(topicInformation);
        }

        // POST: DataTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TopicInformation topicInformation = manageData.FindDataRowById((int)id);
            bool dataDelete = manageData.DeleData(topicInformation);
            if (dataDelete)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Rule Cannot be deleted !";
                return View(topicInformation);
            }
        }

        
    }
}
