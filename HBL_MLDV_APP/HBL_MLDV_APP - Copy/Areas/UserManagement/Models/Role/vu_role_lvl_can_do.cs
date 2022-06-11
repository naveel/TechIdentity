using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models.Role
{
    public class vu_role_lvl_can_do
    {
        public int role_cando_sk { get; set; }
        public int activityid { get; set; }
        public int can_view { get; set; }
        public int can_add { get; set; }
        public int can_edit { get; set; }
        public int can_del { get; set; }
        public int role_sk { get; set; }
        public int created_by { get; set; }
        public DateTime created_on { get; set; }
        public int record_status { get; set; }
        public int row_version { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
    }
}