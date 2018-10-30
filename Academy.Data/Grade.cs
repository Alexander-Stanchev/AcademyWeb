using System.ComponentModel.DataAnnotations;

namespace Academy.Data
{
    public class Grade
    {
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        [Range(0, 100)]
        public double ReceivedGrade { get; set; }
    }
}