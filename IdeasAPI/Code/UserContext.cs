using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using IdeasAPI.Helpers;
using IdeasAPI.Models;

namespace IdeasAPI.Code
{
    public class UserContext
    {
        private static readonly MemoryCache _cache = new MemoryCache("IdeasUsers");

        #region User info

        public virtual User GetUserInfo(string userName)
        {
            var key = "accountInfo_" + userName;

            if (_cache[key] != null) return _cache[key] as User;

            try
            {
                    UpdateCacheElement(key, UserHelper.GetUserFromADByUserName(userName), 3600);
            }
            catch (Exception)
            {
                return null;
            }

            return _cache[key] as User;
        }

        #endregion

        private static void UpdateCacheElement(string key, object obj, int expirationInSeconds)
        {
            if (_cache[key] == null)
            {
                _cache.Add(key, obj, DateTime.Now.AddSeconds(expirationInSeconds));
            }
            else
            {
                _cache.Set(key, obj, DateTime.Now.AddSeconds(expirationInSeconds));
            }
        }

    }
}