using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Authentication.Impl
{
    public class Authenticator : IAuthenticator
    {
        public Guid AuthenticateUser(string userName, string password)
        {
            return Guid.NewGuid();
        }
    }
}
