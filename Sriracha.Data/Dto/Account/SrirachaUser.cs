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

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
