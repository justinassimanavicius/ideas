using System.DirectoryServices;
using IdeasAPI.Models;

namespace IdeasAPI.Helpers
{
    public class UserRepository : IUserRepository
    {
        public User GetUser(string userName)
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

            result.Name = userObject.GetString("displayname");
            result.ShortName = userObject.GetString("givenname");
            result.Thumbnail = userObject.GetByteArray("thumbnailphoto");
            result.BirthDay = userObject.GetDateTime("extensionattribute1");
            result.Email = userObject.GetString("mail");
            result.DomainName = userObject.GetString("samaccountname");
            return result;
        }

    }
}