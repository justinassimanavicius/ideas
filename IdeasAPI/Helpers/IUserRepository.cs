using IdeasAPI.Models;

namespace IdeasAPI.Helpers
{
    public interface IUserRepository
    {
        LdapUser GetUser(string userName);
    }
}