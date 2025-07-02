using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Redirect to Dashboard if logged in, otherwise to Login
            if (Session["Username"] != null)
                return RedirectToAction("Dashboard", "Users");
            else
                return RedirectToAction("Login", "Users");
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
    }
}