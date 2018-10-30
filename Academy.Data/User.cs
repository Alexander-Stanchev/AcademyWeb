using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Academy.Data
{
    public class User : IdentityUser<int>
    {      

        [Required]
        [MaxLength(40)]
        public string FullName { get; set; }

        public int? MentorId { get; set; }

        public User Mentor { get; set; }

        public int RoleId { get; set; }

        public bool Deleted { get; set; }

        public DateTime RegisteredOn { get; set; }
        
        public ICollection<Course> TaughtCourses { get; set; }

        public ICollection<EnrolledStudent> EnrolledStudents { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
