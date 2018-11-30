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
     public class SalesController : Controller
     {
          private MusciToolkitDBEntities db = new MusciToolkitDBEntities();

          public class soWithItems
          {
               public IEnumerable<SaleItem> itemList { get; private set; }
               public Sale s { get; private set; }

               public soWithItems(Sale x, IEnumerable<SaleItem> y)
               {
                    s = x;
                    itemList = y;
               }
          }

          public soWithItems getOrderWithItems(int givenID, MusciToolkitDBEntities dbInstance)
          {
               var ansSO = db.Sales.SingleOrDefault(x => x.SaleID == givenID);
               var ansList = db.SaleItems.Where(x => x.SaleID == givenID);
               int currLineItem = 0;
               foreach (var each in ansList)
               {
                    double currLineCost = each.Quantity * (double)each.Inventory.SalePrice;
                    currLineItem += each.Quantity;
                    each.TotalSIPrice = currLineCost;
                    each.TotalSI = currLineItem;
                    each.Returned = 0;
               }
               db.SaveChanges();
               var ans = new soWithItems(ansSO, ansList);
               soTotalSet(ans);
               db.SaveChanges();
               return ans;
          }

          public void soTotalSet(soWithItems x)
          {
               var allItems = x.itemList;
               double ans = 0;
               int items = 0;
               foreach (var line in allItems)
               {
                    ans += line.TotalSIPrice;
                    items = line.TotalSI;
               }
               x.s.TotalSalePrice = ans;
               x.s.TotalSaleItems = items;
               x.s.Status = 0;
               return;
          }

          public double getTotalSalePrice(int id)
          {
               soWithItems x = getOrderWithItems(id, db);
               return x.s.TotalSalePrice;
          }

          public int getTotalSaleItems(int id)
          {
               soWithItems x = getOrderWithItems(id, db);
               return x.s.TotalSaleItems;
          }

          public string getStatus(int id)
          {
               soWithItems x = getOrderWithItems(id, db);
               return x.s.Status.ToString();
          }

          public ActionResult TransactionLookup()
          {
               List<soWithItems> soListComplete = new List<soWithItems>();
               List<Sale> soList = db.Sales.ToList();

               foreach (var item in soList)
               {
                    int currID = item.SaleID;
                    soWithItems currItem = getOrderWithItems(currID, db);
                    soListComplete.Add(currItem);
               }

               return View(soListComplete.OrderByDescending(x => x.s.SaleID).Take(15).ToList());
          }

          /*public ActionResult CheckOut()
          {
               List<soWithItems> cart = new List<soWithItems>();
               List<SaleItem> saleItemList = db.SaleItems.ToList();

               foreach (var item in saleItemList)
               {
                    int currID = item.Sale.SaleID;
                    soWithItems currItem = getOrderWithItems(currID, db);
                    cart.Add(currItem);
               }

               return View(cart);
          }*/

          public ActionResult Cancel(int? id)
          {
               soWithItems saleOrder = getOrderWithItems((int)id, db);
               foreach (var item in saleOrder.itemList)
               {
                    db.SaleItems.Remove(item);
               }
               db.Sales.Remove(saleOrder.s);
               db.SaveChanges();

               return RedirectToAction("Index");
          }

          // GET: Sales
          public ActionResult Index()
          {
               return View();
          }

          public ActionResult Return()
          {
               return View();
          }

          // GET: Sales/Details/5
          public ActionResult Details(int? id)
          {
               if (id == null)
               {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
               }
               Sale sale = db.Sales.Find(id);
               if (sale == null)
               {
                    return HttpNotFound();
               }
               return View(sale);
          }

          // GET: Sales/NewSale
          public ActionResult NewSale()
          {
               Sale s = new Sale();

               ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName");
               ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName");

               return View(s);
          }

          // POST: Sales/Create
          // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
          // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
          [ValidateAntiForgeryToken]
          [HttpPost]
          public ActionResult NewSale(Sale sale)
          {
               if (ModelState.IsValid)
               {
                    sale.SaleDate = DateTime.Now;
                    db.Sales.Add(sale);
                    db.SaveChanges();
                    Session["Current CustomerID"] = sale.CustomerID;
                    Session["Current SaleID"] = sale.SaleID;
                    return RedirectToAction("Create", "SaleItems");
               }

               ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", sale.CustomerID);
               ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", sale.EmployeeID);
               return View(sale);
          }

          // GET: Sales/Edit/5
          public ActionResult Edit(int? id)
          {
               if (id == null)
               {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
               }
               Sale sale = db.Sales.Find(id);
               if (sale == null)
               {
                    return HttpNotFound();
               }
               ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", sale.CustomerID);
               ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", sale.EmployeeID);
               return View(sale);
          }

          // POST: Sales/Edit/5
          // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
          // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Edit([Bind(Include = "SaleID,CustomerID,EmployeeID,SaleDate")] Sale sale)
          {
               if (ModelState.IsValid)
               {
                    db.Entry(sale).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
               }
               ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", sale.CustomerID);
               ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", sale.EmployeeID);
               return View(sale);
          }

          // GET: Sales/Delete/5
          public ActionResult Delete(int? id)
          {
               if (id == null)
               {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
               }
               Sale sale = db.Sales.Find(id);
               if (sale == null)
               {
                    return HttpNotFound();
               }
               return View(sale);
          }

          // POST: Sales/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public ActionResult DeleteConfirmed(int id)
          {
               Sale sale = db.Sales.Find(id);
               db.Sales.Remove(sale);
               db.SaveChanges();
               return RedirectToAction("TransactionLookup");
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
