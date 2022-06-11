using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Models.Security
{
    public class branch_mst
    {
        public int br_code { get; set; }
        public string br_desc { get; set; }
        public string br_addr { get; set; }
        public string br_contact { get; set; }
        public int br_activity_disabled { get; set; }
        public int created_by { get; set; }
        public DateTime? create_dt_tme { get; set; }
        public int? last_updated_by { get; set; }
        public DateTime? last_updated_on { get; set; }
    }
}