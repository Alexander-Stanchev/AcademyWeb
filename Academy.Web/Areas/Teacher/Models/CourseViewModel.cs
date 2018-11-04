using Academy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Teacher.Models
{
    public class CourseViewModel
    {
        public CourseViewModel(Course course)
        {
            this.Name = course.Name;
            this.Start = course.Start;
            this.End = course.End;
        }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
