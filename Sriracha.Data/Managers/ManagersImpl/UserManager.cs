using Sriracha.Data.Dto.Account;
using Sriracha.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Sriracha.Data.Managers.ManagersImpl
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private string GetEncryptedPassword(string userName, string password)
        {
            return Crypto.HashPassword(userName.ToLower() + password);
        }

        public SrirachaUser AuthenticateUser(string userName, string password)
        {
            var user = _userRepository.TryGetUserByUserName(userName);
            if (user == null || !Crypto.VerifyHashedPassword(user.EncryptedPassword, userName.ToLower() + password))
            {
                return null;
            }
            else 
            {
                return user;
            }
        }

        public SrirachaUser CreateUser(string userName, string emailAddress, string password, string firstName, string lastName)
        {
            var encryptedPassword = GetEncryptedPassword(userName, password);
            return _userRepository.CreateUser(userName, emailAddress, encryptedPassword, firstName, lastName);
        }

        public SrirachaUser TryGetUserByUserName(string userName)
        {
            return _userRepository.TryGetUserByUserName(userName);
        }
    }
}
