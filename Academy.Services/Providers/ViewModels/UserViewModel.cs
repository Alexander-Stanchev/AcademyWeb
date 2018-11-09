using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.Services.Providers.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public int? MentorId { get; set; }
        public UserViewModel Mentor { get; set; }
        public int RoleId { get; set; }
        public ICollection<CourseViewModel> TaughtCourses { get; set; }
        public ICollection<CourseViewModel> EnrolledCourses { get; set; }
        public ICollection<GradeViewModel> Grades { get; set; }
    }
}
