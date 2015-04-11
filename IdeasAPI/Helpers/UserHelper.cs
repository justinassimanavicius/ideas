using System;
using System.DirectoryServices;
using System.Security.Principal;
using IdeasAPI.Models;

namespace IdeasAPI.Helpers
{
    public static class UserHelper
    {
        public static string GetUserNameFromIdentity(IIdentity id)
        {
            if (id == null) return "";
            return GetUserNameFromComplexUsername(id.Name);
        }

        public static string GetUserNameFromComplexUsername(string domainName)
        {
            if (domainName.Split('\\').Length > 1) return domainName.Split('\\')[1];
            return domainName;
        }

        public static User GetUserFromADByUserName(string userName)
        {
            return GetUserFromAD(userName);
        }

        /// <summary>
        /// Private method for retrieving user information from active directory
        /// </summary>
        /// <param name="id">current user identity</param>
        /// <param name="userName">domain username</param>
        /// <returns>User object</returns>
        private static User GetUserFromAD(string userName)
        {
            var result = new User();

            var domain = "WEBMEDIA";

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