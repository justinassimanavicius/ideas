using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;
using System.Collections;
using System.Drawing;
using System.IO;
using IdeasAPI.Models;

namespace IdeasAPI.Helpers
{
    public static class UserHelper
    {
        public static string GetUserNameFromIdentity(IIdentity id)
        {
            if (id == null) return "";
            return id.Name.Split('\\')[1] ?? id.Name;
        }

        public static string GetUserNameFromComplexUsername(string domainName)
        {
            if (domainName.Split('\\').Length > 1) return domainName.Split('\\')[1];
            return domainName;
        }

        public static User GetCurrentUserFromAD(IIdentity id)
        {
            return GetUserFromAD(id, null);
        }

        public static User GetUserFromADByUserName(IIdentity id, string userName)
        {
            return GetUserFromAD(id, userName);
        }

        /// <summary>
        /// Private method for retrieving user information from active directory
        /// </summary>
        /// <param name="id">current user identity</param>
        /// <param name="userName">domain username</param>
        /// <returns>User object</returns>
        private static User GetUserFromAD(IIdentity id, string userName)
        {
            var result = new User();

            var winId = id as WindowsIdentity;

            if (winId == null)
            {
                Console.WriteLine("Identity is not a windows identity");
                return result;
            }

            userName = userName ?? winId.Name.Split('\\')[1];
            var domain = winId.Name.Split('\\')[0];

            var entry = new DirectoryEntry("LDAP://" + domain);
            var adSearcher = new DirectorySearcher(entry)
            {
                SearchScope = SearchScope.Subtree,
                Filter = "(&(objectClass=user)(samaccountname=" + userName + "))"
            };

            var userObject = adSearcher.FindOne();

            if (userObject == null) return result;

            result.Name = userObject.Properties["displayname"][0].ToString();
            result.ShortName = userObject.Properties["givenname"][0].ToString();
            result.Thumbnail = userObject.Properties["thumbnailphoto"][0] as byte[];
            result.BirthDay = DateTime.Parse(userObject.Properties["extensionattribute1"][0].ToString());
            result.Email = userObject.Properties["mail"][0].ToString();
            result.DomainName = userObject.Properties["samaccountname"][0].ToString();

            return result;
        }
    }
}