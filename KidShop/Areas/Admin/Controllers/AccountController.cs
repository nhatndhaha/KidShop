using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.ViewModel;

namespace KidShop.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        KidShopDbContext db = new KidShopDbContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel acc)
        {
            if (!ModelState.IsValid)
            {
                return View(acc);
            }
            var rs_user = db.Account.SingleOrDefault(x => x.Email == acc.Username);
            if (rs_user == null)
            {
                ViewBag.Error = "Tên đăng nhập không tồn tại. Vui lòng kiểm tra lại.";
                return View();
            }
            else
            {
                string encryp = Common.EncryptMD5(acc.Username + acc.Password);
                var rs_pass = db.Account.SingleOrDefault(x => x.Password == encryp);
                if (rs_pass == null)
                {
                    ViewBag.Error = "Mật khẩu không chính xác. Vui lòng kiểm tra lại.";
                    return View();
                }
                else
                {
                    if (!rs_user.Allowed)
                    {
                        ViewBag.Error = "Rất tiếc. Tài khoản này đã bị khóa. Liên hệ quản trị viên để biết thêm chi tiết!";
                        return View();
                    }
                    Session["UserId"] = rs_user.UserId;
                    Session["Avatar"] = rs_user.Avatar;
                    return RedirectToAction("Index", "Product");
                }
            }
            Session["UserId"] = rs_user.UserId;
            Session["Avatar"] = rs_user.Avatar;
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["Username"] = null;
            Session["FullName"] = null;
            Session["Avatar"] = null;
            return RedirectToAction("Login");
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel acc)
        {
            if (ModelState.IsValid)
            {
                //Account temp = new Account()
                //{
                //    Username = acc.Username,
                //    Password = Common.EncryptMD5(acc.Username + acc.Password),
                //    Email = acc.Email,
                //    FullName = acc.FullName,
                //    Avatar = "avatardefault.jpg",
                //    isAdmin = false,
                //    Allowed = true
                //};
                //db.Account.Add(temp);
                //db.SaveChanges();
                //TempData["RegisterSuccess"] = "Đăng ký thành công. Vui lòng đăng nhập để tiếp tục!";
                //return RedirectToAction("Login", "Account");
            }
            return View(acc);
        }
    }
}
