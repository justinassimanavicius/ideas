using IdeasAPI.Models;

namespace IdeasAPI.Helpers
{
    public interface IUserRepository
    {
        User GetUser(string userName);
    }
}