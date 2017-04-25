using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.DataModel
{
    [Table("ProductImage")]
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ProductImageId { get; set; }

        [Display(Name="Sản phẩm")]
        public int? ProductId { get; set; }

        [Display(Name="Hình ảnh")]
        public string ImageName { get; set; }

        public virtual Product Product { get; set; }
    }
}