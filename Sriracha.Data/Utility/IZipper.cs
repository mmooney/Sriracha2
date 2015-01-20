using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Utility
{
    public interface IZipper
    {
        void ZipDirectory(string directoryPath, string zipPath);

        void ZipFile(string filePath, string zipPath);

        void ExtractFile(string zipPath, string targetDirectory);
    }
}
