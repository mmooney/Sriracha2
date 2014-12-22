using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.ServiceConfigurators;

namespace Sriracha.Server
{
    class Program
    {
        static int Main(string[] args)
        {
            var exitCode = HostFactory.Run(host =>
            {
                host.Service<SrirachaApplication>(service =>
                {
                    service.ConstructUsing(() => new SrirachaApplication());
                    service.WhenStarted(a => a.Start());
                    service.WhenStopped(a => a.Stop());
                });

                host.SetDescription("An application to manage sending sms messages and provide message status");
                host.SetDisplayName("Sriracha");
                host.SetServiceName("Sriracha");
                host.RunAsNetworkService();
            });
            return (int)exitCode;
        }
    }
}
