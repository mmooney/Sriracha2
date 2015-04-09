using Sriracha.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Authentication.Impl
{
    public class Authenticator : IAuthenticator
    {
        private readonly IUserRepository _userRepository;

        public Authenticator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Guid AuthenticateUser(string userName, string password)
        {
            var user = _userRepository.TryGetUserByUserNameAndPassword(userName, password);
            return Guid.NewGuid();
        }
    }
}
