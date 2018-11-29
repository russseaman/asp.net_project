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
    public class TempReportQuarterController : Controller
    {
        private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

        // GET: TempReportQuarter
        public ActionResult Index()
        {
            return View(db.ReportQuarters.ToList());
        }

        // GET: TempReportQuarter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportQuarter reportQuarter = db.ReportQuarters.Find(id);
            if (reportQuarter == null)
            {
                return HttpNotFound();
            }
            return View(reportQuarter);
        }

        // GET: TempReportQuarter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempReportQuarter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuarterID,Quarter")] ReportQuarter reportQuarter)
        {
            if (ModelState.IsValid)
            {
                db.ReportQuarters.Add(reportQuarter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportQuarter);
        }

        // GET: TempReportQuarter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportQuarter reportQuarter = db.ReportQuarters.Find(id);
            if (reportQuarter == null)
            {
                return HttpNotFound();
            }
            return View(reportQuarter);
        }

        // POST: TempReportQuarter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuarterID,Quarter")] ReportQuarter reportQuarter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportQuarter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportQuarter);
        }

        // GET: TempReportQuarter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportQuarter reportQuarter = db.ReportQuarters.Find(id);
            if (reportQuarter == null)
            {
                return HttpNotFound();
            }
            return View(reportQuarter);
        }

        // POST: TempReportQuarter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportQuarter reportQuarter = db.ReportQuarters.Find(id);
            db.ReportQuarters.Remove(reportQuarter);
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
