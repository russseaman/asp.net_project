using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _540GPWorkingBuild.Models;

namespace _540GPWorkingBuild.Controllers
{
    public class TempReportMonthController : Controller
    {
        private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

        // GET: TempReportMonth
        public ActionResult Index()
        {
            return View(db.ReportMonths.ToList());
        }

        // GET: TempReportMonth/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMonth reportMonth = db.ReportMonths.Find(id);
            if (reportMonth == null)
            {
                return HttpNotFound();
            }
            return View(reportMonth);
        }

        // GET: TempReportMonth/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempReportMonth/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MonthID,Month")] ReportMonth reportMonth)
        {
            if (ModelState.IsValid)
            {
                db.ReportMonths.Add(reportMonth);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportMonth);
        }

        // GET: TempReportMonth/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMonth reportMonth = db.ReportMonths.Find(id);
            if (reportMonth == null)
            {
                return HttpNotFound();
            }
            return View(reportMonth);
        }

        // POST: TempReportMonth/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MonthID,Month")] ReportMonth reportMonth)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportMonth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportMonth);
        }

        // GET: TempReportMonth/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMonth reportMonth = db.ReportMonths.Find(id);
            if (reportMonth == null)
            {
                return HttpNotFound();
            }
            return View(reportMonth);
        }

        // POST: TempReportMonth/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportMonth reportMonth = db.ReportMonths.Find(id);
            db.ReportMonths.Remove(reportMonth);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
