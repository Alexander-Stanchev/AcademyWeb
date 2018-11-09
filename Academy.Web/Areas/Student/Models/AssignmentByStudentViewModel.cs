using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Student.Models
{
    public class AssignmentByStudentViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<AssignmentViewModel> Assignments { get; set; }
        public IEnumerable<GradeViewModel> Grades { get; set; }
    }
}
