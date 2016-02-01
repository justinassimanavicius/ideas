using IdeasAPI.Models;

namespace IdeasAPI.Helpers
{
    public class FakeUserRepository : IUserRepository
    {
        public User GetUser(string userName)
        {
            return new User
            {
                DomainName = userName,
                Name = userName,
                ShortName = userName
            };
        }
    }
}