using LiteDB;
using MMDB.Shared;
using Sriracha.Data.Dto.Account;
using Sriracha.Data.Identity;
using Sriracha.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Repository.LiteDB
{
    public class LiteDBUserRepository : BaseLiteDBRepository<SrirachaUser>, IUserRepository
    {
        private readonly ISrirachaIdentity _identity;

        public LiteDBUserRepository(ISrirachaIdentity identity)
        {
            _identity = identity;
        }

        protected override LiteCollection<SrirachaUser> EnsureIndexes(LiteCollection<SrirachaUser> collection)
        {
            collection.EnsureIndex(x => x.UserName, true);
            return collection;
        }

        public SrirachaUser TryGetUserByUserName(string userName)
        {
            using(var db = this.GetDB())
            {
                var collection = db.GetCollection<SrirachaUser>("SrirachaUser");
                
                var user = collection.FindOne(x=>x.UserName == userName);
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

        public SrirachaUser CreateUser(string userName, string emailAddress, string encryptedPassword, string firstName, string lastName)
        {
            using (var db = this.GetDB())
            {
                var collection = db.GetCollection<SrirachaUser>("SrirachaUser");
                var user = new SrirachaUser
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    EmailAddress = emailAddress,
                    EncryptedPassword = encryptedPassword, 
                    FirstName = firstName,
                    LastName = lastName,
                };
                user.SetCreatedFields(_identity.UserName);
                collection.Insert(user);
                db.Commit();
                return user;
            }
        }
    }
}
