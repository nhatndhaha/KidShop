using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;


namespace KidShop.Controllers
{
    public class ProductController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();
        // GET: Product
        public ActionResult Index(int? id, string searchString, string sortOrder, int page = 1)
        {
            int categoryId = id ?? 0;
            ViewBag.searchString = searchString;
            ViewBag.sortOrder = sortOrder ?? "";
            Category category = new Category();
            var products = from s in db.Product select s;
            if (categoryId == 0)
            {
                category.CategoryId = 0;
                if (String.IsNullOrEmpty(searchString))
                {
                    category.CategoryName = "Tất cả sản phẩm";
                }
                else
                {
                    category.CategoryName = "Kết quả tìm kiếm cho từ khóa '" + searchString + "'";
                    products = products.Where(p => p.ProductName.ToLower().Contains(searchString));
                }
            }
            else
            {
                category = db.Category.Find(categoryId);
                if (category == null)
                {
                    return HttpNotFound();
                }
                //list tất cả các mã danh mục có mã cha là categoryId
                List<int> categoryIds = KidShop.Controllers.Common.findChild(categoryId);
                categoryIds.Add(categoryId);
                //tìm tất cả sản phẩm có thuộc danh mục có cha là categoryId
                products = products.Where(s => categoryIds.Contains((int)s.CategoryId));
            }
            ViewBag.Category = category;

            switch (sortOrder)
            {
                case "price":
                    products = products.OrderBy(s => s.Price);
                    break;

                case "price-desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;

                case "name":
                    products = products.OrderBy(s => s.ProductName);
                    break;
                case "name-desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;

                case "date":
                    products = products.OrderBy(s => s.CreateDate);
                    break;

                default:  // Date ascending 
                    products = products.OrderByDescending(s => s.CreateDate);
                    break;
            }
            int pageSize = 12;
            return View(products.ToPagedList(page, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductDetail productDetail = db.ProductDetail.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }

            productDetail.Product.View++;
            db.SaveChanges(); 

            //get list size and color from productDetailId
            List<string> listColor = db.ProductDetail.Where(r => r.ProductId == productDetail.ProductId).Select(r => r.Color).Distinct().ToList();
            List<string> listSize = db.ProductDetail.Where(r => r.ProductId == productDetail.ProductId && r.Color == productDetail.Color).Select(r => r.Size).Distinct().ToList();
            ViewBag.ListColor = listColor;
            ViewBag.listSize = listSize;

            //get sản phẩm liên quan
            var ListProduct = db.Product.Where(r => r.CategoryId == productDetail.Product.CategoryId && r.ProductId != productDetail.ProductId).Take(4).ToList();
            ViewBag.ListProduct = ListProduct;

            return View(productDetail);
        }

        [HttpPost]
        public ActionResult ChangeColor(int id, string color)
        {
            List<ProductDetail> list = db.ProductDetail.Where(r => r.ProductId == id && r.Color == color).ToList();
            if (list.Count() != 0)
            {
                var productDetail = list.FirstOrDefault();
                var results = new ColorSizeViewModel
                {
                    listSize = list.Select(r => r.Size).Distinct().ToList(),
                    productDetailId = productDetail.ProductDetailId,
                    OldPrice = productDetail.Price.ToString("C0", CultureInfo.CurrentCulture),
                    SalePrime = (productDetail.Product.Sale > 0) ? (productDetail.Price - (productDetail.Price * productDetail.Product.Sale / 100)).ToString("C0", CultureInfo.CurrentCulture) : productDetail.Price.ToString("C0", CultureInfo.CurrentCulture),
                    StatusQty = (productDetail.Qty > 0) ? "Còn hàng" : "Hết hàng"
                };
                return Json(results);
            }
            return null;
        }

        [HttpPost]
        public ActionResult ChangeSize(int id, string color, string size)
        {
            ProductDetail productDetail = db.ProductDetail.FirstOrDefault(r => r.ProductId == id && r.Color == color && r.Size == size);
            if (productDetail != null)
            {
                var results = new ColorSizeViewModel
                {
                    productDetailId = productDetail.ProductDetailId,
                    OldPrice = productDetail.Price.ToString("C0", CultureInfo.CurrentCulture),
                    SalePrime = (productDetail.Product.Sale > 0) ? (productDetail.Price - (productDetail.Price * productDetail.Product.Sale / 100)).ToString("C0", CultureInfo.CurrentCulture) : productDetail.Price.ToString("C0", CultureInfo.CurrentCulture),
                    StatusQty = (productDetail.Qty > 0) ? "Còn hàng" : "Hết hàng"
                };
                return Json(results);
            }
            return null;
        }

        

        public PartialViewResult PartialProductUrl(int? id = 0, int? product = 0)
        {
            List<Category> list = new List<Category>();
            if (id == 0)
            {
                ViewBag.tt = "Tất cả sản phẩm";
                return PartialView(list);
            }
            
            if (product == 0)
            {
                Category c = db.Category.Find(id);
                ViewBag.tt = c.CategoryName;
                id = c.ParentId ?? 0;
            }
            else
            {
                Product p = db.Product.Find(id);
                if (p != null)
                {
                    ViewBag.tt = p.ProductName;
                    id = p.CategoryId;
                }
            }

            while (true)
            {
                Category c = db.Category.Find(id);
                if (c != null)
                {
                    list.Add(c);
                    id = c.ParentId ?? 0;
                }
                else
                {
                    break;
                }
            }
            
            return PartialView(list);
        }

        public PartialViewResult PartialCategory(int? id = 0)
        {
            ViewBag.Category = id;
            if (id == 0)
            {
                return PartialView(db.Category.Where(r=>r.ParentId == null).ToList());
            }

            //kiem tra xem danh muc co ton tai khong
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return null;
            }

            //tim tat ca cac danh muc cha
            List<Category> listUrl = new List<Category>();
            listUrl.Add(category);
            while (true)
            {
                if (category.Parent != null)
                {
                    category = category.Parent;
                    listUrl.Add(category);
                }
                else
                {
                    break;
                }
            }

            //
            List<Category> categorys = db.Category.Where(r => r.Parent == null).ToList();
            string space = "";
            for (int i = listUrl.Count() - 1; i >= 0; i--)
            {
                space += "--";
                List<Category> list = listUrl[i].Child.ToList();
                foreach (var item in list)
                {
                    item.CategoryName = space + " " + item.CategoryName;
                }
                categorys.InsertRange(categorys.IndexOf(listUrl[i]) + 1, list);
            }
            return PartialView(categorys);
        }

        public PartialViewResult PartialProductImage(int? id)
        {
            int productId = id ?? 0;
            var image = db.ProductImage.Where(r => r.ProductId == productId);
            return PartialView(image.ToList());
        }

        
    }
}