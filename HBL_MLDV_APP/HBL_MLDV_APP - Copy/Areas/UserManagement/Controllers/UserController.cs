using HBL_MLDV_APP.App_Start;
using HBL_MLDV_APP.Areas.UserManagement.Models;
using HBL_MLDV_APP.Models;
using HBL_MLDV_APP.Models.Security;
using HBL_MLDV_APP.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HBL_MLDV_APP.Areas.UserManagement.Controllers
{

    [SessionAuthorize]
    public class UserController : Controller
    {
        private readonly string pageUrl = "/UserManagement/User";
        private readonly UniversalRepository universalRepository= new UniversalRepository();
        EncryptDecrypt enc = new EncryptDecrypt();
        // private readonly TPRespository tp = new TPRespository();

        // GET: UserManagement/User
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                if (await universalRepository.TokenValidation() == true)
                {
                    if (await universalRepository.CheckRights(pageUrl, 1))
                    {
                        ViewBag.canAdd = await universalRepository.CheckRights(pageUrl, 2);
                        return View();
                    }
                    else
                    {
                        SetNotification("warning", universalRepository.messagePermissionDenied);
                        return RedirectToAction("Index", "Home", new { Area = "" });
                    }
                }
                else
                {
                    SetNotification("warning", universalRepository.messagePermissionDenied);
                    return RedirectToAction("Logout", "Login", new { Area = "" });
                }
            }
            catch (Exception ex)
            {
                SetNotification("error", universalRepository.messageInternalException);
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
        }

        //[SessionAuthorize]
        public async Task<JsonResult> GetData()
        {
            List<vu_users_list> usersList = new List<vu_users_list>();

            // Get active cities list
            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api" + pageUrl + "/GetUsersList/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    usersList = JsonConvert.DeserializeObject<List<vu_users_list>>(data);
                }
            }

            // Add actions
            await AddActions(usersList);

            return new JsonResult { Data = usersList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<ActionResult> Create()
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 2))
                {
                    await SetTempData();
                    return View();
                }
                else
                {
                    SetNotification("warning", universalRepository.messagePermissionDenied);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                SetNotification("error", universalRepository.messageInternalException);
                // log exception in file
                universalRepository.WriteException(ex.ToString(), "User Approval Create Initialization");
                return RedirectToAction("Index");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(vu_users_aprv_vm model)
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 2))
                {
                    // Set status sk based on sender
                    switch (model.sender)
                    {
                        case "1":
                            model.status_sk = universalRepository.Draft;
                            break;
                        case "6":
                            model.status_sk = universalRepository.Proceed;
                            break;
                        default:
                            model.status_sk = universalRepository.Draft;
                            break;
                    }
                    model.email = model.email.ToLower();
                    model.doc_link = HttpContext.Request.Url.AbsoluteUri;
                    model.username = model.email;
                    model.record_status = model.status_bool == true ? 0 : 1;
                    var email = ValidateEmailRegistered(model.email, "Create");
                    if (!Convert.ToBoolean(email.Data))
                    {
                        SetNotification("error", "Email already registered");
                        return View(model);
                    }
                    using (UniversalRepository repository= new UniversalRepository())
                    {
                        var response = repository.SaveData(model, "api/UserManagement/User/SendUserForApproval/" + ApplicationSession.Session.br_code + "/" + ApplicationSession.Session.UserAccountObj.UserId).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string data = await response.Content.ReadAsStringAsync();
                            var jsonObj = JObject.Parse(data);

                            if (jsonObj.GetValue("Message").ToString() == "Record has been saved successfully")
                            {
                                // Generate pool record on proceed
                                if (model.sender == "Proceed")
                                {
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonObj.GetValue("Data").ToString());
                                    // Verification
                                    //await tp.ValidationProccessed(model, 3014, int.Parse(dt.Rows[0]["rmtnc_sk"].ToString()), mst.remtr_sk, mst.rmtnc_doc_nbr, Convert.ToDateTime(mst.rmtnc_doc_dte), Request.Url.AbsoluteUri.Replace("Create", "Edit/") + int.Parse(dt.Rows[0]["rmtnc_sk"].ToString()));
                                }

                                SetNotification("info", jsonObj.GetValue("Message").ToString());
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                SetNotification("error", jsonObj.GetValue("Message").ToString());
                            }
                        }
                        else
                        {
                            string data = await response.Content.ReadAsStringAsync();
                            var jsonObj = JObject.Parse(data);

                            SetNotification("error", response.StatusCode.ToString() + ": " + universalRepository.messageSW2);
                        }
                    }
                }
                else
                {
                    SetNotification("warning", universalRepository.messagePermissionDenied);
                }
            }
            catch (Exception ex)
            {
                // log exception in file
                universalRepository.WriteException(ex.ToString(), "User Approval Creation");
                SetNotification("error", universalRepository.messageInternalException);
            }

            return View(model);
        }

        public async Task<ActionResult> View(int id)
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 1))
                {
                    ViewBag.canEdit = await universalRepository.CheckRights(pageUrl, 1);

                    await SetTempData();

                    // Get existing details for user
                    vu_users_aprv_vm vm = await GetUserAprvById(id);

                    // Get child menu count
                    int menuCount = (TempData.Peek("mnuList") as List<HBL_MLDV_APP.Areas.UserManagement.Models.vu_mnu_activity>).Where(a => a.activityparentid > 0).ToList().Count;

                    // Set count for each action
                    int selectAll = vm.cando.Where(a => a.role_sk == 0 && a.can_view_bool == true).ToList().Count;
                    int createAll = vm.cando.Where(a => a.role_sk == 0 && a.can_add_bool == true).ToList().Count;
                    int updateAll = vm.cando.Where(a => a.role_sk == 0 && a.can_edit_bool == true).ToList().Count;
                    int deleteAll = vm.cando.Where(a => a.role_sk == 0 && a.can_del_bool == true).ToList().Count;

                    // Set value for each action's parent checkbox
                    vm.selectAll = (selectAll == menuCount);
                    vm.createAll = (createAll == menuCount);
                    vm.updateAll = (updateAll == menuCount);
                    vm.deleteAll = (deleteAll == menuCount);

                    return View(vm);
                }
                else
                {
                    SetNotification("warning", universalRepository.messagePermissionDenied);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                SetNotification("error", universalRepository.messageInternalException);
                // log exception in file
                universalRepository.WriteException(ex.ToString(), "User Approval View Initialize");
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 4))
                {
                    ViewBag.canEdit = await universalRepository.CheckRights(pageUrl, 4);

                    await SetTempData();

                    // Get existing details for user
                    vu_users_aprv_vm vm = await GetUserAprvById(id);

                    // Get child menu count
                    List<vu_mnu_activity> mnu = (TempData.Peek("mnuList") as List<vu_mnu_activity>).Where(a => a.activityparentid > 0).ToList();
                    int menuCount = mnu.Count;

                    // Set count for each action
                    int selectAll = vm.cando.Where(a => a.role_sk == 0 && a.can_view_bool == true).ToList().Count;
                    int createAll = vm.cando.Where(a => a.role_sk == 0 && a.can_add_bool == true).ToList().Count;
                    int updateAll = vm.cando.Where(a => a.role_sk == 0 && a.can_edit_bool == true).ToList().Count;
                    int deleteAll = vm.cando.Where(a => a.role_sk == 0 && a.can_del_bool == true).ToList().Count;
                    //foreach (var cn in vm.cando)
                    //{
                        
                        //if (vm.role != null && vm.role.Count > 0)
                        //{
                        //    var m = vm.role.FindAll(x => x.activityid == cn.activityid).FirstOrDefault();
                        //    if (m != null)
                        //    {
                        //        //int cntotal = cn.can_view + cn.can_add + cn.can_edit + cn.can_del;
                        //        //int mtotal = m.can_view + m.can_add + m.can_edit + m.can_del;
                        //        if(m.can_view_bool== true || cn.can_view_bool)
                        //        cn.can_view_bool = true;

                        //        if (m.can_add_bool == true || cn.can_add_bool)
                        //            cn.can_add_bool = true;

                        //        if (m.can_edit_bool == true || cn.can_edit_bool)
                        //            cn.can_edit_bool = true;

                        //        if (m.can_del_bool == true || cn.can_del_bool)
                        //            cn.can_del_bool = true;
                        //        //m.can_view = cn.can_view;
                        //        //m.can_add = cn.can_add;
                        //        //m.can_edit = cn.can_edit;
                        //        //m.can_del = cn.can_del;

                        //    }
                        //}
                   // }

                    // Set value for each action's parent checkbox
                    vm.selectAll = (selectAll == menuCount);
                    vm.createAll = (createAll == menuCount);
                    vm.updateAll = (updateAll == menuCount);
                    vm.deleteAll = (deleteAll == menuCount);
                    vm.username = enc.Decrypt(vm.username);
                    vm.password = "********";
                    return View(vm);
                }
                else
                {
                    SetNotification("warning", universalRepository.messagePermissionDenied);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                SetNotification("error", universalRepository.messageInternalException);
                // log exception in file
                universalRepository.WriteException(ex.ToString(), "User Approval Edit Initialize");
                return RedirectToAction("Index");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(vu_users_aprv_vm model)
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 4))
                {
                    vu_users_aprv_vm bakupvm = await GetUserAprvById(Convert.ToInt32(model.user_sk));
                    model.username = bakupvm.username;
                    model.email = bakupvm.email;
                    model.password = bakupvm.password;
                    // Set status sk based on sender
                    switch (model.sender)
                    {
                        case "1":
                            model.status_sk = universalRepository.Draft;
                            break;
                        case "6":
                            model.status_sk = universalRepository.Proceed;
                            model.state = "Changed";
                            break;
                        default:
                            model.status_sk = universalRepository.Draft;
                            break;
                    }
                    model.doc_link = HttpContext.Request.Url.AbsoluteUri.Replace("Edit", "View");
                    //model.username = model.email;
                    var email = ValidateEmailRegistered(model.email, "Edit");
                    if (!Convert.ToBoolean(email.Data))
                    {
                        SetNotification("error", "Email already registered");
                        return View(model);
                    }
                    using (UniversalRepository repository= new UniversalRepository())
                    {
                        var response = repository.SaveData(model, "api/UserManagement/User/SendUserForApprovalUpdate/" + ApplicationSession.Session.br_code + "/" + ApplicationSession.Session.UserAccountObj.UserId).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string data = await response.Content.ReadAsStringAsync();
                            var jsonObj = JObject.Parse(data);

                            if (jsonObj.GetValue("Message").ToString() == "Record has been updated successfully")
                            {
                                // Generate pool record on proceed
                                if (model.sender == "6")
                                {
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonObj.GetValue("Data").ToString());
                                    // Verification
                                    //await tp.ValidationProccessed(model, 3014, int.Parse(dt.Rows[0]["rmtnc_sk"].ToString()), mst.remtr_sk, mst.rmtnc_doc_nbr, Convert.ToDateTime(mst.rmtnc_doc_dte), Request.Url.AbsoluteUri.Replace("Create", "Edit/") + int.Parse(dt.Rows[0]["rmtnc_sk"].ToString()));
                                }

                                SetNotification("info", jsonObj.GetValue("Message").ToString());
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                SetNotification("error", jsonObj.GetValue("Message").ToString());
                            }
                        }
                        else
                        {
                            string data = await response.Content.ReadAsStringAsync();
                            var jsonObj = JObject.Parse(data);

                            SetNotification("error", response.StatusCode.ToString() + ": " + universalRepository.messageSW2);
                        }
                    }
                }
                else
                {
                    SetNotification("warning", universalRepository.messagePermissionDenied);
                }
            }
            catch (Exception ex)
            {
                // log exception in file
                universalRepository.WriteException(ex.ToString(), "User Approval Creation");
                SetNotification("error", universalRepository.messageInternalException);
            }

            return View(model);
        }

        //[HttpPost]
        //public ActionResult Edit(Users model)
        //{
        //    try
        //    {
        //        EncryptDecrypt encrypt = new EncryptDecrypt();

        //        if (ModelState.IsValid)
        //        {
        //            using (UserService _serv = new UserService())
        //            {
        //                //model.updated = DateTime.Now;
        //                model.state = "Changed";
        //                //this.AttachTaskId(model);
        //                model.default_module = 1;
        //                model.user_level = "initial";
        //                model.last_login = DateTime.Now;
        //                model.last_update = DateTime.Now;
        //                model.created = DateTime.Now;
        //                model.login_hash = "test";
        //                model.usr_creatd_id = 1;
        //                model.del_dte = DateTime.Now;
        //                model.pswrd_expiry_dte = DateTime.Now;
        //                model.username = encrypt.Encrypt(model.username);
        //                model.password = encrypt.Encrypt(model.password);
        //                var response = _serv.SaveUsers(model);
        //                var data = JsonConvert.SerializeObject(response);
        //                var Jobj = JObject.Parse(data);
        //                Response.Write(Jobj.GetValue("Message"));
        //                return RedirectToAction("Index");
        //            }
        //        }
        //        else
        //        {
        //            EmployeeDropDowns();
        //            UserRolesDropDowns();
        //            LanguangeDropDowns();
        //            CompanyDropDowns();
        //            DesignationDropDowns();
        //            return View(model);
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //[SessionAuthorize]


        #region ChangePassword
        [HttpGet]
        public async Task<ActionResult> ChangePassword()
        {
            try
            {
                ChangePassword model = new ChangePassword();
                model.userid = ApplicationSession.Session.UserAccountObj.UserId;
                model.username = ApplicationSession.Session.UserAccountObj.Username;
                ViewBag.username = ApplicationSession.Session.UserAccountObj.Username;
                return View(model);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePassword model)
        {
            try
            {
                EncryptDecrypt encry = new EncryptDecrypt();
                model.OldPasswordEncrpt = encry.Encrypt(model.OldPassword);
                model.NewPasswordEncrpt = encry.Encrypt(model.NewPassword);
                using (UniversalRepository repository= new UniversalRepository())
                {
                    var response = repository.SaveData(model, "api/UserManagement/User/ChangePassword/").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        CustomObject obj = JsonConvert.DeserializeObject<CustomObject>(data);
                        if (obj != null)
                        {
                            if (obj.status == true)
                            {
                                SetNotification("success", obj.Message);
                            }
                            else
                            {
                                SetNotification("error", obj.Message);
                            }
                        }
                        else
                        {
                            SetNotification("error", "Password not change! Something went wrong");
                        }

                    }
                }
                model = new ChangePassword();
                model.userid = ApplicationSession.Session.UserAccountObj.UserId;
                model.username = ApplicationSession.Session.UserAccountObj.Username;
                ViewBag.username = ApplicationSession.Session.UserAccountObj.Username;
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        #endregion
        [HttpGet]
        public JsonResult ValidateEmailRegistered(string email, string page)
        {
            bool ret = true;
            vu_users model = new vu_users();
            model.email = email.ToLower();
            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.SaveData(model, "api" + pageUrl + "/ValidateEmailRegistered/" + page).Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    ret = JsonConvert.DeserializeObject<bool>(data.Result);
                }
            }

            return new JsonResult { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        private async Task<List<DropDownModel>> GetUserRoles()
        {
            List<DropDownModel> rolesList = new List<DropDownModel>();

            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api/DropDown/GetUserRoles/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    rolesList = JsonConvert.DeserializeObject<List<DropDownModel>>(data);
                }
            }

            return rolesList;
        }

        //[SessionAuthorize]
        private async Task<List<DropDownModel>> GetUsers()
        {
            List<DropDownModel> usersList = new List<DropDownModel>();

            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api/DropDown/Users/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    usersList = JsonConvert.DeserializeObject<List<DropDownModel>>(data);
                }
            }

            return usersList;
        }

        //[SessionAuthorize]
        private async Task<List<DropDownModel>> GetBranches()
        {
            List<DropDownModel> usersList = new List<DropDownModel>();

            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api/DropDown/Branches/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    usersList = JsonConvert.DeserializeObject<List<DropDownModel>>(data);
                }
            }

            return usersList;
        }

        //[SessionAuthorize]
        private async Task<List<country_mst>> GetCountryList()
        {
            List<country_mst> countryList = new List<country_mst>();

            using (UniversalRepository repository = new UniversalRepository())
            {
                var response = repository.GetData("api/DropDown/GetActiveCountries/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    countryList = JsonConvert.DeserializeObject<List<country_mst>>(data);
                }
            }

            return countryList;
        }

        //[SessionAuthorize]
        private async Task<List<vu_mnu_activity>> GetAllMenus()
        {
            List<vu_mnu_activity> mnuList = new List<vu_mnu_activity>();

            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api/UserManagement/User/GetAllMenus/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    mnuList = JsonConvert.DeserializeObject<List<vu_mnu_activity>>(data);
                }
            }

            return mnuList;
        }

        //[SessionAuthorize]
        private async Task<List<vu_mnu_activity>> GetMenusByUserSk(int user_sk)
        {
            List<vu_mnu_activity> mnuList = new List<vu_mnu_activity>();

            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api/UserManagement/User/GetMenusByUserSk/" + user_sk).Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    mnuList = JsonConvert.DeserializeObject<List<vu_mnu_activity>>(data);
                }
            }

            return mnuList;
        }

        //[SessionAuthorize]
        private async Task SetMasterStatus(string docTypeSk)
        {
            using (UniversalRepository universalRepository= new UniversalRepository())
            {
                var list = await universalRepository.GetMasterStatusList(docTypeSk);

                if (list.Rows.Count > 0)
                {
                    List<DropDownModel> ddm = new List<DropDownModel>();

                    for (int i = 0; i < list.Rows.Count; i++)
                    {
                        ddm.Add(new DropDownModel
                        {
                            value = list.Rows[i]["value"].ToString(),
                            text = list.Rows[i]["text"].ToString()
                        });
                    }

                    TempData["statusMasterList"] = ddm;
                }
                else
                {
                    TempData["statusMasterList"] = null;
                }
            }
        }

        private async Task<vu_users_aprv_vm> GetUserAprvById(int id)
        {
            vu_users_aprv_vm obj = new vu_users_aprv_vm();

            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api" + pageUrl + "/GetUserAprvById/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    obj = JsonConvert.DeserializeObject<vu_users_aprv_vm>(data);
                }
            }

            return obj;
        }

        //[SessionAuthorize]
        private async Task AddActions(List<vu_users_list> usersList)
        {
            bool canEdit = await universalRepository.CheckRights(pageUrl, 4);
            //bool canDelete = await universalRepository.CheckRights(pageUrl, 8);

            foreach (var item in usersList)
            {
                //if (canEdit && item.status_desc == "Draft")
                item.Action = "<a title='Edit' name='Edit' href='" + pageUrl + "/Edit/" + item.user_sk + "'><i class='fa fa-fw fa-pencil-square-o' style='color:orange'></i></a> |";
                //else
                item.Action += "<a title='View' name='View' href='" + pageUrl + "/View/" + item.user_sk + "'><i class='fa fa-fw fa-eye'></i></a> |";
                //if (canDelete)
                //    item.Action += " <a title='Delete' name='Delete' href='#' data-id=" + item.crsp_agnt_sk + " data-toggle='modal' data-target='#myModal'><i style='color:red' class='fa fa-fw fa-remove'></i></a> |";

                if (item.Action != null)
                {
                    item.Action = item.Action.TrimEnd('|');
                }

                if (!string.IsNullOrEmpty(item.username))
                {
                    item.username = enc.Decrypt(item.username);
                }
            }
        }

        //[SessionAuthorize]
        private void SetNotification(string type, string message)
        {
            TempData["Notify"] = message;
            TempData["NotifyType"] = type;
        }

        private async Task SetTempData()
        {
            await SetMasterStatus("3027");
            TempData["rolesList"] = await GetUserRoles();
            TempData["countryList"] = await GetCountryList();
            TempData["branchList"] = await GetBranches();
            TempData["mnuList"] = await GetAllMenus();
        }

        //[HttpPost]
        //public async Task<ActionResult> Delete(int id)
        //{


        //    UserService _serv = new UserService();

        //    string deletericecooking = await _serv.DeleteUsersbyId(id);



        //    //if (deletericecooking != null)
        //    //{

        //    //    //  EmploymentStatusService _serv = new EmploymentStatusService();

        //    //    TempData["message"] = string.Format(deletericecooking);
        //    //}

        //    return RedirectToAction("Index");


        //}

        //private void SetNotification(string type, string message)
        //{
        //    TempData["Notify"] = message;
        //    TempData["NotifyType"] = type;
        //}
        public class CustomObject
        {
            public bool status { get; set; }
            public string Message { get; set; }
            public DataTable Data { get; set; }

        }
    }
}
