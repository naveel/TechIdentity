using HBL_MLDV_APP.Areas.UserManagement.Models.Role;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBL_MLDV_APP.Areas.UserManagement.Models
{
    public class Users
    {
        public long user_sk { get; set; }
        
        [Required(ErrorMessage = "Username is Required")]
        [Display(Name ="Username")]
        [MaxLength(50 ,ErrorMessage = "Username should not be more than 50 Characters")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Email Address is not in correct format")]
        [EmailAddress(ErrorMessage = "Email Address is not in correct format")]
        [Display(Name = "Email Address")]
        [MaxLength(50, ErrorMessage = "Email Address should not be more than 50 Characters")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        [MinLength(8,ErrorMessage ="Password should have 8 alphanumeric characters")]
        [MaxLength(50, ErrorMessage = "Password should not be more than 50 Characters")]
        public string password { get; set; }

        [Required(ErrorMessage = "User Role is Required")]
        [Display(Name = "User Role")]
        public int role_id { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? last_login { get; set; }
        
        public DateTime? last_update  { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime?   created  { get; set; }
        
        //[Required(ErrorMessage = "MT Code is Required")]
        public int mt_code  { get; set; }

        [Required(ErrorMessage = "User Full Name is Required")]
        [Display(Name = "User Full Name")]
        public string user_ful_name { get; set; }

        public string creat_appr_flg  { get; set; }

        public int  usr_creatd_id  { get; set; }

        public DateTime? del_dte  { get; set; }
        
        [MaxLength(50, ErrorMessage = "Remarks should not be more than 100 Characters")]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? pswrd_expiry_dte  { get; set; }
        
        public string state { get; set; }
        public int row_version { get; set; }
        public int record_status { get; set; }
        public int status_sk { get; set; }
        public string sender { get; set; }
        public string mobile_cntry_cde { get; set; }
        [Display(Name = "Mobile No.")]
        public string mobile_nbr { get; set; }
        public string status { get; set; }
    }

    public class vu_users
    {
        public int user_sk { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime? last_login { get; set; }
        public DateTime? updated_on { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_on { get; set; }
        public int mt_code { get; set; }
        public string user_full_name { get; set; }
        public string creat_appr_flg { get; set; }
        public int created_by { get; set; }
        public DateTime? del_dte { get; set; }
        public string remarks { get; set; }
        public DateTime? pswrd_expiry_dte { get; set; }
        public string state { get; set; }
        public int row_version { get; set; }
        public int record_status { get; set; }
        public int status_sk { get; set; }
        public string sender { get; set; }
        public string mobile_cntry_cde { get; set; }
        public string mobile_nbr { get; set; }
        public string status { get; set; }
        public List<vu_user_lvl_can_do> cando { get; set; }
        public string dynmc_url { get; set; }
    }

    public class vu_users_aprv
    {
        public long user_sk { get; set; }
        public string username { get; set; }
        public string user_full_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string mobile_cntry_cde { get; set; }
        public string mobile_nbr { get; set; }
        public DateTime? last_login { get; set; }
        public string creat_appr_flg { get; set; }
        public int mt_code { get; set; }
        public DateTime? del_dte { get; set; }
        public string remarks { get; set; }
        public DateTime? pswrd_expiry_dte { get; set; }
        public int status_sk { get; set; }
        public int created_by { get; set; }
        public DateTime created_on { get; set; }
        public DateTime? updated_on { get; set; }
        public int? updated_by { get; set; }
        public int row_version { get; set; }
        public int record_status { get; set; }
        public string status { get; set; }
        public string state { get; set; }
        public List<vu_user_lvl_can_do_aprv> cando { get; set; }
    }

    public class vu_users_aprv_vm
    {
        public long user_sk { get; set; }

        [Required(ErrorMessage = "Username is Required")]
        [Display(Name = "Username")]
        [MaxLength(50, ErrorMessage = "Username should not be more than 50 Characters")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Address is not in correct format")]
        [EmailAddress(ErrorMessage = "Email Address is not in correct format")]
        [Display(Name = "Email Address")]
        [MaxLength(50, ErrorMessage = "Email Address should not be more than 50 Characters")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        [MinLength(8, ErrorMessage = "Password should have 8 alphanumeric characters")]
        [MaxLength(50, ErrorMessage = "Password should not be more than 50 Characters")]
        public string password { get; set; }

        [Required(ErrorMessage = "User Role is Required")]
        [Display(Name = "User Role")]
        public int[] role_sk { get; set; }

        [DataType(DataType.Date)]
        public DateTime? last_login { get; set; }

        public DateTime? updated_on { get; set; }
        public int? updated_by { get; set; }

        [DataType(DataType.Date)]
        public DateTime? created_on { get; set; }

        //[Required(ErrorMessage = "MT Code is Required")]
        public int mt_code { get; set; }

        [Required(ErrorMessage = "User Full Name is Required")]
        [Display(Name = "User Full Name")]
        public string user_full_name { get; set; }

        public string creat_appr_flg { get; set; }

        public int created_by { get; set; }

        public DateTime? del_dte { get; set; }

        [MaxLength(50, ErrorMessage = "Remarks should not be more than 100 Characters")]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [DataType(DataType.Date)]
        public DateTime? pswrd_expiry_dte { get; set; }

        public string state { get; set; }
        public int row_version { get; set; }
        public int record_status { get; set; }
        public string sender { get; set; }

        [Display(Name = "Mobile No.")]
        [Required(ErrorMessage = "Mobile Code is required.")]
        public string mobile_cntry_cde { get; set; }
        [Required(ErrorMessage = "Mobile No. is required.")]
        public string mobile_nbr { get; set; }

        [Display(Name = "Branch Access")]
        [Required(ErrorMessage = "Branch Access required.")]
        public int[] br_code { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Doc. Status")]
        public int status_sk { get; set; }

        public string status { get; set; }

        public bool status_bool { 
            get 
            {
                return status == "Active" ? true : false;
            } 
            set 
            {
                status = value ? "Active" : "Inactive";
            } 
        }

        [Display(Name = "Branch")]
        public string br_desc { get; set; }

        [Display(Name = "Operator")]
        public string operator_name { get; set; }

        public bool selectAll { get; set; }
        public bool createAll { get; set; }
        public bool updateAll { get; set; }
        public bool deleteAll { get; set; }

        public List<vu_user_lvl_can_do_aprv> cando { get; set; }
        public List<vu_role_lvl_can_do_aprv> role { get; set; }
        public string doc_link { get; set; }
    }
}
