using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Nancy
{
    public class NancyUserIdentity : IUserIdentity
    {
        public IEnumerable<string> Claims { get; private set; }
        public string UserName { get; private set; }

        public NancyUserIdentity(string userName, IEnumerable<string> claims)
        {
            this.UserName = userName;
            if(claims != null)
            {
                this.Claims = claims.ToList();
            }
        }
    }
}
