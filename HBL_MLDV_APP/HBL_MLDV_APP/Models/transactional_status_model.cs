using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Models
{
    public class transactional_status_model
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class transactional_sk_model
    {
        public int user_sk { get; set; }
        public int remtr_sk { get; set; }
        public int beneficiary_sk { get; set; }
        public int customer_sk { get; set; }
        public int agent_sk { get; set; }
        public int instrctd_currency_sk { get; set; }
        public int paymnt_currency_sk { get; set; }
        public int b_currency_sk { get; set; }
        public int f_currency_sk { get; set; }
        public int currency_sk { get; set; }
        public int bank_sk { get; set; }
        public List<int> currency_list { get; set; }
    }
}