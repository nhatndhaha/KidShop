using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace KidShop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        KidShopDbContext db = new KidShopDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public List<HttpPostedFileBase> fileUpload = new List<HttpPostedFileBase>();

        [HttpPost]
        public void UploadFile()
        {
            string fName = "";
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                fName = file.FileName;
                var fileName1 = Path.GetFileName(file.FileName);
                if (file != null && file.ContentLength > 0)
                {
                    //model.fileUpload.Add(file);
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

        public bool DeleteFileImage(string id)
        {
            //Xóa file image
            string fullpath = Request.MapPath("~/Areas/Admin/Content/Images/ProductImages/" + id);
            if (System.IO.File.Exists(fullpath))
            {
                System.IO.File.Delete(fullpath);
            }

            return true;
        }
    }
}