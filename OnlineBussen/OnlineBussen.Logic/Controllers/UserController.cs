using OnlineBussen.Logic.Models;
using OnlineBussen.Logic.Interfaces;


namespace OnlineBussen.Logic.Controllers
{
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            return await _userService.GetUserByCredentialsAsync(username, password);
        }

        public async Task<(bool success, string message)> CreateUserAsync(string username, string password)
        {
            if (await _userService.UsernameExistsAsync(username))
            {
                return (false, "A User with this name already exists");
            }

            var user = new User
            {
                Username = username,
                Password = password,
                CreatedDate = DateTime.Now
            };

            await _userService.CreateUserAsync(user);
            return (true, "User succesfully created");
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userService.GetUserByUsernameAsync(username);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userService.UpdateUserAsync(user);
        }
    }
}
