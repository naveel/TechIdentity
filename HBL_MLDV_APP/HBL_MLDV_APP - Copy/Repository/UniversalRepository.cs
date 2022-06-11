using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using HBL_MLDV_APP.Models.Security;
using System.Linq;
using System.Web.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Web.Mvc;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HBL_MLDV_APP.Repository
{
    public class UniversalRepository : IDisposable
    {
        public int Draft = 1;
        public int Proceed = 6;
        public int Approved = 2;
        public int Rejected = 7;
        public int Recall = 13;

        public string messageInternalException = "Internal Exception: Something went wrong";
        public string messagePermissionDenied = "User permission denied for this action";
        public string messageSW2 = "Something went wrong";

        public const int aprv_cat_sk_Creator = 1;

        public async Task<HttpResponseMessage> SaveData(object model, string Url, string username, string password)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());

                string req = JsonConvert.SerializeObject(model);
                HttpResponseMessage response = client.PostAsync(Url, new StringContent(req, Encoding.UTF8, "application/json")).Result;
                return response;
            }
        }

        public async Task<HttpResponseMessage> PostandGetData(object model, string Url)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());

                string req = JsonConvert.SerializeObject(model);
                HttpResponseMessage response = client.PostAsync(Url, new StringContent(req, Encoding.UTF8, "application/json")).Result;
                return response;
            }
        }

        public async Task<HttpResponseMessage> GetData(string Url, string username, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());

                HttpResponseMessage response = client.GetAsync(Url).Result;
                return response;
            }
        }
        public async Task<HttpResponseMessage> GetDataById(string Url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());
                HttpResponseMessage response = client.GetAsync(Url).Result;
                return response;
            }
        }



        public async Task<HttpResponseMessage> GETTrasByID(string Url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());
                HttpResponseMessage response = client.GetAsync(Url).Result;
                return response;
            }
        }
        public async Task<HttpResponseMessage> GetData(string Url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());

                HttpResponseMessage response = client.GetAsync(Url).Result;
                return response;
            }
        }

        public async Task<HttpResponseMessage> GetData(string Url, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = client.GetAsync(Url).Result;
                return response;
            }
        }

        public async Task<HttpResponseMessage> MarkDelete(string Url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());

                HttpResponseMessage response = client.GetAsync(Url).Result;
                return response;
            }
        }

        public async Task<HttpResponseMessage> SaveData(object model, string Url)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());

                string req = JsonConvert.SerializeObject(model);
                HttpResponseMessage response = client.PostAsync(Url, new StringContent(req, Encoding.UTF8, "application/json")).Result;
                return response;
            }
        }

      
        public async Task<HttpResponseMessage> LoadDropDownsync(string Url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());
                HttpResponseMessage response = null;
                response = client.GetAsync(Url).Result;
                return response;
            }
        }
        public async Task<HttpResponseMessage> CheckTransactionalStatus(object model)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(Enums.ApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                ApplicationSession.Session.UserAccountObj.Token.ToString());

                string req = JsonConvert.SerializeObject(model);
                HttpResponseMessage response = client.PostAsync("api/DropDown/CheckTransactionalStatus/", new StringContent(req, Encoding.UTF8, "application/json")).Result;
                return response;
            }
        }

        #region ScreeningApi
        public async Task<HttpResponseMessage> GetDataScreening(string Url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ScreeningApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
                
                HttpResponseMessage response = client.GetAsync(Url).Result;
                return response;
            }
        }
        public async Task<HttpResponseMessage> PostandGetDataScreening(object model, string Url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Enums.ScreeningApiUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string req = JsonConvert.SerializeObject(model);
                HttpResponseMessage response = client.PostAsync(Url, new StringContent(req, Encoding.UTF8, "application/json")).Result;
                return response;
            }
        }
        #endregion
        /// <summary>
        /// Check user access rights for the current action
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bit"></param>
        /// <returns>bool</returns>
        public async Task<bool> CheckRights(string url, int bit)
        {
            Cando cd = new Cando();
            if (ApplicationSession.Session!= null && ApplicationSession.Session.UserAccountDetailObj != null)
            {
                return await cd.CandoResult(ApplicationSession.Session.UserAccountDetailObj.Where(r => r.ActivityUrl.ToLower() == url.ToLower()).FirstOrDefault(), bit);
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> TokenValidation()
        {
            int sess_exp_val = 0;
            if (!string.IsNullOrEmpty(Enums.sess_exp_val)) { sess_exp_val = Convert.ToInt32(Enums.sess_exp_val); }
            HttpResponseMessage response = await GetData("api/Auth/TokenValidation/" + sess_exp_val);

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync();
                string res = JsonConvert.DeserializeObject<string>(data.Result);
                if (res == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        [HttpGet]
        public async Task<DataTable> GetMasterStatusList(string docTypeSk)
        {
            DataTable statusList = new DataTable();

            using (UniversalRepository repository = new UniversalRepository())
            {
                var response = repository.GetData("api/DropDown/GetStatusMasterList/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    statusList = JsonConvert.DeserializeObject<DataTable>(data);
                }
            }

            return statusList;
        }

        //[SessionAuthorize]
        [HttpGet]
        public async Task<DataTable> GetTxnStatusList(string docTypeSk)
        {
            DataTable statusList = new DataTable();

            using (UniversalRepository repository = new UniversalRepository())
            {
                var response = repository.GetData("api/DropDown/GetTxnStatusList/" + docTypeSk).Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    statusList = JsonConvert.DeserializeObject<DataTable>(data);
                }
            }

            return statusList;
        }
        public DateTime GetDateTimeForTimeZone()
        {
            DateTime UAE = DateTime.Now;

            if (WebConfigurationManager.AppSettings["timezone"] != null)
            {
                string timezone = WebConfigurationManager.AppSettings["timezone"].ToString();

                if (!string.IsNullOrWhiteSpace(timezone))
                {
                    TimeZoneInfo UAETimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                    DateTime utc = DateTime.UtcNow;
                    UAE = TimeZoneInfo.ConvertTimeFromUtc(utc, UAETimeZone);
                }
            }

            return UAE;
        }
        public void WriteException(string error, string process, [CallerMemberName] string method = "")
        {
            if (!Directory.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\")))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\");
            }

            string filelocation = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ErrorLog\\" + DateTime.Now.ToString("ddMMyyy") + "_" + process + ".txt";

            if (!Directory.Exists(Path.GetDirectoryName(filelocation)))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ErrorLog\\");
            }

            if (!File.Exists(filelocation))
            {
                using (StreamWriter sw = File.CreateText(filelocation))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(method.ToString());
                    sw.WriteLine("------------------------------");
                    sw.WriteLine(error);
                    sw.WriteLine("------------------------------");

                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filelocation))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(method.ToString());
                    sw.WriteLine("------------------------------");
                    sw.WriteLine(error);
                    sw.WriteLine("------------------------------");
                    sw.Close();
                }
            }
        }
        public void WriteProcess(string message, string process, [CallerMemberName] string method = "")
        {
            if (!Directory.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\")))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\");
            }

            string filelocation = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Process\\" + DateTime.Now.ToString("ddMMyyy") + "_" + process + ".txt";

            if (!Directory.Exists(Path.GetDirectoryName(filelocation)))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Process\\");
            }

            if (!File.Exists(filelocation))
            {
                using (StreamWriter sw = File.CreateText(filelocation))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(method.ToString());
                    sw.WriteLine("------------------------------");
                    sw.WriteLine(message);
                    sw.WriteLine("------------------------------");

                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filelocation))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(method.ToString());
                    sw.WriteLine("------------------------------");
                    sw.WriteLine(message);
                    sw.WriteLine("------------------------------");
                    sw.Close();
                }
            }
        }

        private readonly Type IEnumerableType = typeof(IEnumerable);
        private readonly Type StringType = typeof(string);
        

        public class CustomObject
        {
            public bool status { get; set; }
            public string Message { get; set; }
            public DataTable Data { get; set; }

        }
        public static string addCommas(string nStr)
        {
            nStr += "";
            //var x = nStr.Split('.');
            //string x1 = x[0];
            //var x2 = x.Length > 1 ? "." + x[1] : "";
            //var rgx = @"([(\d+)(\d{3})]+)";
            //Regex rg = new Regex(rgx);
            //while (rg.IsMatch(x1))
            //{
            //    x1 = x1.Replace(rgx, "$1" + ',' + "$2");
            //}
            //return x1 + x2;
            return Convert.ToDecimal(nStr).ToString("#,##0.00");
        }
        public string GetFullValueof_72SndrRcvrInfo(string _valueFirst, string _value)
        {
            string _returnValue, _valueReplace = "";
            //string _temp = Regex.Replace(b, "@[^\r\n]", "_");
            _valueReplace = _value.Replace("\n", "");
            if (_valueReplace.Length >= 1 && _valueReplace.Length <= 32)
            {
                _returnValue = _valueFirst + Environment.NewLine + "//" + _valueReplace.Substring(0, _valueReplace.Length);
            }
            else
            {
                _returnValue = _valueFirst + Environment.NewLine + "//" + _valueReplace.Substring(0, 32);
                if (_valueReplace.Length > 32)
                {
                    _returnValue += Environment.NewLine + "//" + _valueReplace.Substring(32, (_valueReplace.Length - 32));
                }
            }

            return _returnValue;
        }
        
        public void Dispose() { }
       
    }
}