using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _540GPWorkingBuild.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.Employee e)
        {
            if (ModelState.IsValid)
            {
                using (Models.MusciToolkitDBEntities db = new Models.MusciToolkitDBEntities())
                {
                    var obj = db.Employees.Where(a => a.EmployeeID.Equals(e.EmployeeID) && a.Password.Equals(e.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.EmployeeID.ToString();
                        Session["UserName"] = obj.Password.ToString();
                        Session["UserRole"] = obj.EmployeeRole.EmpRole.ToString();
                        return RedirectToAction("Menu");
                    }
                }
            }
            return View(e);
        }

        public ActionResult Menu()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult Logout()
        {
            return View();
        }

    } // home controller

} // namespace