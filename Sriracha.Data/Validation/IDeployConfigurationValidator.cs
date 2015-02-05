using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Validation
{
    public interface IDeployConfigurationValidator
    {
        void ValidateConfiguration(object configObject);
        object ApplyDefaults(object configObject);

        object ValidateAndApplyDefaults(object configObject);
    }
}
