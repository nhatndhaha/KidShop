using KidShop.Areas.Admin.Models.BusinessModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [StringLength(64, ErrorMessage = "Tên đăng nhập phải từ 3 - 64 ký tự", MinimumLength = 3)]
        [UniqueUsername]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(64, ErrorMessage = "Mật khẩu phải từ 3 - 64 ký tự", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage="Xác nhận mật khẩu phải trùng với mật khẩu")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Họ tên")]
        [MaxLength(256)]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage="Vui lòng nhập tên tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage="Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }

    public class CreateViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [StringLength(64, ErrorMessage = "Tên đăng nhập phải từ 3 - 64 ký tự", MinimumLength = 3)]
        [UniqueUsername]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(64, ErrorMessage = "Mật khẩu phải từ 3 - 64 ký tự", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Họ tên")]
        [MaxLength(256)]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Display(Name = "Ảnh đại diện")]
        [DataType(DataType.Upload)]
        public string Avatar { get; set; }

        [Display(Name = "Quản trị viên?")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool Allowed { get; set; }
    }
}