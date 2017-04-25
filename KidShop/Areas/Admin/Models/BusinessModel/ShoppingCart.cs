using KidShop.Areas.Admin.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidShop.Areas.Admin.Models.BusinessModel
{
    public class ShoppingCart
    {
        KidShopDbContext db = new KidShopDbContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public int AddToCart(ProductDetail product, int qty)
        {
            var cartItem = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductDetailId == product.ProductDetailId);
   
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductDetailId = product.ProductDetailId,
                    CartId = ShoppingCartId,
                    Count = qty,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count+=qty;
            }
            db.SaveChanges();

            return cartItem.Count;
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.Single(cart => cart.CartId == ShoppingCartId && cart.ProductDetailId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);
                db.SaveChanges();
            }

            return itemCount;
        }

        public int UpdateCart(ProductDetail product, int qty)
        {
            if (qty == 0)
            {
                //xoa san pham khoi gio hang
                RemoveFromCart(product.ProductDetailId);
            }
            else
            {
                //thay doi so luong san pham trong gio hang
                var cartItem = db.Carts.Single(cart => cart.CartId == ShoppingCartId && cart.ProductDetailId == product.ProductDetailId);
                if (cartItem == null)
                {
                    //san pham chua co trong gio hang
                    //them san pham vao bang cart
                    cartItem = new Cart
                    {
                        ProductDetailId = product.ProductDetailId,
                        CartId = ShoppingCartId,
                        Count = qty,
                        DateCreated = DateTime.Now
                    };
                    db.Carts.Add(cartItem);
                }
                else
                {
                    //thay doi so luong san pham
                    cartItem.Count = qty;
                }
                db.SaveChanges();
            }
            return qty;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * (cartItems.ProductDetail.Price - (cartItems.ProductDetail.Price * cartItems.ProductDetail.Product.Sale/100))).Sum();
            return total ?? decimal.Zero;
        }

        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            order.OrderDetails = new List<OrderDetail>();

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductDetailId = item.ProductDetailId,
                    OrderId = order.OrderId,
                    UnitPrice = item.ProductDetail.Price,
                    Quantity = item.Count
                };
                orderTotal += (item.Count * item.ProductDetail.Price);
                order.OrderDetails.Add(orderDetail);
                db.OrderDetails.Add(orderDetail);
            }
            order.Total = orderTotal;

            // Save the order
            db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (context.Session["Email"] != null)
                {
                    context.Session[CartSessionKey] = context.Session["Email"];
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }

            db.SaveChanges();
        }
    }
}