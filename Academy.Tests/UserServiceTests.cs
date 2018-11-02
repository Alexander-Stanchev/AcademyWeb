using Academy.Data;
using Academy.DataContext;
using Academy.Services;
using demo_db.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Academy.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public async Task GetUserByIdShouldReturnUser()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetUserByIdShouldReturnUser")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev",
                    RoleId = 2
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                var user = await userService.GetUserByIdAsync(1);

                Assert.IsTrue(user.UserName == "pesho12");

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task GetUserByIdShouldThrowExceptionWhenIdNotValid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetUserByIdShouldThrowExceptionWhenIdNotValid")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev",
                    RoleId = 2
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                var user = await userService.GetUserByIdAsync(-1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetUserByIdShouldThrowExceptionWhenIdNotFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetUserByIdShouldThrowExceptionWhenIdNotFound")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev",
                    RoleId = 2
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                var user = await userService.GetUserByIdAsync(10);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task UpdatedRoleShouldThrowExceptionWhenUserIdNegative()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "UpdatedRoleShouldThrowExceptionWhenUserIdNegative")
                .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                var user = await userService.GetUserByIdAsync(-1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectPermissionsException))]
        public async Task GetUserByIdShouldThrowExceptionWhenPermissionNotValid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetUserByIdShouldThrowExceptionWhenPermissionNotValid")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev",
                    RoleId = 2
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.UpdateRoleAsync(1,1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task UpdateRoleShouldThrowExceptionWhenUserNotFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "UpdateRoleShouldThrowExceptionWhenUserNotFound")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev",
                    RoleId = 2
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.UpdateRoleAsync(10, 3);
            }
        }
    }
}
