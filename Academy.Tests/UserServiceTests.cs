using Academy.Data;
using Academy.DataContext;
using Academy.Services;
using demo_db.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task UpdateRoleThrowExceptionWhenPermissionNotValid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "UpdateRoleThrowExceptionWhenPermissionNotValid")
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

        [TestMethod]
        public async Task UpdateRoleShouldUpdateRoleWhenAllParametersAreValid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "UpdateRoleShouldUpdateRoleWhenAllParametersAreValid")
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
                await userService.UpdateRoleAsync(1, 3);
                var user = context.Users.FirstOrDefaultAsync(us => us.Id == 1);
                
                Assert.IsTrue(user.Result.RoleId == 3);
            }
        }

        [TestMethod]
        public async Task RetriveUsersShouldReturnUsers()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetriveUsersShouldThrowExceptionWhenNoUsersFound")
                .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev",
                    RoleId = 3
                });

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev",
                    RoleId = 3
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                var users = await userService.RetrieveUsers(3);
                var usersList = users.ToList();
                Assert.IsTrue(usersList.Count == 2);
            }
        }




    }
}
