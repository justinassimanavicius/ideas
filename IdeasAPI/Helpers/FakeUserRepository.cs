using IdeasAPI.Models;

namespace IdeasAPI.Helpers
{
    public class FakeUserRepository : IUserRepository
    {
        public LdapUser GetUser(string userName)
        {
            return new LdapUser
            {
                DomainName = userName,
                Name = userName,
                ShortName = userName,
                Email = userName+"@"+userName+".com"
            };
        }
    }
}