using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using todo_list_back.Services;
using todo_list_back.Models;
using todo_list_back.Data;
using todo_list_back.DTOs;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace todo_list_back.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly IConfiguration _config;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();

            var inMemorySettings = new Dictionary<string, string> {
                {"JWT:Key", "SuperSecretKey_MustBeLong1234567890!@#"},
                {"JWT:Issuer", "todo_list_back"},
                {"JWT:Audience", "TaskClient"},
                {"JWT:DurationInMinutes", "60"}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task Register_ShouldCreateUser_WhenEmailIsUnique()
        {
            var service = new AuthService(_context, _passwordHasherMock.Object, _config);
            var dto = new RegisterDto { Name = "Jefer", Email = "unique@test.com", Password = "123456" };
            _passwordHasherMock
                .Setup(p => p.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("hashed-password");

            var user = await service.Register(dto);

            Assert.NotNull(user);
            Assert.Equal(dto.Email, user.Email);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var user = new User { Name = "Jefer", Email = "test@test.com", PasswordHash = "hashed" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _passwordHasherMock
                .Setup(p => p.VerifyHashedPassword(It.IsAny<User>(), "hashed", "123456"))
                .Returns(PasswordVerificationResult.Success);

            var service = new AuthService(_context, _passwordHasherMock.Object, _config);
            var dto = new LoginDto { Email = "test@test.com", Password = "123456" };

            var result = await service.Login(dto);

            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.True(result.ExpiresAt > DateTime.UtcNow);
        }
    }
}
