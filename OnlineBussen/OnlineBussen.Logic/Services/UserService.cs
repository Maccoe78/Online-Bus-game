using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBussen.Data.Interfaces;
using OnlineBussen.Data.Models;
using OnlineBussen.Logic.Interfaces;
using OnlineBussen.Logic.Models;

namespace OnlineBussen.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var userDto = await _userRepository.GetUserByUsernameAsync(username);
            return new User
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
                Password = userDto.Password,
                CreatedDate = userDto.CreatedDate,

            };
        }

        public async Task<User> GetUserByCredentialsAsync(string username, string password)
        {
            var userDto = await _userRepository.GetUserByCredentialsAsync(username, password);
            if (userDto == null)
            {
                return null;
            }

            return new User
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
                Password = userDto.Password,
                CreatedDate = userDto.CreatedDate
            };
        }
        public async Task CreateUserAsync(User user)
        {
            var userDto = new UserDTO
            {
                
                Username = user.Username,
                Password = user.Password,
                CreatedDate = user.CreatedDate
            };
            await _userRepository.CreateUserAsync(userDto);
        }
        public async Task UpdateUserAsync(User user)
        {
            var userDto = new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Password = user.Password,
                CreatedDate = user.CreatedDate
            };
            await _userRepository.UpdateUserAsync(userDto);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userRepository.UsernameExistsAsync(username);
        }
    }
}
