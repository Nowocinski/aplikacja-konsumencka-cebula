using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApplication.Core.Domain;
using WebApplication.Core.Domain.Context;
using WebApplication.Infrastructure.Repositories;
using Xunit;

namespace Tests
{
    public class UserRepositoryTests
    {
        private DataBaseContext FakeDatabase()
        {
            DbContextOptions<DataBaseContext> options = new DbContextOptionsBuilder<DataBaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            return new DataBaseContext(options);
        }

        [Fact]
        public async Task Get_user_by_email_address_that_is_not_in_the_database_will_be_returned_null()
        {
            DataBaseContext context = FakeDatabase();
            User user = new User("testFirstName", "testLastName", "505303101", "test@test.com", "secret");
            UserRepository userRepository = new UserRepository(context);

            User new_user = await userRepository.GetAsync(user.Email);

            Assert.Null(new_user);
        }

        [Theory]
        [InlineData("Jon", "Snow", "505303101", "jon@snow.com", "secret")]
        [InlineData("Rob", "Ford", "404303202", "Rob-Ford@test.com", "zeqbnM#")]
        public async Task Correctly_adding_a_new_user_to_the_database(string FirstName,
            string LastName, string Phone, string Email, string Password)
        {
            DataBaseContext context = FakeDatabase();
            User user = new User(FirstName, LastName, Phone, Email, Password);
            UserRepository userRepository = new UserRepository(context);

            await userRepository.AddAsync(user);
            User any_user = await context.Users.SingleOrDefaultAsync(x => x.Email == user.Email);

            Assert.Equal(user, any_user);
        }

        [Theory]
        [InlineData("jon#snow.com")]
        [InlineData("jon@@snow.com")]
        [InlineData("-jon@@snow.com")]
        [InlineData("jon-@@snow.com")]
        [InlineData("jon@@snow..com")]
        public async Task Return_an_exception_after_trying_to_add_a_new_user_with_an_incorrect_email_adress(string Email)
        {
            UserRepository userRepository = new UserRepository(FakeDatabase());

            await Assert.ThrowsAsync<Exception>(async () =>
                await userRepository.AddAsync(new User("Jon", "Snow", "505303101", Email, "secret")));
        }

        [Theory]
        [InlineData("", "Snow", "505303101", "jon@snow.com", "secret")]
        [InlineData("Jon", "", "505303101", "jon@snow.com", "secret")]
        [InlineData("Jon", "Snow", "", "jon@snow.com", "secret")]
        [InlineData("Jon", "Snow", "505303101", "", "secret")]
        [InlineData("Jon", "Snow", "505303101", "jon@snow.com", "")]
        public async Task Return_an_exception_after_trying_to_add_a_new_user_with_an_empty_field
            (string FirstName, string LastName, string Phone, string Email, string Password)
        {
            DataBaseContext context = FakeDatabase();
            UserRepository userRepository = new UserRepository(context);

            await Assert.ThrowsAsync<Exception>(async () =>
                await userRepository.AddAsync(new User(FirstName, LastName, Phone, Email, Password)));
        }

        [Fact]
        public async Task Return_an_exception_after_attempting_to_add_an_existing_account_in_the_database()
        {
            DataBaseContext context = FakeDatabase();
            User user = new User("testFirstName", "testLastName", "505303101", "test@test.com", "secret");
            UserRepository userRepository = new UserRepository(context);

            await userRepository.AddAsync(user);

            await Assert.ThrowsAsync<ArgumentException>(async () => await userRepository.AddAsync(user));
        }
    }
}
