﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}