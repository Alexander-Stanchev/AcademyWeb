using Academy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Models.HomeViewModels
{
    public class CourseViewModel
    {
        public CourseViewModel(Course course)
        {
            CourseName = course.Name;
            EnrolledStudent = course.EnrolledStudents.Count;
            Assignments = course.Assignments.Count();
            Start = course.Start;
            End = course.End;
        }

        public string CourseName { get; set; }

        public int EnrolledStudent { get; set; }

        public int Assignments { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
