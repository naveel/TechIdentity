using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models
{
    public class vu_user_lvl_can_do
    {
        public int user_cando_sk { get; set; }
        public int user_sk { get; set; }
        public int activityid { get; set; }
        public bool can_view { get; set; }
        public bool can_add { get; set; }
        public bool can_edit { get; set; }
        public bool can_del { get; set; }
        public int row_version { get; set; }
        public int record_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_on { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
    }
}