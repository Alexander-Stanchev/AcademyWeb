using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Academy.Data
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        [MaxLength(35)]
        public string Name { get; set; }

        public int TeacherId { get; set; }

        public User Teacher { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public ICollection<EnrolledStudent> EnrolledStudents { get; set; }
    }
}