using Nancy.Security;
using Sriracha.Data.Dto.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Nancy
{
    public class NancyUserIdentity : IUserIdentity
    {
        private readonly SrirachaUser _user;

        public IEnumerable<string> Claims { get { return null; } }
        public string UserName { get { return _user.UserName; }}

        public NancyUserIdentity(SrirachaUser user)
        {
            _user = user;
        }
    }
}
