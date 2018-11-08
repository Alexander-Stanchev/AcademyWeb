using System.Collections.Generic;

namespace Academy.Web.Areas.Teacher.Models
{
    public class CoursesByTeacherViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<CourseViewModel> Courses { get; set; }
    }
}
