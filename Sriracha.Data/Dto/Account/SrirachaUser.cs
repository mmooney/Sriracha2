using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Dto.Account
{
    public class SrirachaUser : BaseDto
    {
        public string UserName { get; set; }
        public string EncryptedPassword { get; set; }
    }
}
