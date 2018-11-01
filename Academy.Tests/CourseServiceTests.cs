using Academy.Data;
using Academy.DataContext;
using Academy.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Tests
{
    [TestClass]
    public class CourseServiceTests
    {
        [TestMethod]
        public async Task GetAllCoursesShouldReturnCorses()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetAllCoursesShouldReturnCorses")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);
                context.Courses.Add(new Course()
                {
                    Name = "DummyCourse",
                    Start = DateTime.Now,
                    Teacher = new User { Id = 1, UserName = "teacher@abc.bg" }

                });
                context.Courses.Add(new Course()
                {
                    Name = "DummyCourseTwo",
                    Start = DateTime.Now,
                    Teacher = new User { Id = 2, UserName = "teacher@abc.bg" }

                });
                context.SaveChanges();

                var result = await courseService.GetAllCoursesAsync();

                Assert.IsTrue(result.Count() == 2);
                Assert.IsTrue(result.Any(co => co.Name == "DummyCourse") && result.Any(co => co.Name == "DummyCourseTwo"));


            }
        }

        [TestMethod]
        public async Task GetAllCoursesShouldReturnEmptyStringIfNoCourses()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetAllCoursesShouldReturnEmptyStringIfNoCourses")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                var result = await courseService.GetAllCoursesAsync();

                Assert.IsTrue(result.Count() == 0);
            }
        }

        [TestMethod]
        public async Task GetCourseByIdShouldReturnNullIfCourseNotFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetCourseByIdShouldReturnNullIfCourseNotFound")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                var result = await courseService.GetCourseByIdAsync(1);

                Assert.IsTrue(result == null);
            }
        }

        //TO DO: DA PITAME EDO ILI STIVI ZASHTO ZA BOGA GURMI TOVA AKO NE SE PUSNE OTDELNO
        [TestMethod]
        public async Task GetCourseByIdShouldReturnCorrectCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetCourseByIdShouldReturnCorrectCourse")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                context.Courses.Add(new Course()
                {
                    Name = "DummyCourse",
                    Start = DateTime.Now,
                    Teacher = new User { Id = 1, UserName = "teacher@abc.bg" }

                });

                context.SaveChanges();

                var result = await courseService.GetCourseByIdAsync(1);

                Assert.IsTrue(result != null && result.Name == "DummyCourse");
                
            }
        }

        [TestMethod]
        public void GetCourseByIdShouldThrowIfInvalidIdPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetCourseByIdShouldThrowIfInvalidIdPassed")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await courseService.GetCourseByIdAsync(-1));
            }
        }

        [TestMethod]
        public async Task AddCourseShouldCorrectlyAddCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "AddCourseShouldCorrectlyAddCourse")
                .Options;

            var courseName = "SQL";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(30);

            using(var context = new AcademySiteContext(contextOptions))
            {
                var teacher = new User()
                {
                    Id = 1,
                    UserName = "teacher@abv.bg",
                    RoleId = 2
                };

                context.Users.Add(teacher);

                context.SaveChanges();

                var courseService = new CourseService(context);

                await courseService.AddCourseAsync(1, startDate, endDate, courseName);

                Assert.AreEqual(1, context.Courses.Count());
                Assert.IsTrue(context.Courses.Any(co => co.Name == "SQL" && co.TeacherId == 1));

            }
        }

        [TestMethod]
        public void AddCourseShouldThrowIfCourseExists()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "AddCourseShouldThrowIfCourseExists")
                .Options;

            var courseName = "SQL";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(30);

            using (var context = new AcademySiteContext(contextOptions))
            {
                var teacher = new User()
                {
                    Id = 1,
                    UserName = "teacher@abv.bg",
                    RoleId = 2
                };

                var course = new Course()
                {
                    CourseId = 1,
                    Name = "SQL",
                    TeacherId = 1
                };

                context.Users.Add(teacher);
                context.Add(course);

                context.SaveChanges();

                var courseService = new CourseService(context);

                Assert.ThrowsExceptionAsync<ArgumentException>(async () => await courseService.AddCourseAsync(1, startDate, endDate, courseName));

            }
        }

        [TestMethod]
        public void AddCourseShouldThrowIfUserNotTeacher()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "AddCourseShouldThrowIfUserNotTeacher")
                .Options;

            var courseName = "SQL";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(30);

            using (var context = new AcademySiteContext(contextOptions))
            {
                var teacher = new User()
                {
                    Id = 1,
                    UserName = "teacher@abv.bg",
                    RoleId = 3
                };

                context.Users.Add(teacher);

                context.SaveChanges();

                var courseService = new CourseService(context);

                Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await courseService.AddCourseAsync(1, startDate, endDate, courseName));

            }
        }

        [TestMethod]
        public async Task EnrollStudentToCourseShouldEnrollIfAllParamsValid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "EnrollStudentToCourseShouldEnrollIfAllParamsValid")
                .Options;

            var student = new User()
            {
                Id = 1,
                UserName = "Pesho"
            };

            var course = new Course()
            {
                CourseId = 1,
                Name = "SQL",
                TeacherId = 2
            };

            using(var context = new AcademySiteContext(contextOptions))
            {
                context.Courses.Add(course);
                context.Users.Add(student);

                context.SaveChanges();

                var courseService = new CourseService(context);

                await courseService.EnrollStudentToCourseAsync(1, 1);

                var user = context.Users.Include(us => us.EnrolledStudents).FirstOrDefault(us => us.Id == 1);

                Assert.IsTrue(user.EnrolledStudents.Count == 1);
                Assert.IsTrue(user.EnrolledStudents.Any(es => es.CourseId == 1));
            }
        }
    }
}
