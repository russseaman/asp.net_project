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
