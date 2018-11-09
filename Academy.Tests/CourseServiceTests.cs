using Academy.Data;
using Academy.DataContext;
using Academy.Services;
using demo_db.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);
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

        [TestMethod]
        public async Task GetCourseByIdShouldReturnCorrectCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetCourseByIdShouldReturnCorrectCourse")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Courses.Add(new Course()
                {
                    CourseId = 1,
                    Name = "DummyCourse",
                    Start = DateTime.Now,
                    Teacher = new User { Id = 1, UserName = "teacher@abc.bg" }

                });

                await context.SaveChangesAsync();

            }

            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);
                var result = await courseService.GetCourseByIdAsync(1);

                Assert.IsTrue(result != null && result.Name == "DummyCourse");
            }


        }

        [TestMethod]
        public async Task GetCourseByIdShouldThrowIfInvalidIdPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "GetCourseByIdShouldThrowIfInvalidIdPassed")
                .Options;


            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await courseService.GetCourseByIdAsync(-1));
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

            using (var context = new AcademySiteContext(contextOptions))
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
        public async Task AddCourseShouldThrowIfCourseExists()
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

               await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(async () => await courseService.AddCourseAsync(1, startDate, endDate, courseName));

            }
        }

        [TestMethod]
        public async Task AddCourseShouldThrowIfUserNotTeacher()
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

                await Assert.ThrowsExceptionAsync<IncorrectPermissionsException>(async () => await courseService.AddCourseAsync(1, startDate, endDate, courseName));

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

            using (var context = new AcademySiteContext(contextOptions))
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

        [TestMethod]
        public async Task EnrollStudentShouldThrowIfCourseNotFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "EnrollStudentShouldThrowIfCourseNotFound")
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

            using (var context = new AcademySiteContext(contextOptions))
            {
                context.Courses.Add(course);
                context.Users.Add(student);

                context.SaveChanges();

                var courseService = new CourseService(context);

                await Assert.ThrowsExceptionAsync<CourseDoesntExistsException>(async () => await courseService.EnrollStudentToCourseAsync(1, 2));

            }
        }

        [TestMethod]
        public async Task EnrollStudentShouldThrowIfUserAlreadyEnrolled()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "EnrollStudentShouldThrowIfUserAlreadyEnrolled")
                .Options;

            var course = new Course()
            {
                CourseId = 1,
                Name = "SQL",
                TeacherId = 2
            };

            var student = new User()
            {
                Id = 1,
                UserName = "Pesho",
                EnrolledStudents = new List<EnrolledStudent> { new EnrolledStudent() { Course = course } }
            };

            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Users.Add(student);

                context.SaveChanges();

                var courseService = new CourseService(context);

                await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(async () => await courseService.EnrollStudentToCourseAsync(1, 1));

            }

        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfInvalidCourseId()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsInCourseShouldThrowIfInvalidCourseId")
                .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                var expected = Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>
                    (async () => await courseService.RetrieveStudentsInCourseAsync(-1, 1, 1)).GetAwaiter().GetResult();

                Assert.AreEqual("The id of a course can only be a postive number.", expected.Message);
            }

        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfInvalidUserId()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsInCourseShouldThrowIfInvalidUserId")
                .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                var expected = Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>
                    (async () => await courseService.RetrieveStudentsInCourseAsync(1, 1, -1)).GetAwaiter().GetResult();

                Assert.AreEqual("The id of a user can only be a postive number.", expected.Message);
            }

        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfInvalidRoleId()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsInCourseShouldThrowIfInvalidRoleId")
                .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                var expected = Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>
                    (async () => await courseService.RetrieveStudentsInCourseAsync(1, -1, 1)).GetAwaiter().GetResult();

                Assert.AreEqual("The id of a role can only be a postive number.", expected.Message);
            }

        }

        [TestMethod]
        public async Task RetrieveStudentsInCourseShouldThrowIfCourseNotFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsInCourseShouldThrowIfCourseNotFound")
                .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);
                await Assert.ThrowsExceptionAsync<CourseDoesntExistsException>(async () => await courseService.RetrieveStudentsInCourseAsync(1, 1, 2));
            }
        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfUserNotCorrectTeacher()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsInCourseShouldThrowIfUserNotCorrectTeacher")
                .Options;

            var notTeacher = new User()
            {
                Id = 1,
                RoleId = 2,
                UserName = "PeshoLosta"
            };

            var actualTeacher = new User()
            {
                Id = 2,
                RoleId = 2,
                UserName = "PeshoLosta2"
            };

            var course = new Course()
            {
                CourseId = 1,
                Teacher = actualTeacher,
                Name = "SQL"
            };

            using (var context = new AcademySiteContext(contextOptions))
            {
                context.Users.Add(notTeacher);
                context.Courses.Add(course);

                context.SaveChanges();

                var courseService = new CourseService(context);

                var error = Assert.ThrowsExceptionAsync<IncorrectPermissionsException>
                    (async () => await courseService.RetrieveStudentsInCourseAsync(1, 2, 1)).GetAwaiter().GetResult();
                Assert.AreEqual("Invalid Permission", error.Message);

            }
        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfUserNotTeacher()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsInCourseShouldThrowIfUserNotTeacher")
                .Options;

            var notTeacher = new User()
            {
                Id = 1,
                RoleId = 3,
                UserName = "PeshoLosta"
            };

            var actualTeacher = new User()
            {
                Id = 2,
                RoleId = 2,
                UserName = "PeshoLosta2"
            };

            var course = new Course()
            {
                CourseId = 1,
                Teacher = actualTeacher,
                Name = "SQL"
            };

            using (var context = new AcademySiteContext(contextOptions))
            {
                context.Users.Add(notTeacher);
                context.Courses.Add(course);

                context.SaveChanges();

                var courseService = new CourseService(context);

                var error = Assert.ThrowsExceptionAsync<IncorrectPermissionsException>
                    (async () => await courseService.RetrieveStudentsInCourseAsync(1, 2, 1)).GetAwaiter().GetResult();
                Assert.AreEqual("Invalid Permission", error.Message);

            }
        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfUserNotFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsInCourseShouldThrowIfUserNotFound")
                .Options;


            var course = new Course()
            {
                CourseId = 1,
                Name = "SQL"
            };

            using (var context = new AcademySiteContext(contextOptions))
            {

                context.Courses.Add(course);

                context.SaveChanges();

                var courseService = new CourseService(context);

                var error = Assert.ThrowsExceptionAsync<IncorrectPermissionsException>
                    (async () => await courseService.RetrieveStudentsInCourseAsync(1, 2, 1)).GetAwaiter().GetResult();
                Assert.AreEqual("Invalid Permission", error.Message);

            }
        }

        [TestMethod]
        public async Task RetrieveStudentsInCourseShouldRetrieveCorrectStudents()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsInCourseShouldRetrieveCorrectStudents")
                .Options;

            var studentOne = new User()
            {
                Id = 2,
                RoleId = 3,
                UserName = "PeshoLosta"
            };

            var studentTwo = new User()
            {
                Id = 3,
                RoleId = 3,
                UserName = "LeshoPosta",

            };

            var studentThree = new User()
            {
                Id = 4,
                RoleId = 3,
                UserName = "GoshoBokluka"
            };

            var actualTeacher = new User()
            {
                Id = 1,
                RoleId = 2,
                UserName = "PeshoLosta2"
            };

            var course = new Course()
            {
                CourseId = 1,
                Teacher = actualTeacher,
                Name = "SQL",
                EnrolledStudents = new List<EnrolledStudent>()
                {
                    new EnrolledStudent(){Student = studentOne, CourseId = 1},
                    new EnrolledStudent(){Student = studentTwo, CourseId = 1}
                }

            };


            using (var context = new AcademySiteContext(contextOptions))
            {
                context.Courses.Add(course);

                context.Users.Add(studentThree);

                context.SaveChanges();

                var courseService = new CourseService(context);

                var result = await courseService.RetrieveStudentsInCourseAsync(1, 2, 1);

                Assert.IsTrue(result.Count() == 2);
                Assert.IsTrue(result.Any(re => re.EnrolledStudents.Any(en => en.StudentId == 2)));
                Assert.IsTrue(result.Any(re => re.EnrolledStudents.Any(en => en.StudentId == 3)));

            }
        }

        [TestMethod]
        public async Task RetrieveCoursesByTeacherShouldReturnCorrectCourses()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveCoursesByTeacherShouldReturnCorrectCourses")
                .Options;

            var courseByTeacher = new Course()
            {
                CourseId = 1,
                Name = "SQL",
                Teacher = new User
                {
                    UserName = "Pesho",
                    RoleId = 2
                }
            };

            var courseNotByTeacher = new Course()
            {
                CourseId = 2,
                Name = "SQL2",
                Teacher = new User
                {
                    UserName = "Gosho",
                    RoleId = 2
                }
            };

            using (var context = new AcademySiteContext(contextOptions))
            {
                context.Courses.Add(courseByTeacher);

                context.Courses.Add(courseNotByTeacher);

                context.SaveChanges();

                var courseService = new CourseService(context);

                var result = await courseService.RetrieveCoursesByTeacherAsync(1);

                Assert.IsTrue(result.Count() == 1);
                Assert.IsTrue(result.Any(co => co.Teacher.UserName == "Pesho"));

            }
        }

        [TestMethod]
        public async Task RetrieveCoursesByTeacherThrowWhenIdInvalid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveCoursesByTeacherThrowWhenIdInvalid")
                .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {

                var courseService = new CourseService(context);

                await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await courseService.RetrieveCoursesByTeacherAsync(-1));

            }
        }

        [TestMethod]
        public async Task RetrieveCoursesByStudentShouldReturnCorrectCourses()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveCoursesByStudentShouldReturnCorrectCourses")
                .Options;

            var courseByStudent = new Course()
            {
                CourseId = 1,
                Name = "Grebane s lajica",
                EnrolledStudents = new List<EnrolledStudent>()
            };

            courseByStudent.EnrolledStudents.Add(new EnrolledStudent() { StudentId = 2, CourseId = 1 });


            var courseNotByStudent = new Course()
            {
                CourseId = 2,
                Name = "Pletene na potnici",
                Teacher = new User
                {
                    UserName = "Gosho",
                    RoleId = 2
                }
            };

            using (var context = new AcademySiteContext(contextOptions))
            {
                context.Courses.Add(courseByStudent);

                context.Courses.Add(courseNotByStudent);

                context.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "gosho34",
                    FullName = "Gosho Goshev - Studentcheto",
                    RoleId = 3
                });

                context.SaveChanges();

                var courseService = new CourseService(context);

                var result = await courseService.RetrieveCoursesByStudentAsync(2);

                Assert.IsTrue(result.Count() == 1);
                Assert.IsTrue(result.Any(co => co.Name == "Grebane s lajica"));

            }
        }


        [TestMethod]
        public async Task RetrieveCoursesByStudentThrowWhenIdInvalid()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveCoursesByStudentThrowWhenIdInvalid")
                .Options;

            using (var context = new AcademySiteContext(contextOptions))
            {

                var courseService = new CourseService(context);

                await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await courseService.RetrieveCoursesByStudentAsync(-1));

            }
        }

        [TestMethod]
        public void AddAssignmentShouldThrowIfInvalidCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveCoursesByStudentThrowWhenIdInvalid")
                .Options;

            var name = "TestHomework";
            var courseId = 1;
            var teacherId = 1;
            var dateDue = DateTime.Now;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var courseService = new CourseService(context);

                Assert.ThrowsExceptionAsync<ArgumentException>(async () => await courseService.AddAssignment(courseId, teacherId, 100, name, dateDue));
            }
        }

        [TestMethod]
        public async Task AddAssignmentShouldThrowIfInvalidTeacher()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "AddAssignmentShouldThrowIfInvalidTeacher")
                .Options;

            string name = "Pesho";
            var courseId = 1;
            var teacherId = 1;
            var dateDue = DateTime.Now;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var course = new Course
                {
                    CourseId = 1,
                    Name = "MockCourse",
                    TeacherId = 1
                };

                context.Courses.Add(course);
                context.SaveChanges();

                var courseService = new CourseService(context);

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await courseService.AddAssignment(courseId, teacherId, 100, name, dateDue));
            }
        }

        [TestMethod]
        public async Task AddAssignmentShouldThrowIfInvalidName()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "AddAssignmentShouldThrowIfInvalidName")
                .Options;

            string name = null;
            var courseId = 1;
            var teacherId = 1;
            var dateDue = DateTime.Now;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var course = new Course
                {
                    CourseId = 1,
                    Name = "MockCourse",
                    TeacherId = 1
                };
                var teacher = new User
                {
                    Id = 1,
                    UserName = "Gosho"
                };
                context.Courses.Add(course);
                context.Users.Add(teacher);
                context.SaveChanges();

                var courseService = new CourseService(context);

               await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await courseService.AddAssignment(courseId, teacherId, 100, name, dateDue));
            }
        }

        [TestMethod]
        public async Task AddAssignmentShouldAddAssignmentIfCorrectParameters()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademySiteContext>()
                .UseInMemoryDatabase(databaseName: "AddAssignmentShouldAddAssignmentIfCorrectParameters")
                .Options;

            string name = "Test Assignment";
            var courseId = 1;
            var teacherId = 1;
            var dateDue = DateTime.Now;

            using (var context = new AcademySiteContext(contextOptions))
            {
                var course = new Course
                {
                    CourseId = 1,
                    Name = "MockCourse",
                    TeacherId = 1
                };
                var teacher = new User
                {
                    Id = 1,
                    UserName = "Gosho"
                };
                context.Courses.Add(course);
                context.Users.Add(teacher);
                context.SaveChanges();

                var courseService = new CourseService(context);

                var assignemnt = await courseService.AddAssignment(courseId, teacherId, 100, name, dateDue);

                Assert.IsTrue(assignemnt.Name == name);
                Assert.IsTrue(context.Assignments.Any(a => a.Name == name));
                Assert.IsTrue(context.Assignments.Count() == 1);
            }
        }
    }
}
