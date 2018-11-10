using System.Collections.Generic;

namespace Academy.Web.Areas.Teacher.Models
{
    public class CourseInformationModel
    {
        public CourseViewModel Course { get; set; }

        public IEnumerable<AssignmentViewModel> Assignments { get; set; }
    }
}
