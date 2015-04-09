using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Authentication
{
    public interface IAuthenticator
    {
        Guid AuthenticateUser(string userName, string password);
    }
}
