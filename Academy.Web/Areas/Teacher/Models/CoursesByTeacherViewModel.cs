using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Teacher.Models
{
    public class CoursesByTeacherViewModel
    {
        public int UserName { get; set; }
        public IEnumerable<CourseViewModel> Courses { get; set; }
    }
}
