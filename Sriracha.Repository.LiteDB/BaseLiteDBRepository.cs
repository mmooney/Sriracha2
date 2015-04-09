using LiteDB;
using MMDB.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sriracha.Repository.LiteDB
{
    public abstract class BaseLiteDBRepository<T> where T:new()
    {
        public static string _dbPath;

        static BaseLiteDBRepository()
        {
            var dbDirectory = AppSettingsHelper.GetSetting("LiteDBDirectory");
            if(string.IsNullOrEmpty(dbDirectory))
            {
                var exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                dbDirectory = Path.Combine(exeDirectory, "app_data");
            }
            if(!Path.IsPathRooted(dbDirectory))
            {
                dbDirectory = Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, dbDirectory);
            }
            dbDirectory = Path.GetFullPath(dbDirectory);
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }
            _dbPath = Path.Combine(dbDirectory, "sriracha.db");
        }

        protected LiteDatabase GetDB()
        {
            return new LiteDatabase(_dbPath);
        }

        protected abstract LiteCollection<T> EnsureIndexes(LiteCollection<T> collection);

        protected LiteCollection<T> GetCollection(LiteDatabase db)
        {
            var collection = db.GetCollection<T>(typeof(T).Name);
            return this.EnsureIndexes(collection);
        }
    }
}
