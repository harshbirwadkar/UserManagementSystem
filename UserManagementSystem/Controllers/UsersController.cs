using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UserManagementSystem.Models;

namespace UserManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private UserManagementDBEntities db = new UserManagementDBEntities();

        // GET: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            // Server-side validation for required fields
            if (string.IsNullOrWhiteSpace(user.Username))
                ModelState.AddModelError("Username", "Username is required.");
            if (string.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError("Password", "Password is required.");
            if (string.IsNullOrWhiteSpace(user.FullName))
                ModelState.AddModelError("FullName", "Full Name is required.");
            if (user.DOB == null)
                ModelState.AddModelError("DOB", "Date of Birth is required.");
            if (string.IsNullOrWhiteSpace(user.Address))
                ModelState.AddModelError("Address", "Address is required.");

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                TempData["Success"] = "Registration successful!";
                return RedirectToAction("Register");
            }
            return View(user);
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            ViewBag.ShowRegister = true;
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and Password are required.");
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                Session["Username"] = user.Username;
                return RedirectToAction("Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }
        }


        // GET: Users/Dashboard
        public ActionResult Dashboard(string search, int page = 1)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login");
            int pageSize = 10;
            var users = db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u => u.Username.Contains(search));
            }
            users = users.OrderBy(u => u.UserId);
            var pagedUsers = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            int totalUsers = users.Count();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalUsers / pageSize);
            ViewBag.Search = search;
            return View(pagedUsers);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login");
            if (id == null) return HttpNotFound();
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "UserId,FullName,DOB,Address")] User user)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                var userInDb = db.Users.Find(id);
                if (userInDb == null) return HttpNotFound();
                userInDb.FullName = user.FullName;
                userInDb.DOB = user.DOB;
                userInDb.Address = user.Address;
                db.SaveChanges();
                TempData["Success"] = "User updated successfully!";
                return RedirectToAction("Dashboard");
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login");
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();
            db.Users.Remove(user);
            db.SaveChanges();
            TempData["Success"] = "User deleted successfully!";
            return RedirectToAction("Dashboard");
        }
    }
}