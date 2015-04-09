using Nancy;
using Sriracha.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sriracha.Web
{
    public class SetupModule : NancyModule
    {
        private readonly IUserManager _userManager;

        public SetupModule(IUserManager userManager)
        {
            _userManager = userManager;

            Get["/setup"] = _ => 
            {
                var adminUser = _userManager.TryGetUserByUserName("administrator");
                if(adminUser == null)
                {
                    adminUser = _userManager.CreateUser("administrator", "test@mmdbsolutions.com", "password123!", "Admin", "User");
                    return "Setup complete";
                }
                else 
                {
                    return "Already setup";
                }
            };
        }
    }
}