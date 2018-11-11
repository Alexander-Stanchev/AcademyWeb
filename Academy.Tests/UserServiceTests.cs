using Academy.Data;
using Academy.DataContext;
using Academy.Services;
using Academy.Services.Exceptions;
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
                var users = await userService.RetrieveUsersAsynca(3);
                var usersList = users.ToList();
                Assert.IsTrue(usersList.Count == 2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task EvaluateStudentShouldThrowExceptionIfStudentIdIsInvalid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
    .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowExceptionIfStudentIdIsInvalid")
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

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev",
                    RoleId = 3
                });
                context.Assignments.Add(new Assignment()
                {
                    Id = 1,
                    Name = "Grebane s lajica",
                    MaxPoints  = 100,
                    CourseId = 1
                });

                context.Courses.Add(new Course()
                {
                    CourseId = 1,
                    Name = "asdv"
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.EvaluateStudentAsync(78,1,45,1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task EvaluateStudentShouldThrowExceptionIfTeacherIdIsInvalid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
    .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowExceptionIfTeacherIdIsInvalid")
    .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev",
                    RoleId = 1
                });

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev",
                    RoleId = 3
                });
                context.Assignments.Add(new Assignment()
                {
                    Id = 1,
                    Name = "Grebane s lajica",
                    MaxPoints = 100,
                    CourseId = 1
                });

                context.Courses.Add(new Course()
                {
                    CourseId = 1,
                    Name = "asdv"
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.EvaluateStudentAsync(1, 1, 45, 12);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task EvaluateStudentShouldThrowExceptionIfAssignmentntIdIsInvalid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
    .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowExceptionIfAssignmentntIdIsInvalid")
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

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev",
                    RoleId = 3
                });

                context.Courses.Add(new Course()
                {
                    CourseId = 1,
                    Name = "asdv"
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.EvaluateStudentAsync(1, 1, 45, 2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotEnrolledInCourseException))]
        public async Task EvaluateStudentShouldThrowExceptionIfTeacherNotAssignedForThisCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
    .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowExceptionIfTeacherNotAssignedForThisCourse")
    .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev - Uchitelq",
                    RoleId = 2
                });

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev - Studentcheto",
                    RoleId = 3
                });
                context.Assignments.Add(new Assignment()
                {
                    Id = 1,
                    Name = "Grebane s lajica",
                    MaxPoints = 100,
                    CourseId = 1
                });

                context.Courses.Add(new Course()
                {
                    CourseId = 1,
                    Name = "asdv",
                    TeacherId = 12
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.EvaluateStudentAsync(2, 1, 45, 1);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(NotEnrolledInCourseException))]
        public async Task EvaluateStudentShouldThrowExceptionIfStudentNotAssignedForThisCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
    .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowExceptionIfStudentNotAssignedForThisCourse")
    .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev - Uchitelq",
                    RoleId = 2
                });

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev - Studentcheto",
                    RoleId = 3
                });
                context.Assignments.Add(new Assignment()
                {
                    Id = 1,
                    Name = "Grebane s lajica",
                    MaxPoints = 100,
                    CourseId = 1
                });

                context.Courses.Add(new Course()
                {
                    CourseId = 1,
                    Name = "asdv",
                    TeacherId = 1
                });

                context.EnrolledStudents.Add(new EnrolledStudent()
                {

                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.EvaluateStudentAsync(2, 1, 45, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyEvaluatedException))]
        public async Task EvaluateStudentShouldThrowExceptionIfStudentAlreadyEvaluatedForThisCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
    .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowExceptionIfStudentAlreadyEvaluatedForThisCourse")
    .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev - Uchitelq",
                    RoleId = 2
                });

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev - Studentcheto",
                    RoleId = 3
                });
                context.Assignments.Add(new Assignment()
                {
                    Id = 1,
                    Name = "Grebane s lajica",
                    MaxPoints = 100,
                    CourseId = 1
                });

                context.Courses.Add(new Course()
                {
                    CourseId = 1,
                    Name = "asdv",
                    TeacherId = 1
                });

                context.EnrolledStudents.Add(new EnrolledStudent()
                {
                    StudentId = 2,
                    CourseId = 1
                });

                context.Grades.Add(new Grade()
                {
                    StudentId = 2,
                    ReceivedGrade = 43,
                    AssignmentId = 1
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.EvaluateStudentAsync(2, 1, 45, 1);
            }
        }

        [TestMethod]
        public async Task EvaluateStudentShouldAddGradeWhenCorrectParametersArePassed()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
    .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldAddGradeWhenCorrectParametersArePassed")
    .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "pesho12",
                    FullName = "Pesho Peshev - Uchitelq",
                    RoleId = 2
                });

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev - Studentcheto",
                    RoleId = 3
                });
                context.Assignments.Add(new Assignment()
                {
                    Id = 1,
                    Name = "Grebane s lajica",
                    MaxPoints = 100,
                    CourseId = 1
                });

                context.Courses.Add(new Course()
                {
                    CourseId = 1,
                    Name = "asdv",
                    TeacherId = 1
                });

                context.EnrolledStudents.Add(new EnrolledStudent()
                {
                    StudentId = 2,
                    CourseId = 1
                });

                context.SaveChanges();
            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var userService = new UserService(context);
                await userService.EvaluateStudentAsync(2, 1, 45, 1);
                var users = userService.RetrieveUsersAsynca(3).Result.ToList();
                Assert.IsTrue(users.Count == 1);
                Assert.IsTrue(users[0].Grades.Any(gr => gr.ReceivedGrade == 45));
            }
        }
    }
}

