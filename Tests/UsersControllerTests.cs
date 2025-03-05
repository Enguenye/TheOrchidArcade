using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Controllers;
using TheOrchidArchade.Models;
using TheOrchidArchade.Context;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.AspNetCore.DataProtection;
using TheOrchidArchade.Utils;
using System.Security.Claims;

namespace TheOrchidArchade.Tests
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly ILogger<UsersController> _logger;
        private readonly EncryptionHelper _encryptionHelper;

        public UsersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null);

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<User, string>((user, password) =>
            {
                _context.users.Add(user);
                _context.SaveChanges();
            });


            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);

            var mockLogger = new Mock<ILogger<UsersController>>();
            _logger = mockLogger.Object;

            var dataProtectionProvider = new Mock<IDataProtectionProvider>();
            var dataProtector = new Mock<IDataProtector>();
            dataProtector.Setup(p => p.Protect(It.IsAny<byte[]>())).Returns<byte[]>(b => b);
            dataProtector.Setup(p => p.Unprotect(It.IsAny<byte[]>())).Returns<byte[]>(b => b); 
            dataProtectionProvider.Setup(p => p.CreateProtector(It.IsAny<string>())).Returns(dataProtector.Object);
            _encryptionHelper = new EncryptionHelper(dataProtectionProvider.Object);

            _context = new ApplicationDbContext(options);
            _controller = new UsersController(_context, _signInManagerMock.Object, _userManagerMock.Object, _logger, _encryptionHelper);
        }


        [Fact]
        public async Task GetUserDetailsTest()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var userId2 = Guid.NewGuid().ToString();
            var user = new User { Id = userId, Email = "test@example.com", UserName = "testuser1", isDeveloper = false, creditCardNumber = _encryptionHelper.Encrypt("12345678912312") };
            var user2 = new User { Id = userId2, Email = "test2@example.com", UserName = "testuser2", isDeveloper = true, creditCardNumber = _encryptionHelper.Encrypt("12345678912312") };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var result = await _controller.Details(userId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UserDetailsViewModel>(viewResult.Model);
            Assert.Equal(userId, model.User.Id);
            Assert.Equal("test@example.com", model.User.Email);
            Assert.Equal("testuser1", model.User.UserName);
            Assert.False(model.User.isDeveloper);
        }

        [Fact]
        public async Task CreateUserTest()
        {
            var newUser = new User { Id = Guid.NewGuid().ToString(), Email = "test3@example.com", UserName = "testuser3", isDeveloper = true, creditCardNumber="12345678912312" };
            var result = await _controller.Create(newUser,"asfdfko-19As@");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Verify2FA", redirectResult.ActionName);

            var userInDb = await _context.users.FirstOrDefaultAsync(u => u.Id == newUser.Id); ;
            Assert.NotNull(userInDb);
            Assert.Equal(newUser.Email, userInDb.Email);
        }

        [Fact]
        public async Task DeleteUserTest()
        {
  
            var user = new User { Id = Guid.NewGuid().ToString(), Email = "test4@example.com", UserName = "testuser4", isDeveloper = false };
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var result = await _controller.DeleteConfirmed(user.Id);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var userInDb = await _context.users.FindAsync(user.Id);
            Assert.Null(userInDb); 
        }

        [Fact]
        public async Task UpdateUserTest()
        {

            var user = new User { Id = Guid.NewGuid().ToString(), Email = "test5@example.com", UserName = "testuser5", isDeveloper = false };
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            _context.Entry(user).State = EntityState.Detached;

            var updatedUser = await _context.users.FindAsync(user.Id);
            updatedUser.Email = "updated5@example.com";
            updatedUser.UserName = "updateduser5";
            updatedUser.isDeveloper = true;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var result = await _controller.Edit(updatedUser.Id, updatedUser, "123");

            var userInDb = await _context.users.FindAsync(updatedUser.Id);
            Assert.NotNull(userInDb);
            Assert.Equal("updated5@example.com", userInDb.Email); 
            Assert.Equal("updateduser5", userInDb.UserName); 
            Assert.Equal(true, userInDb.isDeveloper); 
        }

    }
}
