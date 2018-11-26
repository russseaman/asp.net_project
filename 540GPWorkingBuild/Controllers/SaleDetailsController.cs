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

          public class transactionList
          {
               public IEnumerable<SaleItem> itemList { get; private set; }
               public Sale s { get; private set; }

               public transactionList(Sale sale, IEnumerable<SaleItem> saleItemList)
               {
                    s = sale;
                    itemList = saleItemList;
               }
          }

          private transactionList getTransactionList(int givenID, MusciToolkitDBEntities dbInstance)
          {
               var sale = db.Sales.SingleOrDefault(x => x.SaleID == givenID);
               var saleItemList = db.SaleItems.Where(x => x.SaleID == givenID);
               var transaction = new transactionList(sale, saleItemList);
               transactionTotalSet(transaction);
               return transaction;
          }

          public void transactionTotalSet(transactionList x)
          {
               var allItems = x.itemList;
               double totalPrice = 0;
               int totalItems = 0;
               foreach (var item in allItems)
               {
                    double price = item.Quantity * (double)item.Inventory.SalePrice;
                    totalPrice += price;
                    totalItems += item.Quantity;
               }
               return;
          }


          /*public void updateSaleVM(SaleVM saleVM)
          {
               List<SaleItem> SaleItemList = new List<SaleItem>();
               var allSaleItems = db.SaleItems.ToList();
               foreach (var saleItem in allSaleItems)
               {
                    if (saleItem.SaleID == Int32.Parse(Session["Current SaleID"].ToString()))
                    {
                         SaleItemList.Add(saleItem);
                         saleVM.Returned = saleItem.Returned;
                    }
               }

               int totalItems = 0;
               double totalPrice = 0;

               foreach (var item in SaleItemList)
               {
                    totalItems += item.Quantity;
                    totalPrice += (item.Quantity * (double)item.Inventory.SalePrice);
                    //saleVM.TotalItems = totalItems;
                    //saleVM.TotalPrice = totalPrice;
               }

               saleVM.TotalItems = totalItems;
               saleVM.TotalPrice = totalPrice;
               return;
          }*/

          public void updateSaleVM(SaleVM saleVM)
          {
               List<SaleItem> SaleItemList = new List<SaleItem>();
               var allSaleItems = db.SaleItems.ToList();
               var saleItemList = db.SaleItems.Where(x => x.SaleID == Int32.Parse(Session["Current SaleID"].ToString()));
               foreach (var saleItem in allSaleItems)
               {
                    if (saleItem.SaleID == Int32.Parse(Session["Current SaleID"].ToString()))
                    {
                         SaleItemList.Add(saleItem);
                         saleVM.Returned = saleItem.Returned;
                    }
               }

               int totalItems = 0;
               double totalPrice = 0;

               foreach (var item in SaleItemList)
               {
                    totalItems = item.Quantity;
                    totalPrice = (item.Quantity * (double)item.Inventory.SalePrice);
                    //saleVM.TotalItems = totalItems;
                    //saleVM.TotalPrice = totalPrice;
               }

               saleVM.TotalSaleItems = totalItems;
               saleVM.TotalSalePrice = totalPrice;
               return;
          }

          // GET: SaleDetails
          public ActionResult TransactionLookupView()
          {
               /*List<SaleVM> SaleVMList = new List<SaleVM>();
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
                    objsvm.TotalSalePrice = item.Quantity * (double)item.SalePrice;
                    SaleVMList.Add(objsvm);
               }

               return View(SaleVMList.OrderByDescending(x => x.SaleID).Take(5).ToList());*/

               List<SaleVM> SaleVMList = new List<SaleVM>();
               var allSales = db.Sales.ToList();
               var allSaleItems = db.SaleItems.ToList();
               foreach (Sale s in allSales)
               {
                    SaleVM saleVM = new SaleVM();
                    saleVM.SaleID = s.SaleID;
                    saleVM.CustomerID = s.CustomerID;
                    saleVM.EmployeeID = s.EmployeeID;
                    saleVM.SaleDate = s.SaleDate;
                    updateSaleVM(saleVM);
                    SaleVMList.Add(saleVM);
               }

               return View(SaleVMList.OrderByDescending(x => x.SaleID).Take(10).ToList());
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
          public ActionResult NewSaleView(SaleVM saleVM)
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
               var allSaleItems = db.SaleItems.ToList();
               List<SaleVM> SaleVMList = new List<SaleVM>();

               foreach (SaleItem si in allSaleItems)
               {
                    SaleVM saleVM = new SaleVM();
                    saleVM.CustomerID = si.Sale.CustomerID;
                    //saleVM.TotalSaleItems = 0;
                    //saleVM.TotalSalePrice = 0;
                    saleVM.SaleID = si.Sale.SaleID; // changed
                    saleVM.ProductID = si.ProductID;
                    saleVM.SIQuantity = si.Quantity;
                    saleVM.Returned = si.Returned;
                    saleVM.TotalSIPrice = si.Quantity * (double)si.Inventory.SalePrice;
                    //saleVM.TotalSalePrice += saleVM.TotalSIPrice;
                    //saleVM.TotalSI += saleVM.SIQuantity;
                    //saleVM.TotalSaleItems += saleVM.TotalSI;
                    saleVM.Name = si.Inventory.Name;
                    SaleVMList.Add(saleVM);
               }

               ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID");
               ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID");

               return View(SaleVMList);
          }

          [ValidateAntiForgeryToken]
          [HttpPost]
          public ActionResult AddPurchaseView(SaleVM saleVM)
          {
               if (ModelState.IsValid)
               {
                    SaleItem si = new SaleItem();
                    Sale s;
                    s = db.Sales.Find(Int32.Parse(Session["Current SaleID"].ToString()));
                    si.Sale = s;
                    si.SaleID = saleVM.SaleID;
                    si.ProductID = saleVM.ProductID;
                    si.Quantity = saleVM.SIQuantity;
                    si.Returned = saleVM.Returned;
                    db.SaleItems.Add(si);
                    db.SaveChanges();
                    return RedirectToAction("AddPurchaseView", new { id = s.SaleID.ToString() });
               }

               ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID", saleVM.ProductID);
               ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID", saleVM.SaleID);
               return View(saleVM);
          }

          public ActionResult CheckoutView()
          {
               return View();
          }

          public ActionResult Index()
          {
               return View();
          }

          public ActionResult ReturnTransactionView()
          {
               return View();
          }

          [ValidateAntiForgeryToken]
          [HttpPost]
          public ActionResult ReturnTransactionView(SaleVM saleVM)
          {
               return View();
          }

          public ActionResult ReturnItemView()
          {
               SaleVM s = new SaleVM();
               return View(s);
          }

          [ValidateAntiForgeryToken]
          [HttpPost]
          public ActionResult ReturnItemView(SaleVM saleVM)
          {

               return View();
          }
     }
}