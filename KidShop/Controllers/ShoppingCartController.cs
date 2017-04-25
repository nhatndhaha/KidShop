using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace KidShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        KidShopDbContext db = new KidShopDbContext();

        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int qty)
        {
            // Retrieve the item from the database
            var addedItem = db.ProductDetail.Single(item => item.ProductDetailId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            

            // Display the confirmation message
            ShoppingCartRemoveViewModel results;
            if (addedItem == null)
            {
                results = new ShoppingCartRemoveViewModel
                {
                    Message = "Xảy ra lỗi khi thêm sản phẩm vào giỏ hàng",
                    CartTotal = cart.GetTotal().ToString("C0", CultureInfo.CurrentCulture),
                    CartCount = cart.GetCount(),
                };
            }
            else
            {
                int count = cart.AddToCart(addedItem, qty);
                results = new ShoppingCartRemoveViewModel
                {
                    Message = qty + " sản phẩm " + addedItem.Product.ProductName + " mầu " + addedItem.Color + " size " + addedItem.Size + " đã được thêm vào giỏ hàng.",
                    CartTotal = cart.GetTotal().ToString("C0", CultureInfo.CurrentCulture),
                    CartCount = cart.GetCount(),
                    ItemCount = count,
                    DeleteId = id
                };
            }
            return Json(results);

            // Go back to the main store page for more shopping
            // return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            //xóa sản phẩm khỏi giỏ hàng
            cart.RemoveFromCart(id);
            
            var results = new ShoppingCartRemoveViewModel
            {
                Message = "1 sản phẩm đã được xóa khỏi rỏ hàng.",
                CartTotal = cart.GetTotal().ToString("C0", CultureInfo.CurrentCulture),
                CartCount = cart.GetCount(),
                ItemCount = 0,
                DeleteId = id
            };
            return Json(results);
        }

        [HttpPost]
        public ActionResult UpdateFromCart(int productDetailId, int qty)
        {

            var cart = ShoppingCart.GetCart(this.HttpContext);

            var cartItem = db.ProductDetail.Single(item => item.ProductDetailId == productDetailId);

            cart.UpdateCart(cartItem, qty);

            var results = new ShoppingCartRemoveViewModel
            {
                CartTotal = cart.GetTotal().ToString("C0", CultureInfo.CurrentCulture),
                CartCount = cart.GetCount(),
                ItemCount = qty,
                ThanhTien = (qty * (cartItem.Price - (cartItem.Price * cartItem.Product.Sale) / 100)).ToString("C0", CultureInfo.CurrentCulture),
                DeleteId = productDetailId
            };
            return Json(results);
        }


        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}