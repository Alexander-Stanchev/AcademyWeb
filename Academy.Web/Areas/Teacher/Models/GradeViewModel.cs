using Academy.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Teacher.Models
{
    public class GradeViewModel
    {
        public GradeViewModel()
        {

        }
        public GradeViewModel(Grade grade)
        {
            StudentId = grade.StudentId;
            UserName = grade.Student.UserName;
            AssignmentId = grade.AssignmentId;
            AssignmentName = grade.Assignment.Name;
            PointsReceived = grade.ReceivedGrade;
        }

        [Required]
        public int StudentId { get; set; }

        public string UserName { get; set; }
        [Required]
        public int AssignmentId { get; set; }

        public string AssignmentName { get; set; }

        [Required]
        [DisplayName("Score")]
        [Range(0,100)]
        public double PointsReceived { get; set; }

        public IEnumerable<SelectListItem> StudentsToBeGraded { get; set; }
    }
}
