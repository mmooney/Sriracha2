using Sriracha.Data.Dto.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Managers
{
    public interface IUserManager
    {
        SrirachaUser CreateUser(string userName, string emailAddress, string password, string firstName, string lastName);
        SrirachaUser TryGetUserByUserName(string userName);

        SrirachaUser AuthenticateUser(string userName, string password);
    }
}
