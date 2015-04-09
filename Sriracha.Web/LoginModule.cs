using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;
using Sriracha.Data.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sriracha.Web
{
    public class LoginModule : NancyModule
    {
        private readonly IAuthenticator _authenticator;

        public LoginModule(IAuthenticator authenticator) : base("login")
        {
            _authenticator = authenticator;

            Get["/"] = _ => View["index"];
            Post["/"] = _ => 
            {  
                string userName = this.Request.Form.userName;
                string password = this.Request.Form.password;
                var userID = _authenticator.AuthenticateUser(userName, password);
                return this.LoginAndRedirect(userID);
            };
        }
    }
}