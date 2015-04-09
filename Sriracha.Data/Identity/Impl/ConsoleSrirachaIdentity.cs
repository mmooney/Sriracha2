using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Identity.Impl
{
    public class ConsoleSrirachaIdentity : ISrirachaIdentity
    {
        public string UserName
        {
            get { return Environment.UserName; }
        }
    }
}
