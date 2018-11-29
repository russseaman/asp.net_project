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
    public class TempReportYearController : Controller
    {
        private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

        // GET: TempReportYear
        public ActionResult Index()
        {
            return View(db.ReportYears.ToList());
        }

        // GET: TempReportYear/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportYear reportYear = db.ReportYears.Find(id);
            if (reportYear == null)
            {
                return HttpNotFound();
            }
            return View(reportYear);
        }

        // GET: TempReportYear/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempReportYear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YearID,Year")] ReportYear reportYear)
        {
            if (ModelState.IsValid)
            {
                db.ReportYears.Add(reportYear);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportYear);
        }

        // GET: TempReportYear/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportYear reportYear = db.ReportYears.Find(id);
            if (reportYear == null)
            {
                return HttpNotFound();
            }
            return View(reportYear);
        }

        // POST: TempReportYear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YearID,Year")] ReportYear reportYear)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportYear).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportYear);
        }

        // GET: TempReportYear/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportYear reportYear = db.ReportYears.Find(id);
            if (reportYear == null)
            {
                return HttpNotFound();
            }
            return View(reportYear);
        }

        // POST: TempReportYear/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportYear reportYear = db.ReportYears.Find(id);
            db.ReportYears.Remove(reportYear);
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
