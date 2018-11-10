using Academy.Data;
using Academy.DataContext;
using Academy.Services.Contracts;
using Academy.Services.Providers.ViewModels;
using demo_db.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Services
{
    //TODO: This is not tested because we have the same method in userService and it is tested there. We should consider removing this one.
    public class GradeService : IGradeService
    {
        private readonly AcademySiteContext context;

        public GradeService(AcademySiteContext context)
        {
            this.context = context;
        }

        public async Task EvaluateStudentAsync(string username, int assignmentId, int grade, string teacherUsername)
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
            await this.context.SaveChangesAsync();
        }

        public async Task<IList<GradeViewModel>> RetrieveGradesAsync(string username, string coursename = "")
        {
            var user = new User();
            if (coursename != "")
            {
                user = await this.context.Users
                    .Include(us => us.Grades)
                         .ThenInclude(gr => gr.Assignment)
                            .ThenInclude(a => a.Course)
                    .FirstOrDefaultAsync(us => us.UserName == username && us.EnrolledStudents.Any(es => es.Course.Name == coursename));
            }
            else
            {
                user = await this.context.Users
                    .Include(us => us.Grades)
                        .ThenInclude(gr => gr.Assignment)
                            .ThenInclude(a => a.Course)
                .FirstOrDefaultAsync(us => us.UserName == username);
            }

            if (user == null)
            {
                throw new NotEnrolledInCourseException("You are not assigned to this course");
            }

            IList<GradeViewModel> gradesMapped = new List<GradeViewModel>();

            foreach (var grade in user.Grades)
            {
                if (coursename == "")
                {
                    gradesMapped.Add(new GradeViewModel { Assaingment = new AssignmentViewModel { Course = new CourseViewModel { CourseName = grade.Assignment.Course.Name }, Name = grade.Assignment.Name, MaxPoints = grade.Assignment.MaxPoints }, Score = grade.ReceivedGrade });
                }
                else if (grade.Assignment.Course.Name == coursename)
                {
                    gradesMapped.Add(new GradeViewModel { Assaingment = new AssignmentViewModel { Course = new CourseViewModel { CourseName = grade.Assignment.Course.Name }, Name = grade.Assignment.Name, MaxPoints = grade.Assignment.MaxPoints }, Score = grade.ReceivedGrade });
                }
            }
            return gradesMapped;
        }
    }
}
