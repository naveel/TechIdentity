using HBL_MLDV_APP.Areas.UserManagement.Models;
using HBL_MLDV_APP.Models.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HBL_MLDV_APP.Repository
{
    public class LoginRepository : IDisposable
    {
        public void Dispose()
        {
         }

        public async Task<HttpResponseMessage> GetLoginData(vu_users model , string Url)
        {

            using (var client = new HttpClient())
            {
                //setup client

                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string req = JsonConvert.SerializeObject(model);
                HttpResponseMessage response = client.PostAsync(Url, new StringContent(req, Encoding.UTF8, "application/json")).Result;
                
                return response;
            }

        }

        public async Task<HttpResponseMessage> GetData(string Url)
        {
            using (var client = new HttpClient())
            {
                //setup client

                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(Url).Result;

                return response;
            }
        }
    }
}