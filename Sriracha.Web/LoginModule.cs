using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sriracha.Data.Managers;

namespace Sriracha.Web
{
    public class LoginModule : NancyModule
    {
        public class LoginViewModel
        {
            public string ErrorMessage { get; set; }
        }

        private readonly IUserManager _authenticator;

        public LoginModule(IUserManager authenticator) : base("login")
        {
            _authenticator = authenticator;

            Get["/"] = _ => View["index"];
            Post["/"] = _ => 
            {  
                string userName = this.Request.Form.userName;
                string password = this.Request.Form.password;
                var user = _authenticator.AuthenticateUser(userName, password);
                if(user == null)
                {
                    return View["index", new LoginViewModel { ErrorMessage = "Login Failed" }]; 
                }
                return this.LoginAndRedirect(user.Id);
            };
        }
    }
}