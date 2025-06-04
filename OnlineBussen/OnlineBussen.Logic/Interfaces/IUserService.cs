using OnlineBussen.Logic.Models;

namespace OnlineBussen.Logic.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByCredentialsAsync(string username, string password);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<bool> UsernameExistsAsync(string Username);
    }
}
