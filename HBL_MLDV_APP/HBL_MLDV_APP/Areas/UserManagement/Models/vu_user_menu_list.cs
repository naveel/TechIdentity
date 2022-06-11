using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models
{
    public class vu_user_menu_list
    {
        public int user_sk { get; set; }
        public int mst_sk { get; set; }
        public int activityid { get; set; }
        public int can_view { get; set; }
        public int can_add { get; set; }
        public int can_edit { get; set; }
        public int can_del { get; set; }
        public int created_by { get; set; }
        public DateTime created_on { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
        public int row_version { get; set; }
        public int record_status { get; set; }
        public int role_sk { get; set; }
    }
}