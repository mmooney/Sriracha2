using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sriracha.Repository.LiteDB
{
    public abstract class BaseLiteDBRepository
    {
        public static string _dbPath;

        static BaseLiteDBRepository()
        {
            var exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dataPath = Path.Combine(exeDirectory, "app_data");
            if(!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            _dbPath = Path.Combine(dataPath, "sriracha.db");
        }

        protected LiteDatabase GetDB()
        {
            return new LiteDatabase(_dbPath);
        }
    }
}
