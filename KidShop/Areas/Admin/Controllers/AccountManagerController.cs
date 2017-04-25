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
    public class AccountManagerController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();

        public ActionResult Index()
        {
            return View(db.Account.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,FullName,Email,Avatar,IsAdmin,Allowed")] CreateViewModel account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            Account acc = new Account() {
                FirstName = account.Username,
                Password = Common.EncryptMD5(account.Username + account.Password),
                Email = account.Email,
                //Role = account.,
                Allowed = account.Allowed,
                Avatar = "avatardefault.jpg"
            };

            db.Account.Add(acc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Account.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                var entry = db.Entry(account);
                entry.Property(e => e.Email).IsModified = false;
                entry.Property(e => e.Password).IsModified = false;
                if (account.Avatar == null)
                {
                    entry.Property(e => e.Avatar).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        public ActionResult Delete(int id)
        {
            Account account = db.Account.Find(id);
            db.Account.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
