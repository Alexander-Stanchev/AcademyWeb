using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Student.Models
{
    public class CoursesByStudentViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<CourseViewModel> Courses { get; set; }
    }
}
