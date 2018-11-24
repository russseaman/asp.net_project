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
     }
}