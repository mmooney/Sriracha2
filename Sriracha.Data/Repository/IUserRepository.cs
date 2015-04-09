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
        SrirachaUser TryGetUserByUserNameAndPassword(string userName, string encryptedPassword);
        SrirachaUser GetUserById(Guid identifier);
    }
}
