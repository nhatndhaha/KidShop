using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.DataModel
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Display(Name="Nhóm hàng")]
        [Column(TypeName="nvarchar")]
        [StringLength(150)]
        public string CategoryName { get; set; }

        [Column(TypeName="varchar")]
        [StringLength(150)]
        public string Alias { get; set; }

        [Display(Name = "Mô tả")]
        [Column(TypeName = "nvarchar")]
        [StringLength(300)]
        public string Description { get; set; }

        [Display(Name = "Thứ tự")]
        public int? Order { get; set; }

        [Display(Name = "Ẩn/Hiện")]
        public bool Status { get; set; }

        [ForeignKey("Parent")]
        [Display(Name="Nhóm hàng cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString="{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? CreateDate { get; set; }

        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Child { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}