using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _540GPWorkingBuild.Models;
using _540GPWorkingBuild.ViewModels;

namespace _540GPWorkingBuild.Controllers
{
     public class SaleDetailsController : Controller
     {
          private MusciToolkitDBEntities db = new MusciToolkitDBEntities();
          // GET: SaleDetails
          public ActionResult TransactionLookupView()
          {
               List<SaleVM> SaleVMList = new List<SaleVM>();
               var saleList = (from sale in db.Sales
                               join saleItem in db.SaleItems on sale.SaleID equals saleItem.SaleID
                               select new { sale.SaleID, sale.CustomerID, sale.EmployeeID, sale.SaleDate, saleItem.Quantity, saleItem.Returned, saleItem.Inventory.SalePrice }).ToList();

               foreach (var item in saleList)
               {
                    SaleVM objsvm = new SaleVM();
                    objsvm.SaleID = item.SaleID;
                    objsvm.CustomerID = item.CustomerID;
                    objsvm.EmployeeID = item.EmployeeID;
                    objsvm.SaleDate = item.SaleDate;
                    objsvm.SIQuantity = item.Quantity;
                    objsvm.Returned = item.Returned;
                    objsvm.TotalPrice = item.Quantity * (double)item.SalePrice;
                    SaleVMList.Add(objsvm);
               }

               return View(SaleVMList.OrderByDescending(x => x.SaleID).Take(5).ToList());
          }

          public ActionResult NewSaleView()
          {
               SaleVM s = new SaleVM();

               ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName");
               ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName");

               return View(s);
          }

          [ValidateAntiForgeryToken]
          [HttpPost]
          public ActionResult NewSaleView([Bind(Include = "SaleID,CustomerID,EmployeeID,SaleDate")] SaleVM saleVM)
          {
               if (ModelState.IsValid)
               {
                    Sale sale = new Sale();
                    sale.SaleID = saleVM.SaleID;
                    sale.CustomerID = saleVM.CustomerID;
                    sale.EmployeeID = saleVM.EmployeeID;
                    saleVM.SaleDate = DateTime.Now;
                    sale.SaleDate = saleVM.SaleDate;
                    db.Sales.Add(sale);
                    db.SaveChanges();
                    Session["Current SaleID"] = sale.SaleID;
                    return RedirectToAction("AddPurchaseView");
               }

               ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", saleVM.CustomerID);
               ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", saleVM.EmployeeID);
               return View(saleVM);
          }

          public ActionResult AddPurchaseView()
          {
               ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID");
               ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID");

               var allSaleItems = db.SaleItems.ToList();
               List<SaleVM> SaleVMList = new List<SaleVM>();

               foreach (SaleItem si in allSaleItems)
               {
                    SaleVM saleVM = new SaleVM();
                    saleVM.SaleID = si.SaleID;
                    saleVM.ProductID = si.ProductID;
                    saleVM.SIQuantity = si.Quantity;
                    saleVM.Returned = si.Returned;
                    saleVM.TotalPrice = si.Quantity * (double)si.Inventory.SalePrice;
                    saleVM.Name = si.Inventory.Name;
                    SaleVMList.Add(saleVM);
               }
              
               return View(SaleVMList);
          }

          [ValidateAntiForgeryToken]
          [HttpPost]
          public ActionResult AddPurchaseView(SaleVM saleVM)
          {
               if (ModelState.IsValid)
               {
                    SaleItem si = new SaleItem();
                    si.SaleID = saleVM.SaleID;
                    si.ProductID = saleVM.ProductID;
                    si.Quantity = saleVM.SIQuantity;
                    si.Returned = saleVM.Returned;
                    db.SaleItems.Add(si);
                    db.SaveChanges();
                    return RedirectToAction("AddPurchaseView");
               }

               ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID", saleVM.ProductID);
               ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID", saleVM.SaleID);
               return View(saleVM);
          }

          public ActionResult Index()
          {
               return View();
          }
     }
}