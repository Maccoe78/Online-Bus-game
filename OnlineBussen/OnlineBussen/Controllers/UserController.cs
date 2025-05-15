using OnlineBussen.Models;
using OnlineBussen.Interfaces;

namespace OnlineBussen.Controllers
{
    public class UserController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            return await _userRepository.GetUserByCredentialsAsync(username, password);
        }

        public async Task CreateUserAsync(string username, string password)
        {
            var user = new User
            {
                Username = username,
                Password = password,
                CreatedDate = DateTime.Now
            };

            await _userRepository.CreateUserAsync(user);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
