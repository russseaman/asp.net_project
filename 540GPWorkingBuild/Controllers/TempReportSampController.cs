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
    public class TempReportSampController : Controller
    {
        private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

        // GET: TempReportSamp
        public ActionResult Index()
        {
            var reports = db.Reports.Include(r => r.ReportDate).Include(r => r.ReportMonth).Include(r => r.ReportQuarter).Include(r => r.ReportYear);
            return View(reports.ToList());
        }

        // GET: TempReportSamp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: TempReportSamp/Create
        public ActionResult Create()
        {
            ViewBag.ReportDateID = new SelectList(db.ReportDates, "DateID", "Date");
            ViewBag.ReportMonthID = new SelectList(db.ReportMonths, "MonthID", "Month");
            ViewBag.ReportQuarterID = new SelectList(db.ReportQuarters, "QuarterID", "Quarter");
            ViewBag.ReportYearID = new SelectList(db.ReportYears, "YearID", "Year");
            return View();
        }

        // POST: TempReportSamp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportID,ReportMonthID,ReportQuarterID,ReportDateID,ReportYearID")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReportDateID = new SelectList(db.ReportDates, "DateID", "Date", report.ReportDateID);
            ViewBag.ReportMonthID = new SelectList(db.ReportMonths, "MonthID", "Month", report.ReportMonthID);
            ViewBag.ReportQuarterID = new SelectList(db.ReportQuarters, "QuarterID", "Quarter", report.ReportQuarterID);
            ViewBag.ReportYearID = new SelectList(db.ReportYears, "YearID", "Year", report.ReportYearID);
            return View(report);
        }

        // GET: TempReportSamp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReportDateID = new SelectList(db.ReportDates, "DateID", "Date", report.ReportDateID);
            ViewBag.ReportMonthID = new SelectList(db.ReportMonths, "MonthID", "Month", report.ReportMonthID);
            ViewBag.ReportQuarterID = new SelectList(db.ReportQuarters, "QuarterID", "Quarter", report.ReportQuarterID);
            ViewBag.ReportYearID = new SelectList(db.ReportYears, "YearID", "Year", report.ReportYearID);
            return View(report);
        }

        // POST: TempReportSamp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReportID,ReportMonthID,ReportQuarterID,ReportDateID,ReportYearID")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReportDateID = new SelectList(db.ReportDates, "DateID", "Date", report.ReportDateID);
            ViewBag.ReportMonthID = new SelectList(db.ReportMonths, "MonthID", "Month", report.ReportMonthID);
            ViewBag.ReportQuarterID = new SelectList(db.ReportQuarters, "QuarterID", "Quarter", report.ReportQuarterID);
            ViewBag.ReportYearID = new SelectList(db.ReportYears, "YearID", "Year", report.ReportYearID);
            return View(report);
        }

        // GET: TempReportSamp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: TempReportSamp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
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
