using Academy.Data;
using Academy.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Academy.Services
{
    public class UserService
    {
        private readonly AcademySiteContext context;

        public UserService(AcademySiteContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public User GetUserById(int id)
        {
            Validations.RangeNumbers(0, int.MaxValue, id, "Your id can be only a postive number.");

            return this.context.Users.Find(id);
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return this.context.Courses;
        }

        public void UpdateRole(int userId, int newRoleId)
        {
            Validations.RangeNumbers(0, int.MaxValue, userId, "Your id can be only a postive number.");

            if (userId == 1)
            {
                throw new InvalidOperationException("You are not allowed to set someone's role to Adminitrator");
            }

            var user = GetUserById(userId);

            if (user == null)
            {
                throw new ArgumentNullException("User doesn't exist");
            }
            else
            {
                user.RoleId = newRoleId;
            }

            context.SaveChanges();
        }

        public void EvaluateStudent(int studentId, int assignmentId, int grade, int teacherId)
        {

            var teacher = this.context.Users.Include(us => us.TaughtCourses).FirstOrDefault(us => us.Id == teacherId);
            var student = this.context.Users.Include(us => us.EnrolledStudents).Include(us => us.Grades).FirstOrDefault(us => us.Id == studentId);
            var assaignment = this.context.Assignments.Include(c => c.Course).FirstOrDefault(a => a.Id == assignmentId);

            if (assaignment == null)
            {
                throw new ArgumentNullException("Unfortunately there is no such an assignment");
            }

            if (teacher != null && assaignment.Course.TeacherId != teacher.Id)
            {
                throw new ArgumentException($"Teacher {teacher.UserName} is not assigned to {assaignment.Name}.");
            }

            if (student != null && student.EnrolledStudents.All(c => c.CourseId != assaignment.CourseId))
            {
                throw new ArgumentException($"Student {student.UserName} is not assigned to {assaignment.Name}.");
            }

            if (student.Grades.Any(g => g.AssignmentId == assaignment.Id))
            {
                throw new ArgumentException("Student already received grade for this assignment.");
            }

            var newGrade = new Grade
            {
                AssignmentId = assaignment.Id,
                StudentId = student.Id,
                ReceivedGrade = grade
            };

            student.Grades.Add(newGrade);
            this.context.SaveChanges();
        }

        public void EnrollStudent(int studentId, string coursename)
        {

            var user = this.context.Users.Include(us => us.EnrolledStudents).FirstOrDefault(us => us.Id == studentId);
            var course = this.context.Courses.FirstOrDefault(co => co.Name == coursename);

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
    }
}
