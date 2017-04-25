using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.DataModel
{
    public class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "bắt buộc")]
        [DisplayName("Tên")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "bắt buộc")]
        [DisplayName("Họ")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "bắt buộc")]
        [DisplayName("Địa chỉ")]
        [StringLength(70)]
        public string Address { get; set; }

        [Required(ErrorMessage = "bắt buộc")]
        [DisplayName("Điện thoại")]
        [StringLength(24)]
        public string Phone { get; set; }

        public bool SaveInfo { get; set; }

        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}