using Academy.Data;
using Academy.DataContext;
using Academy.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Academy.Services
{
    public class CourseService : ICourseService
    {
        private readonly AcademySiteContext context;
        private readonly IUserService userService;
        public CourseService(AcademySiteContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return this.context.Courses;
        }

        public Course GetCourseById(int id)
        {
            Validations.RangeNumbers(0, int.MaxValue, id, "The id of a course can only be a postive number.");
            return this.context.Courses.Find(id);
        }

        public void AddCourse(int teacherId, DateTime start, DateTime end, int courseId)
        {
            var course = this.GetCourseById(courseId);

            var teacher = userService.GetUserById(teacherId);

            if (teacher.RoleId != 2)
            {
                throw new ArgumentOutOfRangeException("You don't have access.");
            }

            if (course != null)
            {
                throw new ArgumentException("Course already exists");
            }

            course = new Course
            {
                Name = course.Name,
                TeacherId = teacher.Id,
                Start = start,
                End = end
            };



            context.Courses.Add(course);
            context.SaveChanges();
        }

        public void EnrollStudentToCourse(int studentId, int courseId)
        {

            var user = this.context.Users.Include(us => us.EnrolledStudents).FirstOrDefault(us => us.Id == studentId);
            var course = this.context.Courses.FirstOrDefault(co => co.CourseId == courseId);

            if (course == null)
            {
                throw new ArgumentOutOfRangeException("Unfortunately we are not offering such a course at the moment");
            }
            else if (user.EnrolledStudents.Any(es => es.CourseId == course.CourseId))
            {
                throw new ArgumentException($"You are already enrolled for the course {course.Name}.");
            }
            else
            {
                var enrolled = new EnrolledStudent
                {
                    StudentId = user.Id,
                    CourseId = course.CourseId
                };
                user.EnrolledStudents.Add(enrolled);
                this.context.SaveChanges();
            }
        }

        public IList<User> RetrieveStudentsInCourse(int courseId, int roleId, int userId)
        {
            Validations.RangeNumbers(0, int.MaxValue, userId, "The id of a user can only be a postive number.");
            Validations.RangeNumbers(0, int.MaxValue, courseId, "The id of a course can only be a postive number.");
            Validations.RangeNumbers(0, int.MaxValue, roleId, "The id of a role can only be a postive number.");
            

            var course = GetCourseById(courseId);

            if (course == null)
            {
                throw new ArgumentNullException("I can't find users in this course. Did you use '_' instead of all the spaces in the course name?");
            }

            var teacher = course.Teacher;


            var users = this.context.Users
                .Where(us => us.EnrolledStudents.Any(es => es.Course.CourseId == courseId))
                .Select(user => new User
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Grades = user.Grades
                        .Where(gr => gr.Assignment.Course.CourseId == courseId).Select(gr => new Grade { ReceivedGrade = gr.ReceivedGrade }).ToList()
                })
                .ToList();


            return users;
        }
    }
}
