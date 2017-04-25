using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.DataModel
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [StringLength(256)]
        [Display(Name = "Họ")]
        [Column(TypeName = "nvarchar")]
        public string LastName { get; set; }


        [StringLength(256)]
        [Display(Name = "Tên")]
        [Column(TypeName = "nvarchar")]
        public string FirstName { get; set; }


        [Display(Name="Email")]
        [Column(TypeName = "varchar")]
        [StringLength(256)]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(64)]
        [Display(Name="Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ảnh đại diện")]
        [Column(TypeName = "varchar")]
        [StringLength(500)]
        public string Avatar { get; set; }

        [Display(Name="Quản trị viên")]
        public int Role { get; set; }

        [Display(Name="Kích hoạt")]
        public bool Allowed { get; set; }
    }
}