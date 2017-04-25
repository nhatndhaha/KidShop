using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.ViewModel
{
    public class ColorSizeViewModel
    {
        public List<string> listSize { get; set; }
        public int productDetailId { get; set; }
        public string OldPrice { get; set; }
        public string SalePrime { get; set; }
        public string StatusQty { get; set; }
    }
}