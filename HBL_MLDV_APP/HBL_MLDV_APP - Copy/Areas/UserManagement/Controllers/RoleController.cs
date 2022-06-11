using HBL_MLDV_APP.App_Start;
using HBL_MLDV_APP.Areas.UserManagement.Models;
using HBL_MLDV_APP.Areas.UserManagement.Models.Role;
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
using System.Web;
using System.Web.Mvc;

namespace HBL_MLDV_APP.Areas.UserManagement.Controllers
{
    [SessionAuthorize]
    public class RoleController : Controller
    {
        private readonly string pageUrl = "/UserManagement/Role";
        private readonly UniversalRepository universalRepository= new UniversalRepository();
       // private readonly TPRespository tp = new TPRespository();

        // GET: UserManagement/Role
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
            List<vu_role_list> rolesList = new List<vu_role_list>();

            // Get active cities list
            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api" + pageUrl + "/GetRolesList/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    rolesList = JsonConvert.DeserializeObject<List<vu_role_list>>(data);
                }
            }

            // Add actions
            await AddActions(rolesList);

            return new JsonResult { Data = rolesList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                universalRepository.WriteException(ex.ToString(), "Role Approval Create Initialization");
                return RedirectToAction("Index");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(vu_role_mst_aprv model)
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 2))
                {
                    //Set status sk based on sender
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
                    model.doc_link = HttpContext.Request.Url.AbsoluteUri;
                    using (UniversalRepository repository = new UniversalRepository())
                    {
                        var response = repository.SaveData(model, "api" + pageUrl + "/SendRoleForApproval/" + ApplicationSession.Session.br_code + "/" + ApplicationSession.Session.UserAccountObj.UserId).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string data = await response.Content.ReadAsStringAsync();
                            var jsonObj = JObject.Parse(data);

                            if (jsonObj.GetValue("Message").ToString() == "Record has been saved successfully")
                            {
                                //Generate pool record on proceed
                                if (model.sender == "6")
                                {
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonObj.GetValue("Data").ToString());
                                    //Verification
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
                //log exception in file
                universalRepository.WriteException(ex.ToString(), "Role Approval Creation");
                SetNotification("error", universalRepository.messageInternalException);
            }

            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 4))
                {
                    ViewBag.canEdit = await universalRepository.CheckRights(pageUrl, 4);

                    await SetTempData();

                    // Get existing details for role
                    vu_role_mst_aprv vm = await GetRoleAprvById(id);

                    // Get child menu count
                    int menuCount = (TempData.Peek("mnuList") as List<HBL_MLDV_APP.Areas.UserManagement.Models.vu_mnu_activity>).Where(a => a.activityparentid > 0).ToList().Count;

                    if (vm.cando != null)
                    {
                        // Set count for each action
                        int selectAll = vm.cando.Where(a => a.role_sk == 0 && a.can_view_bool == true).ToList().Count;
                        int createAll = vm.cando.Where(a => a.role_sk == 0 && a.can_add_bool == true).ToList().Count;
                        int updateAll = vm.cando.Where(a => a.role_sk == 0 && a.can_edit_bool == true).ToList().Count;
                        int deleteAll = vm.cando.Where(a => a.role_sk == 0 && a.can_del_bool == true).ToList().Count;
                    }

                    // Set value for each action's parent checkbox
                    //vm.selectAll = (selectAll == menuCount);
                    //vm.createAll = (createAll == menuCount);
                    //vm.updateAll = (updateAll == menuCount);
                    //vm.deleteAll = (deleteAll == menuCount);

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
                universalRepository.WriteException(ex.ToString(), "Role Approval Edit Initialize");
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> View(int id)
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 1))
                {
                    ViewBag.canEdit = await universalRepository.CheckRights(pageUrl, 1);

                    await SetTempData();

                    // Get existing details for role
                    vu_role_mst_aprv vm = await GetRoleAprvById(id);

                    // Get child menu count
                    int menuCount = (TempData.Peek("mnuList") as List<HBL_MLDV_APP.Areas.UserManagement.Models.vu_mnu_activity>).Where(a => a.activityparentid > 0).ToList().Count;

                    // Set count for each action
                    int selectAll = vm.cando.Where(a => a.role_sk == 0 && a.can_view_bool == true).ToList().Count;
                    int createAll = vm.cando.Where(a => a.role_sk == 0 && a.can_add_bool == true).ToList().Count;
                    int updateAll = vm.cando.Where(a => a.role_sk == 0 && a.can_edit_bool == true).ToList().Count;
                    int deleteAll = vm.cando.Where(a => a.role_sk == 0 && a.can_del_bool == true).ToList().Count;

                    // Set value for each action's parent checkbox
                    //vm.selectAll = (selectAll == menuCount);
                    //vm.createAll = (createAll == menuCount);
                    //vm.updateAll = (updateAll == menuCount);
                    //vm.deleteAll = (deleteAll == menuCount);

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
                universalRepository.WriteException(ex.ToString(), "Role Approval View Initialize");
                return RedirectToAction("Index");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(vu_role_mst_aprv model)
        {
            try
            {
                if (await universalRepository.CheckRights(pageUrl, 4))
                {
                    //Set status sk based on sender
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
                    using (UniversalRepository repository = new UniversalRepository())
                    {
                        var response = repository.SaveData(model, "api" + pageUrl + "/SendRoleForApprovalUpdate/" + ApplicationSession.Session.br_code + "/" + ApplicationSession.Session.UserAccountObj.UserId).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string data = await response.Content.ReadAsStringAsync();
                            var jsonObj = JObject.Parse(data);

                            if (jsonObj.GetValue("Message").ToString() == "Record has been updated successfully")
                            {
                                //Generate pool record on proceed
                                if (model.sender == "6")
                                {
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonObj.GetValue("Data").ToString());
                                    //Verification
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
                //log exception in file
                universalRepository.WriteException(ex.ToString(), "Role Approval Creation");
                SetNotification("error", universalRepository.messageInternalException);
            }

            return View(model);
        }

        
        private async Task<List<vu_mnu_activity>> GetAllMenus()
        {
            List<vu_mnu_activity> mnuList = new List<vu_mnu_activity>();

            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api" + pageUrl + "/GetAllMenus/").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    mnuList = JsonConvert.DeserializeObject<List<vu_mnu_activity>>(data);
                }
            }

            return mnuList;
        }

        //[SessionAuthorize]
        private async Task<List<vu_mnu_activity>> GetMenusByRoleSk(int role_sk)
        {
            List<vu_mnu_activity> mnuList = new List<vu_mnu_activity>();

            using (UniversalRepository repository= new UniversalRepository())
            {
                var response = repository.GetData("api" + pageUrl + "/GetMenusByRoleSk/" + role_sk).Result;

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

        private async Task<vu_role_mst_aprv> GetRoleAprvById(int id)
        {
            vu_role_mst_aprv obj = new vu_role_mst_aprv();

            using (UniversalRepository repository = new UniversalRepository())
            {
                var response = repository.GetData("api" + pageUrl + "/GetRoleAprvById/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    obj = JsonConvert.DeserializeObject<vu_role_mst_aprv>(data);
                }
            }

            return obj;
        }

        //[SessionAuthorize]
        private async Task AddActions(List<vu_role_list> rolesList)
        {
            bool canEdit = await universalRepository.CheckRights(pageUrl, 4);
            //bool canDelete = await universalRepository.CheckRights(pageUrl, 8);

            foreach (var item in rolesList)
            {
                if (canEdit)
                    item.Action = "<a title='Edit' name='Edit' href='" + pageUrl + "/Edit/" + item.role_sk + "'><i class='fa fa-fw fa-pencil-square-o' style='color:orange'></i></a> |";
                else
                    item.Action = "<a title='View' name='View' href='" + pageUrl + "/View/" + item.role_sk + "'><i class='fa fa-fw fa-eye'></i></a> |";

                if (item.Action != null)
                    item.Action = item.Action.TrimEnd('|');
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
            await SetMasterStatus("3029");
            TempData["mnuList"] = await GetAllMenus();
        }
    }
}