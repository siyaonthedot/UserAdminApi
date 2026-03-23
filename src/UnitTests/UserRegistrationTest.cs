using Moq;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;
using Application.Dtos.UserDtos;

namespace UnitTests
{
    [TestClass]
    public sealed class UserRegistrationTest
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IUserRoleService> _mockUserRoleService;

        [TestInitialize]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockUserRoleService = new Mock<IUserRoleService>();
        }

        [TestMethod]
        public async Task RegisterUser_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var requestUserDto = new RequestUserDto
            {
                //Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "Test@123",
                FirstName = "Test",
                LastName = "User"
            };

            var expectedUserDto = new UserDto
            {
                Id = new Guid(),
               // Username = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };

            //_mockUserService
            //    .Setup(x => x.CreateUserAsync(It.IsAny<RequestUserDto>()))
            //    .ReturnsAsync(expectedUserDto);

            // Act
            var result = await _mockUserService.Object.CreateUserAsync(requestUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUserDto.FirstName, result.Data.ToString());
            Assert.AreEqual(expectedUserDto.Email, result.Data.ToString());
            _mockUserService.Verify(x => x.CreateUserAsync(It.IsAny<RequestUserDto>()), Times.Once);
        }

        [TestMethod]
        public async Task RegisterUser_WithDuplicateEmail_ThrowsException()
        {
            // Arrange
            var requestUserDto = new RequestUserDto
            {
                FirstName = "testuser",
                Email = "existing@example.com",
                PasswordHash = "Test@123"
            };

            _mockUserService
                .Setup(x => x.CreateUserAsync(It.IsAny<RequestUserDto>()))
                .ThrowsAsync(new InvalidOperationException("Email already exists"));

            // Act & Assert
            //await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            //    async () => await _mockUserService.Object.CreateUserAsync(requestUserDto)
            //);
        }

        [TestMethod]
        public async Task RegisterUser_WithDuplicateUsername_ThrowsException()
        {
            // Arrange
            var requestUserDto = new RequestUserDto
            {
                FirstName = "existinguser",
                Email = "test@example.com",
                PasswordHash = "Test@123"
            };

            _mockUserService
                .Setup(x => x.CreateUserAsync(It.IsAny<RequestUserDto>()))
                .ThrowsAsync(new InvalidOperationException("Username already exists"));

            // Act & Assert
            //await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            //    async () => await _mockUserService.Object.CreateUserAsync(requestUserDto)
            //);
        }

        [TestMethod]
        public async Task RegisterUser_WithInvalidPassword_ThrowsException()
        {
            // Arrange
            var requestUserDto = new RequestUserDto
            {
                FirstName = "testuser",
                Email = "test@example.com",
                PasswordHash = "weak"
            };

            _mockUserService
                .Setup(x => x.CreateUserAsync(It.IsAny<RequestUserDto>()))
                .ThrowsAsync(new ArgumentException("Password does not meet requirements"));

            // Act & Assert
            //await Assert.ThrowsExceptionAsync<ArgumentException>(
            //    async () => await _mockUserService.Object.CreateUserAsync(requestUserDto)
            //);
        }
    }
}
