using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Deployment
{
    public interface IDeployStatusReporter
    {
        void Info(string message, params object[] args);
        void Debug(string message, params object[] args);
        void Error(string message, object[] args);
        void ErrorException(Exception err);
        void ErrorException(Exception err, string message, params object[] args);
    }
}
