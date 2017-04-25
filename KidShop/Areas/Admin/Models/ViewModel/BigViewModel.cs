using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KidShop.Areas.Admin.Models.DataModel;

namespace KidShop.Areas.Admin.Models.ViewModel
{
    public class BigViewModel
    {
        public Product Product { get; set; }
        public ProductDetail ProductDetail { get; set; }
    }
}