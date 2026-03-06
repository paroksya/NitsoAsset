using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NitsoAsset.Models
{
    public class UserModel
    {
        public string comp_code { get; set; }
        public int user_pid { get; set; }
        public string username { get; set; }
        public int role_id { get; set; }
        public string company_pid { get; set; }
        public string location_fid { get; set; }
        public string company_fid { get; set; }
        public bool Active { get; set; }
        public string last_login_date { get; set; }
        public bool IsAppLogin { get; set; }
        public string Compname { get; set; }
    }

    public class LoginResponse : CommonResponse
    {
        public UserModel Response { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseStatus { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        //public string Access_Token { get; set; }
        //public string Token_type { get; set; }
        //public UserModel Data { get; set; }
    }
}