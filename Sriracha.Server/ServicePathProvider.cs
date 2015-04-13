using MMDB.Shared;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sriracha.Server
{
    public class ServicePathProvider : IRootPathProvider
    {
        private readonly string _webPath;
        public ServicePathProvider(string webPath)
        {
            _webPath = webPath;
        }

        public string GetRootPath()
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullWebPath = Path.GetFullPath(Path.Combine(currentDirectory, _webPath));
            if(!Directory.Exists(fullWebPath))
            {
                throw new Exception("Web Path " + fullWebPath + " does not exist");
            }
            return fullWebPath;
        }
    }
}
