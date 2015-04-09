using Sriracha.Data.Dto.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Repository
{
    public interface IUserRepository
    {
        SrirachaUser TryGetUserByUserName(string userName);
        SrirachaUser GetUserById(Guid identifier);

        SrirachaUser CreateUser(string userName, string emailAddress, string password, string firstName, string lastName);
    }
}
