using Academy.Data;
using Academy.DataContext;
using Academy.Services.Contracts;
using Academy.Services.Exceptions;
using demo_db.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
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

            var user = await this.context.Users.Include(us => us.Grades).FirstOrDefaultAsync(us => us.Id == id);

            return user ?? throw new ArgumentNullException(nameof(user));
        }

        public async Task UpdateRoleAsync(int userId, int newRoleId)
        {
            Validations.RangeNumbers(0, int.MaxValue, userId, Validations.POSITIVE_ERROR);

            if (newRoleId == 1)
            {
                throw new IncorrectPermissionsException("You are not allowed to set someone's role to Adminitrator");
            }

            var user = await GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException("User doesn't exist");
            }
            user.RoleId = newRoleId;

            var role = context.UserRoles.FirstOrDefault(r => r.UserId == userId);
            context.UserRoles.Remove(role);
            context.UserRoles.Add(new IdentityUserRole<int>()
            {
                RoleId = newRoleId,
                UserId = user.Id
            });

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> RetrieveUsersAsync(int roleId)
        {
            var users = await this.context.Users.Where(us => us.RoleId == roleId).ToListAsync();

            return users ?? throw new ArgumentNullException(nameof(users));
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

            if (teacher == null)
            {
                throw new ArgumentNullException("Unfortunately user does not exist");
            }

            if (student == null)
            {
                throw new ArgumentNullException("Unfortunately user does not exist");
            }

            if (assaignment.Course.TeacherId != teacher.Id)
            {
                throw new NotEnrolledInCourseException($"Teacher {teacher.UserName} is not assigned to {assaignment.Name}.");
            }

            if (student.EnrolledStudents.All(c => c.CourseId != assaignment.CourseId))
            {
                throw new NotEnrolledInCourseException($"Student {student.UserName} is not assigned to {assaignment.Name}.");
            }

            if (student.Grades.Any(g => g.AssignmentId == assaignment.Id))
            {
                throw new AlreadyEvaluatedException("Student already received grade for this assignment.");
            }

            var newGrade = new Grade
            {
                AssignmentId = assaignment.Id,
                StudentId = student.Id,
                ReceivedGrade = grade
            };

            student.Grades.Add(newGrade);
            await this.context.SaveChangesAsync();
        }


    }
}
