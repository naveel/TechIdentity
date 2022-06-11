using HBL_MLDV_APP.App_Start;
using HBL_MLDV_APP.Areas.UserManagement.Models;
using HBL_MLDV_APP.Models;
using HBL_MLDV_APP.Models.Security;
using HBL_MLDV_APP.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HBL_MLDV_APP.Controllers
{
    //[SessionAuthorize]
    public class LoginController : Controller
    {
        // GET: Login
        UniversalRepository universalRepository = new UniversalRepository();
        public ActionResult Index()
        {
            try
            {
                //if (ApplicationSession.Session.UserAccountDetailObj.Count > 0)
                //{
                //    //return RedirectToAction("Index", "Attendance");
                //    return RedirectToAction("Index", "Home");
                //}
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string username, string password)
        {
            EncryptDecrypt encrypt = new EncryptDecrypt();

            try
            {
                string Encrypt_password = encrypt.Encrypt(password);
                string Encrypt_username = encrypt.Encrypt(username);
                vu_users usr = new vu_users();
                usr.username = username;
                usr.password = password;
                HttpResponseMessage result;
                using (LoginRepository _repo = new LoginRepository())
                {

                    result = await _repo.GetLoginData(usr, "api/Auth/AuthenticateAndGenerateSession");

                }
                string data = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    //string data = await result.Content.ReadAsStringAsync();
                    UserAuthRepository userauth = JsonConvert.DeserializeObject<UserAuthRepository>(data);
                    userauth.UserAccountObj.Username = username;

                    TempData["UserName"] = username;
                    TempData["branchList"] = await GetBranchesList(int.Parse(userauth.UserAccountObj.UserId.ToString()));

                    ApplicationSession.Session = new UserAuthRepository()
                    {
                        UserAccountObj = userauth.UserAccountObj,
                        UserAccountDetailObj = userauth.UserAccountDetailObj,
                        //setting branch sk temp. it has to be fetched from api

                        br_code = 0,//userauth.br_code,
                        br_desc = "Main"//userauth.br_desc
                    };

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    data = JsonConvert.DeserializeObject<string>(data);
                    TempData["ResultLogin"] = data;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ResultLogin"] = "Something went wrong";
                universalRepository.WriteException(ex.ToString(), "Login");
                return View("Index");
            }
        }

        public async Task<ActionResult> GenerateURL(int UID)
        {
            EncryptDecrypt encrypt = new EncryptDecrypt();

            try
            {
                HttpResponseMessage result;
                vu_users usr = new vu_users();
                usr.user_sk = UID;
                using (LoginRepository _repo = new LoginRepository())
                {

                    result = await _repo.GetLoginData(usr, "api/Auth/GenerateURL/");

                }
                string data = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    usr = JsonConvert.DeserializeObject<vu_users>(data);
                    return Json(usr, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    data = JsonConvert.DeserializeObject<string>(data);
                    TempData["ResultLogin"] = data;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ResultLogin"] = "Something went wrong";
                universalRepository.WriteException(ex.ToString(), "Login");
                return View("Index");
            }
        }
        
        public async Task<ActionResult> Logout(int UID)
        {
            Session.Abandon();
            Session.RemoveAll();
            HttpResponseMessage result;
            vu_users usr = new vu_users();
            usr.user_sk = UID;
            using (LoginRepository _repo = new LoginRepository())
            {
                result = await _repo.GetLoginData(usr, "api/Auth/Logout/");
            }
            string data = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                usr = JsonConvert.DeserializeObject<vu_users>(data);
                //return Json(usr, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", "Login");
        }
        
        //public async Task<ActionResult> GetLogin()
        //{
        //    return View("Index");
        //}

        //[ValidateGoogleCaptcha]
        //[HttpPost]
        public async Task<ActionResult> GetLogin(string du)
        {
            EncryptDecrypt encrypt = new EncryptDecrypt();

            try
            {
                string id = du;
                string dUrl = id;

                vu_users usr = new vu_users();
                usr.dynmc_url = dUrl;
                HttpResponseMessage result;
                using (LoginRepository _repo = new LoginRepository())
                {

                    result = await _repo.GetLoginData(usr, "api/Auth/AuthenticateAndGenerateSessionDynmc");

                }
                string data = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    //string data = await result.Content.ReadAsStringAsync();
                    UserAuthRepository userauth = JsonConvert.DeserializeObject<UserAuthRepository>(data);
                    userauth.UserAccountObj.Username = encrypt.Decrypt(userauth.UserAccountObj.Username_Enc);

                    TempData["UserName"] = userauth.UserAccountObj.Username;
                    TempData["branchList"] = await GetBranchesList(int.Parse(userauth.UserAccountObj.UserId.ToString()));

                    ApplicationSession.Session = new UserAuthRepository()
                    {
                        UserAccountObj = userauth.UserAccountObj,
                        UserAccountDetailObj = userauth.UserAccountDetailObj,
                        //setting branch sk temp. it has to be fetched from api

                        br_code = 0,//userauth.br_code,
                        br_desc = "Main"//userauth.br_desc
                    };

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    data = JsonConvert.DeserializeObject<string>(data);
                    TempData["ResultLogin"] = data;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ResultLogin"] = "Something went wrong";
                universalRepository.WriteException(ex.ToString(), "Login");
                return View("Index");
            }
        }

        private async Task<List<DropDownModel>> GetBranchesList(int user_sk)
        {
            List<DropDownModel> lst = new List<DropDownModel>();

            using (LoginRepository repository = new LoginRepository())
            {
                var response = repository.GetData("api/DropDown/GetUserBranches/" + user_sk).Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<DropDownModel>>(data);
                }
            }

            return lst;
        }


        public async Task<JsonResult> GetMenu()
        {
            //UserAuthRepository result = new UserAuthRepository();
            return Json(ApplicationSession.Session.UserAccountDetailObj.Where(x => x.CanView != 0).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}