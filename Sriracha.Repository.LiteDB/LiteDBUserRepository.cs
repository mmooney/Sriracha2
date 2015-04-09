using MMDB.Shared;
using Sriracha.Data.Dto.Account;
using Sriracha.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Repository.LiteDB
{
    public class LiteDBUserRepository : BaseLiteDBRepository, IUserRepository
    {
        public SrirachaUser TryGetUserByUserNameAndPassword(string userName, string encryptedPassword)
        {
            using(var db = this.GetDB())
            {
                var collection = db.GetCollection<SrirachaUser>("SrirachaUser");
                collection.EnsureIndex(x=>x.UserName);
                
                var user = collection.FindOne(x=>x.UserName == userName && x.EncryptedPassword == encryptedPassword);
                return user;
            }
        }

        public SrirachaUser GetUserById(Guid id)
        {
            using(var db = this.GetDB())
            {
                var collection = db.GetCollection<SrirachaUser>("SrirachaUser");
                collection.EnsureIndex(x=>x.UserName);
                
                var user = collection.FindById(id);
                if(user == null)
                {
                    throw new RecordNotFoundException<SrirachaUser>(id);
                }
                return user;
            }
        }
    }
}
