using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Areas.UserManagement.Models.Role
{
    public class vu_role_lvl_can_do_aprv
    {
        public int role_cando_sk { get; set; }
        public int activityid { get; set; }
        public int activityparentid { get; set; }
        public string activitytitle { get; set; }
        public bool can_view_bool
        {
            get
            {
                return can_view == 1 ? true : false;
            }
            set
            {
                can_view = value ? 1 : 0;
            }
        }
        public bool can_add_bool
        {
            get
            {
                return can_add == 2 ? true : false;
            }
            set
            {
                can_add = value ? 2 : 0;
            }
        }
        public bool can_edit_bool
        {
            get
            {
                return can_edit == 4 ? true : false;
            }
            set
            {
                can_edit = value ? 4 : 0;
            }
        }
        public bool can_del_bool
        {
            get
            {
                return can_del == 8 ? true : false;
            }
            set
            {
                can_del = value ? 8 : 0;
            }
        }
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
        public string state { get; set; }
    }
}