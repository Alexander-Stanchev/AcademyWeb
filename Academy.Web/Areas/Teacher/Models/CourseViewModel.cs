using Academy.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Teacher.Models
{
    public class CourseViewModel
    {
        public CourseViewModel()
        {

        }
        public CourseViewModel(Course course)
        {
            this.Id = course.CourseId;
            this.Name = course.Name;
            this.Start = course.Start;
            this.End = course.End;
            this.EnrolledStudentCount = course.EnrolledStudents.Count;
        }
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(25)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public int EnrolledStudentCount { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
    }
}
