using AutoMapper;
using Moq;
using System;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.Services.User;
using WebApplication.Infrastructure.Services.User.JwtToken;
using Xunit;

namespace Tests
{
    public class UserServiceTests
    {
        [Theory]
        [InlineData("Jon", "Snow", "505303101", "jon@snow.com", "secret")]
        [InlineData("Rob", "Ford", "404303202", "Rob-Ford@test.com", "zeqbnM#")]
        public async Task Correctly_adding_a_new_user_to_the_database
            (string FirstName, string LastName, string PhoneNumber, string Email, string Password)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            var voivodeshipRepositoryMock = new Mock<IVoivodeshipRepository>();
            var userService = new UserService(userRepositoryMock.Object, jwtHandlerMock.Object, mapperMock.Object,
                voivodeshipRepositoryMock.Object);
            var data = new Register()
            {
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                Email= Email,
                Password = Password
            };

            await userService.RegisterAsync(data);

            userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [InlineData("jon#snow.com")]
        [InlineData("jon@@snow.com")]
        [InlineData("-jon@@snow.com")]
        [InlineData("jon-@@snow.com")]
        [InlineData("jon@@snow..com")]
        [InlineData("jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjon@@snow..com")]
        public async Task Return_an_exception_after_trying_to_add_a_new_user_with_an_incorrect_email_adress(string Email)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            var voivodeshipRepositoryMock = new Mock<IVoivodeshipRepository>();
            var userService = new UserService(userRepositoryMock.Object, jwtHandlerMock.Object, mapperMock.Object,
                voivodeshipRepositoryMock.Object);
            var data = new Register()
            {
                FirstName = "Jon",
                LastName = "Snow",
                PhoneNumber = "505303101",
                Email= Email,
                Password = "secret"
            };

            await Assert.ThrowsAsync<Exception>(async () => await userService.RegisterAsync(data));
        }
    }
}
