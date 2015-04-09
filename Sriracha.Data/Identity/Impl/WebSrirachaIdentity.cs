using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Identity.Impl
{
    public class WebSrirachaIdentity : ISrirachaIdentity
    {
        private readonly Nancy.Security.IUserIdentity _nancyUserIdentity;

        public WebSrirachaIdentity(Nancy.Security.IUserIdentity nancyUserIdentity)
        {
            _nancyUserIdentity = nancyUserIdentity;
        }

        public string UserName
        {
            get 
            { 
                if(_nancyUserIdentity != null)
                {
                    return _nancyUserIdentity.UserName;
                }
                else
                {
                    return "(None)";
                }
            }
        }
    }
}
