using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Impersonation
{
    public interface IImpersonator
    {
        ImpersonationContext BeginImpersonation(string userName, string password, string domain=null);
    }
}
