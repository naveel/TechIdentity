namespace HBL_MLDV_APP.Models.Security
{
    public class UserAccountModel
    {
        public string UserFullName { get; set; }
        public string Password { get; set; }
        public string Password_Enc { get; set; }
        public string Username { get; set; }
        public string Username_Enc { get; set; }
        public string EmployeeCode { get; set; }
        public long? UserId { get; set; }
        public long? EmployeeID { get; set; }
        public string email { get; set; }

        public string Token { get; set; }
        public int br_sk { get; set; }
    }
}