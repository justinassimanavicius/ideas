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
    }
}