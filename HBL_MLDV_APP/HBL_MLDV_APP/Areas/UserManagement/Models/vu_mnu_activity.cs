using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models
{
    public class vu_mnu_activity
    {
        public int activityid { get; set; }
        public string activitytitle { get; set; }
        public string activityurl { get; set; }
        public int activityparentid { get; set; }
        public string activitydisc { get; set; }
        public string activityicon { get; set; }
        public string module_nme { get; set; }

        public bool can_view { get; set; }
        public bool can_add { get; set; }
        public bool can_edit { get; set; }
        public bool can_del { get; set; }

        public bool isRole { get; set; }
    }
}