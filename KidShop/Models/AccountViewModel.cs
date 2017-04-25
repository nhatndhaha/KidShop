using KidShop.Areas.Admin.Models.BusinessModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidShop.Models
{
    public class RegisterView
    {
        
        [Required(ErrorMessage = "Họ bắt buộc")]
        [Display(Name = "Họ")]
        [MaxLength(256)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Tên bắt buộc")]
        [Display(Name = "Tên")]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [EmailValidation]
        [Display(Name = "Email")]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(64, ErrorMessage = "Mật khẩu phải từ 6 - 64 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class LoginView
    {
        [Required(ErrorMessage = "Email bắt buộc")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}