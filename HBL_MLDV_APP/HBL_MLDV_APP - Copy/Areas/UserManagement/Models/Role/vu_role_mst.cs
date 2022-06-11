using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models.Role
{
    public class vu_role_mst
    {
        public int role_sk { get; set; }
        [Display(Name = "Role Name*")]
        [Required(ErrorMessage = "This field is required")]
        public string role_desc { get; set; }
        public int status_sk { get; set; }
        public int record_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_on { get; set; }
        public int row_version { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
        public List<vu_role_lvl_can_do> role_lvl { get; set; }
        public string sender { get; set; }
        [Display(Name = "Status")]
        public bool status_check { get; set; }
    }
}