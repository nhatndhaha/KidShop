using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.ViewModel;

namespace KidShop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();
        static List<string> fileNameTemp = new List<string>();
        public List<HttpPostedFileBase> fileUpload = new List<HttpPostedFileBase>();


        /*--------------------------------INDEX-----------------------------------*/
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var product = db.Product.OrderByDescending(m => m.ProductId).Include(p => p.Category).Skip((page-1)*pageSize).Take(pageSize);
            return View(product.ToList());
        }


        /*--------------------------------CREATE-GET-----------------------------------*/
        public ActionResult Create()
        {
            List<GetCategory> Cate = new List<GetCategory>();
            ViewBag.CategoryId = new SelectList(new GetCategory().getCategory(0, "", Cate), "CategoryId", "CategoryName");
            return View();
        }


        /*--------------------------------CREATE-POST-----------------------------------*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(/*[Bind(Include = "ProductId,ProductName,Description,CategoryId,Price,Qty,Status")]*/ Product product/*, HttpPostedFileBase Image*/)
        {
            // Lấy tổng sản phẩm
            var tags_size = Request.Form["tags_size"].Split(',');
            var tags_color = Request.Form["tags_color"].Split(',');
            int Qty_detail = 0;
            for (int i = 0; i < tags_color.Length; i++)
            {
                for (int j = 0; j < tags_size.Length; j++)
                {
                    if (Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null)
                    {
                        try
                        {
                            Qty_detail += Int16.Parse(Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"]);
                        }
                        catch
                        {
                            ModelState.AddModelError("ProductDetails", "Số lượng nhập ko hợp lệ");
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                product.Qty = (Qty_detail == 0) ? product.Qty : Qty_detail;

                //Lưu ảnh detail sản phẩm vào server
                List<string> listFileName = new List<string>();
                if (Session["fileUpload"] != null)
                {
                    fileUpload = (List<HttpPostedFileBase>)Session["fileUpload"];
                    foreach (var item in fileUpload)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            string extensionFile2 = item.FileName.Substring(item.FileName.LastIndexOf("."));
                            string newfilename2 = Common.EncryptMD5(DateTime.Now.ToBinary().ToString()) + extensionFile2;
                            string path2 = Path.Combine(Server.MapPath("~/Areas/Admin/Content/Images/ProductImages"), newfilename2);
                            item.SaveAs(path2);
                            listFileName.Add(newfilename2);
                        }
                    }
                }
                if (listFileName != null)
                {
                    product.Image = listFileName[0];
                }
                Session["fileUpload"] = null;
                db.Product.Add(product);
                db.SaveChanges();

                //Lấy id sản phẩm
                int lastProductId = db.Product.Max(x => x.ProductId);

                //Lưu ảnh detail sản phẩm vào CSDL
                foreach (var item in listFileName)
                {
                    ProductImage a = new ProductImage();
                    a.ProductId = lastProductId;
                    a.ImageName = item;
                    db.ProductImage.Add(a);
                }
                db.SaveChanges();

                //Lưu chi tiết sản phẩm
                for (int i = 0; i < tags_color.Length; i++)
                {
                    for (int j = 0; j < tags_size.Length; j++)
                    {
                        if (Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null && Request.Form["price_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null)
                        {
                            db.ProductDetail.Add(new ProductDetail()
                            {
                                ProductId = lastProductId,
                                Color = tags_color[i],
                                Size = tags_size[j],
                                Qty = (Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null) ? Int16.Parse(Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"]) : 0,
                                Price = (Request.Form["price_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null) ? Decimal.Parse(Request.Form["price_detail[" + tags_color[i] + "-" + tags_size[j] + "]"]) : 0
                            });
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Session["fileUpload"] = null;
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }


        /*--------------------------------EDIT-GET-----------------------------------*/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Product.Include(x=>x.ProductDetails).FirstOrDefault(x=>x.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            List<GetCategory> Cate = new List<GetCategory>();
            ViewBag.CategoryId = new SelectList(new GetCategory().getCategory(0, "", Cate), "CategoryId", "CategoryName", product.CategoryId);
            
            return View(product);
        }


        /*--------------------------------EDIT-POST-----------------------------------*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(/*[Bind(Include = "ProductId,ProductName,Description,CategoryId,Price,Image,Qty,Status")] */Product product, HttpPostedFileBase Image)
        {
            // Lấy tổng sản phẩm
            var tags_size = Request.Form["tags_size"].Split(',');
            var tags_color = Request.Form["tags_color"].Split(',');
            int Qty_detail = 0;
            for (int i = 0; i < tags_color.Length; i++)
            {
                for (int j = 0; j < tags_size.Length; j++)
                {
                    if (Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null)
                    {
                        try
                        {
                            Qty_detail += Int16.Parse(Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"]);
                        }
                        catch
                        {
                            ModelState.AddModelError("ProductDetails", "Số lượng nhập ko hợp lệ");
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;

                // Xóa chi tiết sản phẩm cũ
                var delete = db.ProductDetail.Where(x => x.ProductId == product.ProductId);
                db.ProductDetail.RemoveRange(delete);
                db.SaveChanges();

                // Lưu chi tiết sản phẩm
                product.Qty = (Qty_detail == 0) ? product.Qty : Qty_detail; 
                for (int i = 0; i < tags_color.Length; i++)
                {
                    for (int j = 0; j < tags_size.Length; j++)
                    {
                        if (Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null && Request.Form["price_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null)
                        {
                            db.ProductDetail.Add(new ProductDetail()
                            {
                                ProductId = product.ProductId,
                                Color = tags_color[i],
                                Size = tags_size[j],
                                Qty = (Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null) ? Int16.Parse(Request.Form["qty_detail[" + tags_color[i] + "-" + tags_size[j] + "]"]) : 0,
                                Price = (Request.Form["price_detail[" + tags_color[i] + "-" + tags_size[j] + "]"] != null) ? Decimal.Parse(Request.Form["price_detail[" + tags_color[i] + "-" + tags_size[j] + "]"]) : 0
                            });
                        }
                    }
                }
                db.SaveChanges();

                //Lưu ảnh detail sản phẩm
                List<string> listFileName = new List<string>();
                if (Session["fileUpload"] != null)
                {
                    fileUpload = (List<HttpPostedFileBase>)Session["fileUpload"];
                    foreach (var item in fileUpload)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            string extensionFile2 = item.FileName.Substring(item.FileName.LastIndexOf("."));
                            string newfilename2 = Common.EncryptMD5(DateTime.Now.ToBinary().ToString()) + extensionFile2;
                            db.ProductImage.Add(new ProductImage()
                            {
                                 ImageName = newfilename2,
                                 ProductId = product.ProductId
                            });
                            string path2 = Path.Combine(Server.MapPath("~/Areas/Admin/Content/Images/ProductImages"), newfilename2);
                            item.SaveAs(path2);
                            listFileName.Add(newfilename2);
                        }
                    }
                }
                Session["fileUpload"] = null;

                //Xóa ảnh detail người dùng xóa
                if (fileNameTemp != null)
                {
                    foreach (var item in fileNameTemp)
                    {
                        var rs = db.ProductImage.FirstOrDefault(x => x.ImageName == item);
                        if (rs != null)
                        {
                            db.ProductImage.Remove(rs);
                        }
                        string fullpath = Request.MapPath("~/Areas/Admin/Content/Images/ProductImages/" + item);
                        if (System.IO.File.Exists(fullpath))
                        {
                            System.IO.File.Delete(fullpath);
                        }
                    }
                }
                db.SaveChanges();

                product.Image = db.ProductImage.Where(x=>x.ProductId == product.ProductId).Select(x=>x.ImageName).FirstOrDefault();

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }


        /*--------------------------------DELETE-----------------------------------*/
        [HttpPost]
        public bool Delete(int id)
        {
            //Xóa file image detail
            var find = db.ProductImage.Where(x => x.ProductId == id).ToList();
            foreach (var item in find)
            {
                string path = Request.MapPath("~/Areas/Admin/Content/Images/ProductImages/" + item.ImageName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            
            //Xóa file image
            Product product = db.Product.Find(id);
            string fullpath = Request.MapPath("~/Areas/Admin/Content/Images/Product/" + product.Image);
            if (System.IO.File.Exists(fullpath))
            {
                System.IO.File.Delete(fullpath);
            }
            db.Product.Remove(product);
            db.SaveChanges();
            return true;
        }


        // UploadFile function
        // Mỗi khi chọn ảnh tại DropzoneJS, function này sẽ đc thực thi
        // và lưu vào Session['fileUpload'], List fileUpload 
        // Khi nào người dùng Submit form thì mới thực lưu file vào CSDL
        [HttpPost]
        public void UploadFile()
        {
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file != null && file.ContentLength > 0)
                {
                    if (Session["fileUpload"] == null)
                    {
                        fileUpload.Add(file);
                    }
                    else
                    {
                        fileUpload = (List<HttpPostedFileBase>)Session["fileUpload"];
                        fileUpload.Add(file);
                    }
                }
            }
            Session["fileUpload"] = fileUpload;
        }


        // Tại View Create nếu người dùng xóa bỏ file image ko muốn upload nữa (DropzoneJS)
        // thì function này sẽ đc thực thi
        // loại bỏ file đó ra khỏi List<HttpPostFileBase> fileUpload, Session['fileUpload']
        public bool DeleteFileImage(string id)
        {
            fileUpload = (List<HttpPostedFileBase>)Session["fileUpload"];
            foreach (var file in fileUpload)
            {
                if (file.FileName == id)
                {
                    fileUpload.Remove(file);
                    break;
                }
            }
            Session["fileUpload"] = fileUpload;
            return true;
        }


        // Tại View Edit nếu người dùng muốn xóa file image (DropzoneJS)
        // Ta sẽ lưu tên file vào List<string> fileNameTemp
        // Khi người dùng submit Lưu thì mới xóa file ra khỏi CSDL, server
        public void DeleteFileImage_Edit(string id)
        {
            fileNameTemp.Add(id);
        }


        // Method này dùng để lấy tất cả tên ảnh detail của ProductId truyền vào
        // Return Json
        // Dùng để hiển thị trong DropzoneJS, View Edit
        public ActionResult GetListImageDetail(int id)
        {
            var model = db.ProductImage.Where(x => x.ProductId == id).ToList();
            return Json(new { Data = model.Select(x => new { FileName = x.ImageName, ProductImageId = x.ProductImageId }) }, JsonRequestBehavior.AllowGet);
        }


        // Method này đc sử dụng tại View Edit và Create
        // Dùng để xóa dữ liệu Session['fileUpload'], List fileUpload khi người dùng chuyển trang
        public void Load()
        {
            fileUpload = null;
            Session["fileUpload"] = null;
        }

        /*************************** PHÂN TRANG SẢN PHẨM ************************/
        // Lấy tổng số trang
        public JsonResult Count(int items)
        {
            int rows = db.Product.Count();
            float temp = (float)rows / items;
            int sum = (temp - (int)temp != 0) ? (int)temp + 1 : (int)temp;
            return Json(new { total = sum}, JsonRequestBehavior.AllowGet);
        }

        // Lấy danh sách sản phẩm
        [HttpPost]
        public JsonResult GetAllProduct(int page = 1, int pageSize = 10)
        {
            var product = db.Product.OrderByDescending(m => m.ProductId).Include(p => p.Category).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                Data = product.Select(x => new
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryName = x.Category.CategoryName,
                    Price = x.Price,
                    Qty = x.Qty,
                    Image = x.Image,
                    Status = x.Status
                })
            }, JsonRequestBehavior.AllowGet);
        }


        // Lấy sản phẩm sau sản phẩm có Id truyền vào
        public JsonResult GetNextProduct(int lastId)
        {
            var rs = db.Product.Include(x => x.Category).OrderByDescending(x => x.ProductId).Where(x => x.ProductId < lastId).Take(1);
            return Json(new
            {
                Data = rs.Select(x => new
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryName = x.Category.CategoryName,
                    Price = x.Price,
                    Qty = x.Qty,
                    Image = x.Image,
                    Status = x.Status
                })
            }, JsonRequestBehavior.AllowGet);
        }


        //  Search product
        public JsonResult SearchProduct(string key)
        {
            var rs = db.Product.Select(x => new { x.ProductId, x.ProductName, x.Price, x.Qty }).Where(w => w.ProductName.Contains(key) || w.ProductId.ToString().Contains(key)).Take(5).ToList();
            return Json(new { Data = rs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductDetail(int id)
        {
            var rs = db.ProductDetail.Select(x => new { x.ProductId, x.Color, x.Size, x.Qty, x.Price }).Where(x => x.ProductId == id);
            return Json( rs, JsonRequestBehavior.AllowGet);
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
