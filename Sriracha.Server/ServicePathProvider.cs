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
        public string GetRootPath()
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string webPath = AppSettingsHelper.GetRequiredSetting("WebPath");
            var fullWebPath = Path.GetFullPath(Path.Combine(currentDirectory, webPath));
            if(!Directory.Exists(fullWebPath))
            {
                throw new Exception("Web Path " + fullWebPath + " does not exist");
            }
            return fullWebPath;
        }
    }
}
