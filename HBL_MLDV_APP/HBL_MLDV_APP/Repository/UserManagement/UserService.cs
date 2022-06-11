using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HBL_MLDV_APP.Repository.UserManagement
{
   public class UserService :  IDisposable
    {


        DbContextHelper _ctx;

        public async Task<object> SaveUsers(Users model)
        {
            object obj = null;
            try
            {

                using (DbContextHelper db = new DbContextHelper())
                {
                    obj = db.SaveChanges("public", model);
                }
            }
            catch (Exception ex)
            {
                return new CustomObject { Data = null, Message = ex.ToString() };
            }
            return obj;
        }


        public async Task<int> Register(Users model)
        {

            DbContextHelper db = new DbContextHelper();


            NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = '"+ model.username +"'");

            int i = db.ExecuteScalar(cmd);


            return i;
            
            //command.CommandText = query;
        
        
        
        
        
        
        }

        public async Task<List<Users_VM>> GetUser()
        {
            var tg = new List<Users_VM>();
            try
            {


                string select = @"Select * from UsersGet_vu";
                _ctx = new DbContextHelper();
                DataTable dt = await _ctx.SelectRecord(select);
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tg.Add(new BL_HRM.Models.Users_VM
                        {



                            id = Convert.ToInt32(dt.Rows[i]["userid"]),

                            username = dt.Rows[i]["username"].ToString(),
                            email = dt.Rows[i]["email"].ToString(),
                            password = WebUtility.HtmlDecode(dt.Rows[i]["password"].ToString()),


                            employee_name = dt.Rows[i]["employee_name"].ToString()==""? dt.Rows[i]["username"].ToString(): dt.Rows[i]["employee_name"].ToString(),
                            default_module = Convert.ToInt32(dt.Rows[i]["default_module"].ToString()),
                            ///   last_login = dt.Rows[i]["last_login"].ToString(),
                            //   last_update = dt.Rows[i]["last_update"].ToString(),
                            //  created = dt.Rows[i]["created"].ToString(),
                            login_hash = dt.Rows[i]["login_hash"].ToString(),
                            //lang_name = dt.Rows[i]["lang_name"].ToString(),
                            //mt_code = Convert.ToInt32(dt.Rows[i]["mt_code"].ToString()),
                            user_ful_name = dt.Rows[i]["user_ful_name"].ToString(),
                          //  name = dt.Rows[i]["jobtitlename"].ToString(),
                            creat_appr_flg = dt.Rows[i]["creat_appr_flg"].ToString(),
                            //      usr_creatd_id = Convert.ToDateTime(dt.Rows[i]["updated"].ToString()),
                            remarks = dt.Rows[i]["remarks"].ToString(),
                           // comp_name = dt.Rows[i]["comp_name"].ToString(),
                          //  role_name = dt.Rows[i]["role_name"].ToString(),


                            //   row_version = Convert.ToInt32(dt.Rows[i]["row_version"]),
                            //  record_status = Convert.ToInt32(dt.Rows[i]["record_status"]),

                        });
                    }

                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
          
            return tg;
        }

        public async Task<Users> GetUserById(int Course_Id)
        {
             Users tg = null;


             Encrypt_Password desccript = new Encrypt_Password();



         


            string select = @"select id, username, email, password, employee_id, default_module, user_level, 
       last_login, last_update, created, login_hash, lang_id, mt_code, 
       user_ful_name, jobstitle_id, creat_appr_flg, usr_creatd_id, 
       del_dte, remarks, pswrd_expiry_dte, comp_id, role_id,userid,coalesce(users.row_version,0) as row_version , coalesce(users.record_status,0) as record_status
               
       FROM users WHERE id =" + Course_Id + "";
            _ctx = new DbContextHelper();

            DataTable dt = await _ctx.SelectRecord(select);
            if (dt.Rows.Count > 0)
            {

              
                 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tg = new Users()
                    {


                        id = Convert.ToInt32(dt.Rows[i]["id"]),

                        username = dt.Rows[i]["username"].ToString(),
                        email = dt.Rows[i]["email"].ToString(),
                        password =  dt.Rows[i]["password"].ToString(),
                        
                        employee_id = Convert.ToInt32(dt.Rows[i]["employee_id"].ToString()==""?"0": dt.Rows[i]["employee_id"]),
                        default_module = Convert.ToInt32(dt.Rows[i]["default_module"].ToString()),
                        ///   last_login = dt.Rows[i]["last_login"].ToString(),
                        //   last_update = dt.Rows[i]["last_update"].ToString(),
                        //  created = dt.Rows[i]["created"].ToString(),
                        login_hash = dt.Rows[i]["login_hash"].ToString(),
                        lang_id = Convert.ToInt32(dt.Rows[i]["lang_id"].ToString()),
                        mt_code = Convert.ToInt32(dt.Rows[i]["mt_code"].ToString()),
                        user_ful_name = dt.Rows[i]["user_ful_name"].ToString(),
                        jobstitle_id = Convert.ToInt32(dt.Rows[i]["jobstitle_id"].ToString()),
                        creat_appr_flg = dt.Rows[i]["creat_appr_flg"].ToString(),
                        //      usr_creatd_id = Convert.ToDateTime(dt.Rows[i]["updated"].ToString()),
                        remarks = dt.Rows[i]["remarks"].ToString(),
                        comp_id = Convert.ToInt32(dt.Rows[i]["comp_id"].ToString()),
                        role_id = Convert.ToInt32(dt.Rows[i]["role_id"].ToString()), 


                         row_version = Convert.ToInt32(dt.Rows[i]["row_version"]),
                         record_status = Convert.ToInt32(dt.Rows[i]["record_status"]),
                    };
                }

            }
            return tg;
        }

       

        public string base64Decode(string sData) //Decode    
        {
            try
            {
                var encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }


        public async Task<string> DeleteUsersbyId(int id)
        {


            try
            {




                DbContextHelper db = new DbContextHelper();

                // NpgsqlCommand cmd = new NpgsqlCommand("update certifications set record_status = 1 where certifications_id ='" + id + "'");
                // int i = db.doinsertupdatedelete(cmd);

                string query = "update users set record_status = 1 where id ='" + id + "'";

                // int i =  await db.doinsertupdatedelete(query);
                int i = await db.doinsertupdatedelete(query);

                if (i > 0)
                {

                    return "Data Delete Successfully";

                }
                else
                {
                    return "Data not Delete Successfully";

                }

            }
            catch (Exception ex)
            {
                return "Data not Delete Successfully";
            }

            return "Data Delete Successfully";


        }





        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<string> GetEmployeeCode(string username)
        {
            string employee_code = "";
            try
            {   DbContextHelper db = new DbContextHelper();
                // NpgsqlCommand cmd = new NpgsqlCommand("update certifications set record_status = 1 where certifications_id ='" + id + "'");
                // int i = db.doinsertupdatedelete(cmd);
                string query = @"select employee_code from employees where employee_id =(select employee_id from users where username='" + username+"')";
                DataTable dt= db.SelectDataTable(query);
                if (dt != null && dt.Rows.Count != 0) {
                    employee_code= dt.Rows[0][0].ToString();
                }


            }
            catch (Exception ex)
            {
                return "";
            }

            return employee_code;
        }
    }
}
