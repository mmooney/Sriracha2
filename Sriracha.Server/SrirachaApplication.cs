using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Server
{
    public class SrirachaApplication
    {
        private IDisposable _webApplication;

        public void Start()
        {
            _webApplication = WebApp.Start<WebPipeline>("http://localhost:5000");
        }

        public void Stop()
        {
            try 
            {
                using(_webApplication)
                {
                    _webApplication = null;
                }
            }
            catch {}
        }
    }
}
