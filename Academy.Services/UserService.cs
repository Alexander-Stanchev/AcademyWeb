using Academy.Data;
using Academy.DataContext;
using Academy.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Services
{
    public class UserService : IUserService
    {
        private readonly AcademySiteContext context;

        public UserService(AcademySiteContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            Validations.RangeNumbers(0, int.MaxValue, id, Validations.POSITIVE_ERROR);

            var user = await this.context.Users.FirstOrDefaultAsync(us => us.Id == id);

            return user ?? throw new ArgumentNullException(nameof(user));
        }
        
        public async Task UpdateRoleAsync(int userId, int newRoleId)
        {
            Validations.RangeNumbers(0, int.MaxValue, userId, Validations.POSITIVE_ERROR);

            if (userId == 1)
            {
                throw new InvalidOperationException("You are not allowed to set someone's role to Adminitrator");
            }

            var user = await GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException("User doesn't exist");
            }
            else
            {
                user.RoleId = newRoleId;
            }

            await context.SaveChangesAsync();
        }

        public async Task EvaluateStudentAsync(int studentId, int assignmentId, int grade, int teacherId)
        {

            var teacher = await this.context.Users.Include(us => us.TaughtCourses).FirstOrDefaultAsync(us => us.Id == teacherId);
            var student = await this.context.Users.Include(us => us.EnrolledStudents).Include(us => us.Grades).FirstOrDefaultAsync(us => us.Id == studentId);
            var assaignment = await this.context.Assignments.Include(c => c.Course).FirstOrDefaultAsync(a => a.Id == assignmentId);

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
            await  this.context.SaveChangesAsync();
        }

        
    }
}
