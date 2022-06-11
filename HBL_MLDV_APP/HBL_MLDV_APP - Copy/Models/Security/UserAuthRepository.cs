
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Models.Security
{
    public class UserAuthRepository
    {
        public List<UserAccountDetails> UserAccountDetailObj { get; set; }
        public UserAccountModel UserAccountObj { get; set; }

       
        public int br_code { get; set; }
        public string br_desc { get; set; }
        public DateTime blklst_db_upt_dttme { get; set; }
        public string styp { get; set; }
    }
}