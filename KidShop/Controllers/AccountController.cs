using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KidShop.Controllers
{
    public class AccountController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();

        private void MigrateShoppingCart(string UserName)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        // GET: /Account/Register
        public ActionResult Register(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        //POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterView register, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string passwordMD5 = KidShop.Areas.Admin.Models.BusinessModel.Common.EncryptMD5(register.Email + register.Password);
                Account acc = new Account { LastName = register.LastName, FirstName = register.FirstName, Email = register.Email, Password = passwordMD5, Role = 50, Allowed = true };
                db.Account.Add(acc);
                db.SaveChanges();
                Session["Email"] = register.Email;
                Session["LastName"] = register.LastName;
                Session["FirstName"] = register.FirstName;
                return RedirectToLocal(returnUrl);
            }
            return View(register);
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginView login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Account acc = db.Account.SingleOrDefault(r => r.Email == login.Email);
                if (acc != null)
                {
                    string passwordMD5 = KidShop.Areas.Admin.Models.BusinessModel.Common.EncryptMD5(login.Email + login.Password);
                    if (acc.Password == passwordMD5)
                    {
                        Session["Email"] = acc.Email;
                        Session["LastName"] = acc.LastName;
                        Session["FirstName"] = acc.FirstName;
                        return RedirectToLocal(returnUrl);
                    }
                    ViewBag.errorPassword = "Sai mật khẩu.";
                }
                else
                {
                    ViewBag.errorEmail = "Email không tồn tại.";
                }
            }
            return View(login);
        }

        public ActionResult Logout()
        {
            Session["Email"] = null;
            Session["FirstName"] = null;
            Session["LastName"] = null;
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}