using KidShop.Areas.Admin.Models.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.ViewModel;

namespace KidShop.Controllers
{
    public class CheckoutController : Controller
    {
        KidShopDbContext db = new KidShopDbContext();
        

        public ActionResult AddressAndPayment()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            Order order = new Order();

            order.Email = Session["Email"].ToString();
            order.FirstName = Session["FirstName"].ToString();
            order.LastName = Session["LastName"].ToString();

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount()
            };
            ViewBag.Cart = viewModel;
            return View(order);
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(Order order)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (ModelState.IsValid)
            {
                order.Username = Session["Email"].ToString();
                order.OrderDate = DateTime.Now;

                //Save Order
                db.Orders.Add(order);
                db.SaveChanges();

                //Process the order
                
                cart.CreateOrder(order);

                return RedirectToAction("Complete", new { id = order.OrderId });
            }
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            ViewBag.CountCart = cart.GetCount();
            ViewBag.Cart = viewModel;
            return View(order);
        }

        //
        // GET: /Checkout/Complete

        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = db.Orders.Any(
                o => o.OrderId == id);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}