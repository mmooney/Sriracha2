using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sriracha.Data.Identity.Impl
{
    public class WebSrirachaIdentity : ISrirachaIdentity
    {
        private readonly Nancy.NancyContext _context;

        public WebSrirachaIdentity(Nancy.NancyContext context)
        {
            _context = context;    
        }

        public string UserName
        {
            get 
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null
                        && HttpContext.Current.User.Identity != null
                        && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return HttpContext.Current.User.Identity.Name;
                }
                else if (_context != null && _context.CurrentUser != null 
                        && !string.IsNullOrEmpty(_context.CurrentUser.UserName))
                {
                    return _context.CurrentUser.UserName;
                }
                else
                {
                    return Environment.UserName;
                }
            }
        }
    }
}
