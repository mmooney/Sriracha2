using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Utility
{
    public interface IProcessRunner
    {
        int Run(string executablePath, string executableParameters, TextWriter standardOutputWriter, TextWriter errorOutputWriter, string workingDirectory = null);
        int RunAsUser(string executablePath, string executableParameters, TextWriter standardOutputWriter, TextWriter errorOutputWriter, string domain, string userName, SecureString password);
        int RunAsToken(string executablePath, string executableParameters, TextWriter standardOutputWriter, TextWriter errorOutputWriter, SafeHandle impersonationToken);
    }
}
