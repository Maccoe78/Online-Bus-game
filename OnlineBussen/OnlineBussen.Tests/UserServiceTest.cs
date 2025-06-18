using Moq;
using Xunit;
using OnlineBussen.Data.Interfaces;
using OnlineBussen.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBussen.Data.Models;
using OnlineBussen.Logic.Models;

namespace OnlineBussen.Tests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task CreateUser_ReturnsSucces()
        {
            var user = new User
            {
                Username = "newuser",
                Password = "newpass",
                CreatedDate = DateTime.Now
            };

            await _userService.CreateUserAsync(user);

            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.Is<UserDTO>(dto =>
                dto.Username == user.Username &&
                dto.Password == user.Password &&
                dto.CreatedDate == user.CreatedDate
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ReturnsSucces()
        {
            var originalUser = new User
            {
                UserId = 1,
                Username = "oldusername",
                Password = "oldpassword",
                CreatedDate = DateTime.Now.AddDays(-30)
            };

            var updatedUser = new User
            {
                UserId = 1,
                Username = "newusername", 
                Password = "updatedpass", 
                CreatedDate = originalUser.CreatedDate 
            };

            //UserDTO capturedUserDTO = null;
            //_mockUserRepository.Setup(repo => repo.UpdateUserAsync(It.IsAny<UserDTO>()))
            //    .Callback<UserDTO>(dto => capturedUserDTO = dto);

            await _userService.UpdateUserAsync(updatedUser);

            //Assert.NotNull(capturedUserDTO);
            //Assert.Equal(1, capturedUserDTO.UserId);
            //Assert.Equal("newusername", capturedUserDTO.Username); 
            //Assert.Equal("updatedpass", capturedUserDTO.Password); 
            //Assert.Equal(originalUser.CreatedDate, capturedUserDTO.CreatedDate);


            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(It.Is<UserDTO>(dto =>
                dto.UserId == 1 &&
                dto.Username == "newusername" &&
                dto.Password == "updatedpass"
            )), Times.Once);
        }


        [Fact]
        public async Task UsernameExistsAsync_WhenUsernameDoesNotExist()
        {
            var nonExistingUsername = "newuser";
            var existingUsername = "existinguser";

            _mockUserRepository.Setup(x => x.UsernameExistsAsync(existingUsername))
                .ReturnsAsync(false);

            var result = await _userService.UsernameExistsAsync(nonExistingUsername);

            Assert.False(result);
            _mockUserRepository.Verify(repo => repo.UsernameExistsAsync(nonExistingUsername), Times.Once);

        }
        [Fact]
        public async Task UsernameExistsAsync_WhenUsernameDoesExist()
        {
            var nonExistingUsername = "newuser";

            _mockUserRepository.Setup(x => x.UsernameExistsAsync(nonExistingUsername))
                .ReturnsAsync(true);

            var result = await _userService.UsernameExistsAsync(nonExistingUsername);

            
            Assert.True(result);

            _mockUserRepository.Verify(repo => repo.UsernameExistsAsync(nonExistingUsername), Times.Once);
        }


    }
}
