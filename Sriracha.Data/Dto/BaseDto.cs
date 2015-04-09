using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Dto
{
    public class BaseDto 
    {
        public Guid Id { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public string UpdatedByUserName { get; set; }
        public DateTime UpdatedDateTimeUtc { get; set; }

        public void SetCreatedFields(string userName)
        {
            this.CreatedByUserName = userName;
            this.CreatedDateTimeUtc = DateTime.UtcNow;
            this.UpdatedByUserName = userName;
            this.UpdatedDateTimeUtc = DateTime.UtcNow;
        }

        public void SetUpdatedFields(string userName)
        {
            this.UpdatedByUserName = userName;
            this.UpdatedDateTimeUtc = DateTime.UtcNow;
        }
    }
}
