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
    public class PurchaseOrdersController : Controller
    {

        private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

        // Bridge between PurchaseOrder and PurchaseOrderItems
        public class poWithItems
        {
            public IEnumerable<PurchaseOrderItem> itemList { get; private set; }
            public PurchaseOrder p { get; private set; }

            // Pass a specific Po and entirety of DB's PurchaseOrderItems
            public poWithItems(PurchaseOrder x, IEnumerable<PurchaseOrderItem> y)
            {
                p = x;
                itemList = y;
            }
        }

        // Get a poWithItems object based on a given purchase order ID and db instance
        private poWithItems getOrderWithItems(int givenID, MusciToolkitDBEntities dbInstance)
        {

                var ansPO = db.PurchaseOrders.SingleOrDefault(x => x.PurchaseOrderID == givenID);
                var ansList = db.PurchaseOrderItems.Where(x => x.PurchaseOrderID == givenID);
                // Declare total line price for each PurchaseOrderItem in the list.. Shoutout to Luke!!
                foreach (var each in ansList)
                {
                    double currLineCost = (double)each.Quantity * (double)each.Inventory.NetPrice;
                    each.totalPrice = currLineCost;
                }
                var ans = new poWithItems(ansPO, ansList);
                poTotalSet(ans);
                return ans;

        }

        public void poTotalSet(poWithItems x)
        {
            var allItems = x.itemList;
            double ans = 0;
            foreach(var line in allItems)
            {
                double lineTotal = line.Quantity * (double)line.Inventory.NetPrice;
                ans += lineTotal;
            }
            x.p.totalPrice = ans;
            return;
        }


        

        // GET: PurchaseOrders
        public ActionResult Index()
        {
            return View(db.PurchaseOrders.ToList());
        }

        // GET: PurchaseOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            var x = getOrderWithItems((int)id, db);
            Session["currPo"] = x.p.PurchaseOrderID.ToString();
            return View(x);
        }

        // GET: PurchaseOrders/Create
        public ActionResult Create()
        {
            PurchaseOrder p = new PurchaseOrder();
    
            return Create(p);
        }

        // POST: PurchaseOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PurchaseOrderID,OrderDate")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                purchaseOrder.OrderDate = DateTime.Now;
                db.PurchaseOrders.Add(purchaseOrder);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = purchaseOrder.PurchaseOrderID });
        }

        // GET: PurchaseOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseOrderID,OrderDate")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }





        // RECEIVE PURCHASE ORDER
        public ActionResult Receive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poWithItems entireOrder = getOrderWithItems((int)id, db);
            if (entireOrder == null)
            {
                return HttpNotFound();
            }

            foreach (var line in entireOrder.itemList)
            {
                line.Inventory.Quantity += line.Quantity;
                line.Received = line.Quantity;
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = entireOrder.p.PurchaseOrderID });
        }


        // CANCEL A PURCHASE ORDER (bring it to a state of never having existed)
        public ActionResult Cancel(int? id)
        {
            poWithItems entireOrder = getOrderWithItems((int)id, db);
            foreach (var item in entireOrder.itemList)
            {
                db.PurchaseOrderItems.Remove(item);
            }
            db.PurchaseOrders.Remove(entireOrder.p);
            db.SaveChanges();

            //PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            //db.PurchaseOrders.Remove(purchaseOrder);
            //db.SaveChanges();
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
