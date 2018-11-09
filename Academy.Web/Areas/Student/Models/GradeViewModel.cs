using Academy.Data;

namespace Academy.Web.Areas.Student.Models
{
    public class GradeViewModel
    {
        public GradeViewModel(Grade grade)
        {
            this.ReceivedGrade = grade.ReceivedGrade;
            this.Assignment = grade.Assignment;
            this.Student = grade.Student.FullName;
        }

        public double ReceivedGrade { get; set; }
        public Assignment Assignment { get; set; }
        public string Student { get; set; }
    }
}