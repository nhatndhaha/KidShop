using KidShop.Areas.Admin.Models.BusinessModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.DataModel
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name="Mã sản phẩm")]
        public int ProductId { get; set; }

        [Display(Name="Tên sản phẩm")]
        [Column(TypeName="nvarchar")]
        [StringLength(256)]
        [Required(ErrorMessage="Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Nội dung")]
        [Column(TypeName = "nvarchar")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Nhóm hàng")]
        public int? CategoryId { get; set; }

        [Display(Name = "Giá")]
        [Required(ErrorMessage="Vui lòng nhập giá sản phẩm")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage="Vui lòng nhập số lượng sản phẩm")]
        public int Qty { get; set; }

        [Display(Name="Ảnh đại diện")]
        public string Image { get; set; }

        [Display(Name = "Khuyến mại")]
        public int Sale { get; set; }

        [Display(Name = "Số lượt xem")]
        [DefaultValue(0)]
        public int View { get; set; }


        [Display(Name = "Ngày tạo")]
        public DateTime? CreateDate { get; set; }

        [Display(Name="Ẩn/Hiện")]
        public bool Status { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}