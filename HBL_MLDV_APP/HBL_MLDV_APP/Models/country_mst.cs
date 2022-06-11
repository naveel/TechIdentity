using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Models
{
    public class country_mst
    {
        public int country_sk { get; set; }
        [Display(Name = "Country Name*")]
        [Required(ErrorMessage = "Country name is required")]
        [MaxLength(50, ErrorMessage = "Name length cannot be more than 50 characters")]
        [RegularExpression(@"([a-zA-Z0-9\s-_/,()]+)", ErrorMessage = "Special characters are not allowed except (-, _, (), /, ',')")]
        public string country_desc { get; set; }
        [Display(Name = "Select Region*")]
        [Required(ErrorMessage = "Region selection is required")]
        public int region_sk { get; set; }
        public string currency_code { get; set; }
        public string currency_name { get; set; }
        public int row_version { get; set; }
        public int created_by { get; set; }
        public DateTime? create_dt_tme { get; set; }
        public int? updated_by { get; set; }
        public DateTime? update_dt_tme { get; set; }
        public int record_status { get; set; }
        [Display(Name = "Select Flag")]
        [Required(ErrorMessage = "Flag selection is required")]
        public int Flag_id_sk { get; set; }
        public string img_url { get; set; }
        [Display(Name = "Select Currency")]
        [Required(ErrorMessage = "Currency selection is required")]
        public int currency_sk { get; set; }
        public string Action { get; set; }
        [Display(Name = "Inactive")]
        public bool record_status_bool { get { return record_status > 0; } set { record_status = value ? 1 : 0; } }
        public int? call_cde { get; set; }
        [Display(Name = "IBAN Length*")]
        [Required(ErrorMessage = "IBAN Length  is required")]
        public int iban_len { get; set; }
        [Display(Name = "Phone Number Length*")]
        [Required(ErrorMessage = "Phone Length is required")]
        //[Range(4, 20,ErrorMessage = "Phone Number Length should between 4 to 20")]
        public int phne_no_len { get; set; }
    }
}