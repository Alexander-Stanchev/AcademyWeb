using Academy.Data;
using Academy.DataContext;
using Academy.Services.Contracts;
using demo_db.Common.Exceptions;
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
            return await this.context.Courses
                .Include(co => co.EnrolledStudents)
                .Include(co => co.Assignments)
                .ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            Validations.RangeNumbers(0, int.MaxValue, id, "The id of a course can only be a postive number.");
            return await this.context.Courses
                .Include(co => co.EnrolledStudents)
                    .ThenInclude(en => en.Student)
                .Include(co => co.Assignments)
                    .ThenInclude(a => a.Grades)
                        .ThenInclude(gr => gr.Student)
                .FirstOrDefaultAsync(co => co.CourseId == id);
        }

        public async Task<Course> AddCourseAsync(int teacherId, DateTime start, DateTime end, string courseName)
        {

            var teacher = await this.context.Users.FirstOrDefaultAsync(us => us.Id == teacherId);

            if (teacher.RoleId != 2)
            {
                throw new IncorrectPermissionsException("You don't have access.");
            }

            if (this.context.Courses.Any(co => co.Name == courseName))
            {
                throw new EntityAlreadyExistsException("Course already exists");
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
                throw new CourseDoesntExistsException("Unfortunately we are not offering such a course at the moment");
            }
            else if (user.EnrolledStudents.Any(es => es.CourseId == course.CourseId))
            {
                throw new EntityAlreadyExistsException($"You are already enrolled for the course {course.Name}.");
            }
            else
            {
                var enrolled = new EnrolledStudent
                {
                    StudentId = user.Id,
                    CourseId = course.CourseId
                };
                user.EnrolledStudents.Add(enrolled);
                await context.SaveChangesAsync();
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
                throw new CourseDoesntExistsException("Course doesn't exist");
            }


            var teacher = await this.context.Users
                .Include(us => us.TaughtCourses)
                .FirstOrDefaultAsync(us => us.Id == userId);
            
            if(teacher == null || teacher.RoleId != 2 || !teacher.TaughtCourses.Any(co => co.CourseId == courseId))
            {
                throw new IncorrectPermissionsException("Invalid Permission");
            }

            var users = this.context.EnrolledStudents
                .Where(en => en.CourseId == courseId)
                .Select(en => en.Student)
                .Include(st => st.Grades)
                .ToListAsync();


            return await users;
        }

        public async Task<IEnumerable<Course>> RetrieveCoursesByTeacherAsync(int teacherId)
        {
            Validations.RangeNumbers(0, int.MaxValue, teacherId, "The id of a course can only be a postive number.");
            return await this.context.Courses
                .Include(co => co.EnrolledStudents)
                .Where(co => co.TeacherId == teacherId).ToListAsync();
        }

        public async Task<IEnumerable<Course>> RetrieveCoursesByStudentAsync(int studentId)
        {
            Validations.RangeNumbers(0, int.MaxValue, studentId, "The id of a student can only be a postive number.");
            return await this.context.Courses.Include(course => course.EnrolledStudents).Where(c => c.EnrolledStudents.Any(es => es.StudentId == studentId)).ToListAsync();
        }

        public async Task<Assignment> AddAssignment(int courseId, int teacherId, int maxPoints, string name, DateTime dueDate)
        {
            var course = await this.context.Courses
                .Include(co => co.Assignments)
                .FirstOrDefaultAsync(co => co.CourseId == courseId);
            
            var teacher = await this.context.Users.FirstOrDefaultAsync(us => us.Id == teacherId);

            if(course == null || teacher == null || name == null)
            {
                throw new ArgumentException("Invalid parameters");
            }

            else if(course.TeacherId != teacher.Id)
            {
                throw new IncorrectPermissionsException("You are not authorized to add assignments for this course");
            }

            else if(course.Assignments.Any(a => a.Name == name))
            {
                throw new EntityAlreadyExistsException("Assignment already exists");
            }
            else
            {
                var assignment = new Assignment()
                {
                    Name = name,
                    Course = course,
                    MaxPoints = maxPoints,
                    DateTime = dueDate
                };
                await this.context.Assignments.AddAsync(assignment);
                await this.context.SaveChangesAsync();

                return assignment;
            }
        }
    }
}
