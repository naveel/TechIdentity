using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Models.Security
{
    public class UserAccountDetails
    {
        public int? AcivityId { get; set; }
        public string AcivityName { get; set; }
        public string AcivityTitle { get; set; }
        public string ActivityIcon { get; set; }
        public int? ActivityParentId { get; set; }
        public string ActivityUrl { get; set; }
        public int? CanAdd { get; set; }
        public int? CanDel { get; set; }
        public int? CanEdit { get; set; }
        public int? CanView { get; set; }
        public long UserId { get; set; }
        public string ActivityAttr { get; set; }
    }
}