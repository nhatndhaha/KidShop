using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.BusinessModel;

namespace KidShop.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();

        public ActionResult Index()
        {
            return View(db.Category.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<GetCategory> Cate = new List<GetCategory>();
            ViewBag.ParentID = new SelectList(new GetCategory().getCategory(0, "", Cate), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,Alias,Description,Order,status,ParentId,CreateDate")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.ParentId = (category.ParentId == null) ? 0 : category.ParentId;
                category.CreateDate = DateTime.Now;
                category.Alias = ConvertToAlias.ConvertTitle(category.CategoryName);
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            List<GetCategory> Cate = new List<GetCategory>();
            ViewBag.ParentId = new SelectList(new GetCategory().getCategory(0, "", Cate), "CategoryId", "CategoryName");

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.ParentId = (category.ParentId == null) ? 0 : category.ParentId;
                category.CreateDate = DateTime.Now;
                category.Alias = ConvertToAlias.ConvertTitle(category.CategoryName);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(int id)
        {
            Category category = db.Category.Find(id);
            var cate = db.Category.Where(x => x.ParentId == id).ToList();
            foreach (var item in cate)
            {
                db.Category.Remove(item);
            }
            db.Category.Remove(category);
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
