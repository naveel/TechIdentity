using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Models.Security
{
    public partial class user
    {
        public long id { get; set; }
        [Display(Name ="User Name")]
        [Required(ErrorMessage ="User Name is required")]
        public string username { get; set; }
        public string email { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
        public Nullable<long> employee_id { get; set; }
        public Nullable<long> default_module { get; set; }
        public string user_level { get; set; }
        public Nullable<System.DateTime> last_login { get; set; }
        public Nullable<System.DateTime> last_update { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<System.DateTime> login_hash { get; set; }
        public Nullable<long> lang_id { get; set; }
        public Nullable<int> mt_code { get; set; }
        [Display(Name = "User Full Name")]
        [Required(ErrorMessage = "User Full Name is required")]
        public string user_ful_name { get; set; }
        public string usr_designation { get; set; }
        public string creat_appr_flg { get; set; }
        public Nullable<long> usr_creatd_id { get; set; }
        public Nullable<System.DateTime> del_dte { get; set; }
        public string remarks { get; set; }
        public Nullable<System.DateTime> pswrd_expiry_dte { get; set; }
        public Nullable<long> comp_id { get; set; }
        public Nullable<long> role_id { get; set; }
        public Nullable<int> row_version { get; set; }
        public Nullable<int> record_status { get; set; }
        public Nullable<long> userid { get; set; }
        [Display(Name = "Employee Code")]
        [Required(ErrorMessage = "Employee Code is required")]
        public string Employee_Code { get; set; }

      
    }
}