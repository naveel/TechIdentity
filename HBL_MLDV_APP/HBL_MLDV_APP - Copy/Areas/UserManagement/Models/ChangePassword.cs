using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models
{
    public class ChangePassword
    {

        [Required(ErrorMessage = "User is required")]
        public long? userid { get; set; }
        [Required(ErrorMessage = "Old Password is required")]


        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
        public string OldPasswordEncrpt { get; set; }
        //[RegularExpression(@"(^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+{}[]|/<>,.?]).{8,20}$)", ErrorMessage = "Password must be at least 8 characters, no more than 20 characters, and must include at least one upper case letter, one lower case letter, one numeric digit and one special character")]
        [Required(ErrorMessage = "New Password is required")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        //[RegularExpression(@"(^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+{}[]|/<>,.?]).{8,20}$)", ErrorMessage = "Password must be at least 8 characters, no more than 20 characters, and must include at least one upper case letter, one lower case letter, one numeric digit and one special character")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string NewPasswordEncrpt { get; set; }

        public string username { get; set; }
    }
}