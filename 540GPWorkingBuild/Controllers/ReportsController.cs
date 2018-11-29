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
    public class ReportsController : Controller
    {
        private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

        // GET: Reports
        public ActionResult Index()
        {
            var reports = db.Reports.Include(r => r.ReportDate).Include(r => r.ReportMonth).Include(r => r.ReportQuarter).Include(r => r.ReportYear);
            return View(reports.ToList());
        }

        // GET: Reports/Details/5
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

        // GET: Reports/Create
        public ActionResult Create()
        {
            ViewBag.ReportDateID = new SelectList(db.ReportDates, "DateID", "Date");
            ViewBag.ReportMonthID = new SelectList(db.ReportMonths, "MonthID", "Month");
            ViewBag.ReportQuarterID = new SelectList(db.ReportQuarters, "QuarterID", "Quarter");
            ViewBag.ReportYearID = new SelectList(db.ReportYears, "YearID", "Year");
            return View();
        }

        // POST: Reports/Create
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

        // GET: Reports/Edit/5
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

        // POST: Reports/Edit/5
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

        // GET: Reports/Delete/5
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

        // POST: Reports/Delete/5
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

        public ActionResult StoreReport()
        {
            ViewBag.ReportDateID = new SelectList(db.ReportDates, "DateID", "Date");
            ViewBag.ReportMonthID = new SelectList(db.ReportMonths, "MonthID", "Month");
            ViewBag.ReportQuarterID = new SelectList(db.ReportQuarters, "QuarterID", "Quarter");
            ViewBag.ReportYearID = new SelectList(db.ReportYears, "YearID", "Year");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StoreReport([Bind(Include = "ReportID,ReportMonthID,ReportQuarterID,ReportDateID,ReportYearID")] Report report)
        {

            

            if (report.ReportMonthID == 13)
            {
                ViewBag.month = "Dec";
            }
            else
            {
                ViewBag.month = "Oops";
            }

            return View("TestPostReport");
        }

        public ActionResult EmployeeReport()
        {
            ViewBag.ReportDateID = new SelectList(db.ReportDates, "DateID", "Date");
            ViewBag.ReportMonthID = new SelectList(db.ReportMonths, "MonthID", "Month");
            ViewBag.ReportQuarterID = new SelectList(db.ReportQuarters, "QuarterID", "Quarter");
            ViewBag.ReportYearID = new SelectList(db.ReportYears, "YearID", "Year");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName");
            return View();
        }

        public ActionResult ProductReport()
        {
            ViewBag.ReportDateID = new SelectList(db.ReportDates, "DateID", "Date");
            ViewBag.ReportMonthID = new SelectList(db.ReportMonths, "MonthID", "Month");
            ViewBag.ReportQuarterID = new SelectList(db.ReportQuarters, "QuarterID", "Quarter");
            ViewBag.ReportYearID = new SelectList(db.ReportYears, "YearID", "Year");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Category1");
            return View();
        }

        public ActionResult TestPostReport()
        {
            return View();
        }

        private void ReportUtil(Report report)
        {

        }
    }
}
