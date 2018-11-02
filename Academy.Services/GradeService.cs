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
    public class GradeService : IGradeService
    {
        private readonly AcademySiteContext context;

        public GradeService(AcademySiteContext context)
        {
            this.context = context;
        }

        public void EvaluateStudent(string username, int assignmentId, int grade, string teacherUsername)
        {
            Validations.ValidateLength(Validations.MIN_USERNAME, Validations.MAX_USERNAME, username, $"The username can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.VerifyUserName(username);

            var teacher = this.context.Users.Include(us => us.TaughtCourses).FirstOrDefault(us => us.UserName == teacherUsername);
            var student = this.context.Users.Include(us => us.EnrolledStudents).Include(us => us.Grades).FirstOrDefault(us => us.UserName == username);
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
    }
}
