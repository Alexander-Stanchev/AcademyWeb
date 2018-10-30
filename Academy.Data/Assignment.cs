using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Academy.Data
{
    public class Assignment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(35)]
        public string Name { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        [Range(0, 100)]
        public int MaxPoints { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
