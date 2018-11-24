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
    public class SaleItemsController : Controller
    {
        private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

        // GET: SaleItems
        public ActionResult Index()
        {
            var saleItems = db.SaleItems.Include(s => s.Inventory).Include(s => s.Sale);
            return View(saleItems.ToList());
        }

        // GET: SaleItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleItem saleItem = db.SaleItems.Find(id);
            if (saleItem == null)
            {
                return HttpNotFound();
            }
            return View(saleItem);
        }

        // GET: SaleItems/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID");
            ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID");
            return View();
        }

        // POST: SaleItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleItemID,ProductID,Quantity,Returned,SaleID")] SaleItem saleItem)
        {
            if (ModelState.IsValid)
            {
                db.SaleItems.Add(saleItem);
                db.SaveChanges();
                return RedirectToAction("TransactionLookupView", "SaleDetails");
            }

            ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID", saleItem.ProductID);
            ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID", saleItem.SaleID);
            return View(saleItem);
        }

        // GET: SaleItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleItem saleItem = db.SaleItems.Find(id);
            if (saleItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID", saleItem.ProductID);
            ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID", saleItem.SaleID);
            return View(saleItem);
        }

        // POST: SaleItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleItemID,ProductID,Quantity,Returned,SaleID")] SaleItem saleItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID", saleItem.ProductID);
            ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID", saleItem.SaleID);
            return View(saleItem);
        }

        // GET: SaleItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleItem saleItem = db.SaleItems.Find(id);
            if (saleItem == null)
            {
                return HttpNotFound();
            }
            return View(saleItem);
        }

        // POST: SaleItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SaleItem saleItem = db.SaleItems.Find(id);
            db.SaleItems.Remove(saleItem);
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
