using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBL_MLDV_APP.Models.Security
{
    public class ApplicationSession
    {
        //AppSession property  to Get or Set Application Session
        public static UserAuthRepository Session
        {
            get
            {
                if (HttpContext.Current.Session["App"] == null)
                {
                    return null;
                }
                else
                {
                    return JsonConvert.DeserializeObject<UserAuthRepository>(HttpContext.Current.Session["App"].ToString());
                }
            }
            set
            {

                HttpContext.Current.Session["App"] = JsonConvert.SerializeObject(value).ToString();
            }
        }
    }
}