using Academy.Data;
using Academy.DataContext;
using Academy.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Services
{
    public class CourseService : ICourseService
    {
        private readonly AcademySiteContext context;

        public CourseService(AcademySiteContext context)
        {
            this.context = context;

        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await this.context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            Validations.RangeNumbers(0, int.MaxValue, id, "The id of a course can only be a postive number.");
            return await this.context.Courses.FirstOrDefaultAsync(co => co.CourseId == id);
        }

        public async Task<Course> AddCourseAsync(int teacherId, DateTime start, DateTime end, string courseName)
        {

            var teacher = await this.context.Users.FirstOrDefaultAsync(us => us.Id == teacherId);

            if (teacher.RoleId != 2)
            {
                throw new ArgumentOutOfRangeException("You don't have access.");
            }

            if (this.context.Courses.Any(co => co.Name == courseName))
            {
                throw new ArgumentException("Course already exists");
            }

            var course = new Course
            {
                Name = courseName,
                TeacherId = teacher.Id,
                Start = start,
                End = end
            };



            context.Courses.Add(course);
            await context.SaveChangesAsync();
            return course;
        }

        public async Task EnrollStudentToCourseAsync(int studentId, int courseId)
        {

            var user = await this.context.Users.Include(us => us.EnrolledStudents).FirstOrDefaultAsync(us => us.Id == studentId);
            var course = await this.context.Courses.FirstOrDefaultAsync(co => co.CourseId == courseId);

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

        public async Task<IEnumerable<User>> RetrieveStudentsInCourseAsync(int courseId, int roleId, int userId)
        {
            Validations.RangeNumbers(0, int.MaxValue, userId, "The id of a user can only be a postive number.");
            Validations.RangeNumbers(0, int.MaxValue, courseId, "The id of a course can only be a postive number.");
            Validations.RangeNumbers(0, int.MaxValue, roleId, "The id of a role can only be a postive number.");
            

            var course = await GetCourseByIdAsync(courseId);

            if (course == null)
            {
                throw new ArgumentNullException("Course doesn't exist");
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
                .ToListAsync();


            return await users;
        }
    }
}
