using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _540GPWorkingBuild.Controllers
{
    public class MainLogInController : Controller
    {
        // GET: MainLogIn
        public ActionResult Index()
        {
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
                        return RedirectToAction("UserDashBoard");
                    }
                }
            }
            return View(e);
        }

        public ActionResult UserDashBoard()
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
    }
}