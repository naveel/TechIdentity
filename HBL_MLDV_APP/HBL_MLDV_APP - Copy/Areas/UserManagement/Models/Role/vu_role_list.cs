using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models.Role
{
    public class vu_role_list
    {
        public int role_sk { get; set; }
        public string role_desc { get; set; }
        public int status_sk { get; set; }
        public string status_desc { get; set; }
        public string Action { get; set; }
    }
}