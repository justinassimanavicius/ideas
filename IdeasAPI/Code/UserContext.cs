using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using IdeasAPI.DataContexts;
using IdeasAPI.Helpers;
using IdeasAPI.Models;

namespace IdeasAPI.Code
{
    public class UserContext
    {
        private static readonly MemoryCache Cache = new MemoryCache("IdeasUsers");
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly object _writeLock = new {};

        public UserContext(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            var c = new MapperConfiguration(x => x.CreateMap<LdapUser, User>());
            _mapper = c.CreateMapper();
        }

        #region LdapUser info

        public virtual User GetUser(string userName)
        {
            var key = "accountInfo_" + userName;

            var cachedUser = Cache[key];
            if (cachedUser != null) return cachedUser as User;


            return LoadLdapUser(userName, key);
        }

        private User LoadLdapUser(string userName, string key)
        {
            lock (_writeLock)
            {
                User user;

                using (var db = new IdeasDb())
                {
                    user = db.Users.FirstOrDefault(x => x.DomainName == userName);
                    if (user == null)
                    {
                        user = db.Users.Create();
                        user.CreatedDate = DateTime.Now;
                        db.Users.Add(user);
                    }
                    if (user.LastUpdateDate < DateTime.Now.AddDays(-1))
                    {
                        var ldapUser = _userRepository.GetUser(userName);
                        _mapper.Map(ldapUser, user);
                        user.DomainName = userName;
                        user.LastUpdateDate = DateTime.Now;
                    }
                    
                    db.SaveChanges();
                }


                UpdateCacheElement(key, user, 3600);

                return user;
            }
        }

        #endregion

        private static void UpdateCacheElement(string key, object obj, int expirationInSeconds)
        {
            if (Cache[key] == null)
            {
                Cache.Add(key, obj, DateTime.Now.AddSeconds(expirationInSeconds));
            }
            else
            {
                Cache.Set(key, obj, DateTime.Now.AddSeconds(expirationInSeconds));
            }
        }

    }
}