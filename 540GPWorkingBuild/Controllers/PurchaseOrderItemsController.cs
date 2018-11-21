﻿using System;
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
    public class PurchaseOrderItemsController : Controller
    {
        private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

        // GET: PurchaseOrderItems
        public ActionResult Index()
        {
            var purchaseOrderItems = db.PurchaseOrderItems.Include(p => p.Inventory).Include(p => p.PurchaseOrder);
            return View(purchaseOrderItems.ToList());
        }

        // GET: PurchaseOrderItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderItem purchaseOrderItem = db.PurchaseOrderItems.Find(id);
            if (purchaseOrderItem == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrderItem);
        }

        // GET: PurchaseOrderItems/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "Name");
            ViewBag.PurchaseOrderID = new SelectList(db.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderID");
            return View();
        }

        // POST: PurchaseOrderItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "POItemID,ProductID,Quantity,Received,PurchaseOrderID")] PurchaseOrderItem purchaseOrderItem)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseOrderItems.Add(purchaseOrderItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "Name", purchaseOrderItem.ProductID);
            ViewBag.PurchaseOrderID = new SelectList(db.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderID", purchaseOrderItem.PurchaseOrderID);
            return View(purchaseOrderItem);
        }

        // GET: PurchaseOrderItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderItem purchaseOrderItem = db.PurchaseOrderItems.Find(id);
            if (purchaseOrderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "Name", purchaseOrderItem.ProductID);
            ViewBag.PurchaseOrderID = new SelectList(db.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderID", purchaseOrderItem.PurchaseOrderID);
            return View(purchaseOrderItem);
        }

        // POST: PurchaseOrderItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "POItemID,ProductID,Quantity,Received,PurchaseOrderID")] PurchaseOrderItem purchaseOrderItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseOrderItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "Name", purchaseOrderItem.ProductID);
            ViewBag.PurchaseOrderID = new SelectList(db.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderID", purchaseOrderItem.PurchaseOrderID);
            return View(purchaseOrderItem);
        }

        // GET: PurchaseOrderItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderItem purchaseOrderItem = db.PurchaseOrderItems.Find(id);
            if (purchaseOrderItem == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrderItem);
        }

        // POST: PurchaseOrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseOrderItem purchaseOrderItem = db.PurchaseOrderItems.Find(id);
            db.PurchaseOrderItems.Remove(purchaseOrderItem);
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
