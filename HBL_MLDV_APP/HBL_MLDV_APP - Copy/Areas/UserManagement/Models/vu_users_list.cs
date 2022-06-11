using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models
{
    public class vu_users_list
    {
        public int user_sk { get; set; }
        public string user_full_name { get; set; }
        public string username { get; set; }
        public int status_sk { get; set; }
        public string status_desc { get; set; }
        public string role_desc { get; set; }
        public string br_desc { get; set; }
        public int login_status { get; set; }
        public string remarks { get; set; }
        public string Action { get; set; }
        public int record_status { get; set; }
    }
}