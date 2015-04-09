using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using Sriracha.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.NancyFX
{
    public class NancyUserMapper : IUserMapper
    {
        private readonly IUserRepository _userRepository;

        public NancyUserMapper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user = _userRepository.GetUserById(identifier);
            return new NancyUserIdentity(user);
        }
    }
}
