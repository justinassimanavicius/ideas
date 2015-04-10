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
    public static class UserContext
    {
        private static readonly MemoryCache _cache = new MemoryCache("Narsyk_Users");

        #region User info

        public static User GetUserInformation(IIdentity identity)
        {
            return GetUserInfoFromAD(identity, null);
        }

        public static User GetUserInformationByUserName(IIdentity identity, string userName)
        {
            return GetUserInfoFromAD(identity, userName);
        }

        private static User GetUserInfoFromAD(IIdentity identity, string userName)
        {
            var key = "accountInfo_" + (userName ?? identity.Name);

            if (_cache[key] != null) return _cache[key] as User;

            try
            {
                if (userName != null)
                {
                    UpdateCacheElement(key, UserHelper.GetUserFromADByUserName(identity, userName), 3600);
                }
                else
                {
                    UpdateCacheElement(key, UserHelper.GetCurrentUserFromAD(identity), 3600);
                }
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