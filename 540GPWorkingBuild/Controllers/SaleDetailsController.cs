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
                               select new { sale.SaleID, sale.CustomerID, sale.EmployeeID, sale.SaleDate, saleItem.Quantity, saleItem.Returned }).ToList();

               foreach (var item in saleList)
               {
                    SaleVM objsvm = new SaleVM();
                    objsvm.SaleID = item.SaleID;
                    objsvm.CustomerID = item.CustomerID;
                    objsvm.EmployeeID = item.EmployeeID;
                    objsvm.SaleDate = item.SaleDate;
                    objsvm.SIQuantity = item.Quantity;
                    objsvm.Returned = item.Returned;
                    SaleVMList.Add(objsvm);
               }

               return View(SaleVMList.OrderByDescending(x => x.SaleID).Take(5).ToList());
          }

          public ActionResult AddPurchaseItem()
          {
               ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID");
               ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID");

               List<SaleVM> SaleVMList = new List<SaleVM>();
               var itemList = (from inv in db.Inventories
                               join saleItem in db.SaleItems on inv.ProductID equals saleItem.ProductID
                               select new { inv.ProductID, inv.Name, saleItem.Quantity, inv.SalePrice }).ToList();

               foreach (var item in itemList)
               {
                    SaleVM saleVM = new SaleVM();
                    saleVM.ProductID = item.ProductID;
                    saleVM.Name = item.Name;
                    saleVM.SIQuantity = item.Quantity;
                    saleVM.SalePrice = item.SalePrice;
                    SaleVMList.Add(saleVM);
               }

               return View(SaleVMList);
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult AddPurchaseItem([Bind(Include = "ProductID,Name,SIQuantity,SalePrice")] SaleVM saleVM)
          {
               if (ModelState.IsValid)
               {
                    db.SaleVMs.Add(saleVM);
                    db.SaveChanges();
                    return RedirectToAction("AddPurchaseItem");
               }

               ViewBag.ProductID = new SelectList(db.Inventories, "ProductID", "ProductID", saleVM.ProductID);
               ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "SaleID", saleVM.SaleID);
               return View(saleVM);
          }
     }
}