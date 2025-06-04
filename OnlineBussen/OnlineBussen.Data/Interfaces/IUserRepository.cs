using OnlineBussen.Data.Models;

namespace OnlineBussen.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<UserDTO> GetUserByCredentialsAsync(string username, string password);
        Task CreateUserAsync(UserDTO user);
        Task UpdateUserAsync(UserDTO user);
        Task<bool> UsernameExistsAsync(string Username);
    }
}
