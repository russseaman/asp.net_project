using _540GPWorkingBuild.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _540GPWorkingBuild.ViewModels
{
     public class SaleVM
     {
          [Key]
          [Display(Name ="Transaction ID")]
          public int SaleID { get; set; } // Sale
          [Display(Name ="Customer ID")]
          public Nullable<int> CustomerID { get; set; } // Sale
          [Display(Name ="Employee ID")]
          public int EmployeeID { get; set; } // Sale
          [Display(Name ="Sale Date")]
          public System.DateTime SaleDate { get; set; } // Sale
          [Display(Name ="Items")]
          public int SIQuantity { get; set; } // SaleItem
          [Display(Name ="Status")]
          public int Returned { get; set; } // SaleItem
          public int ProductID { get; set; } // SaleItem
          public decimal SalePrice { get; set; } // Inventory
          public int IQuantity { get; set; } // Inventory
          public string Name { get; set; } // Inventory

          public double TotalPrice { get; set; }
          public int TotalItems { get; set; }

          //public double TotalPrice { get { return (double)SIQuantity * (double)SalePrice; } }
     }
}