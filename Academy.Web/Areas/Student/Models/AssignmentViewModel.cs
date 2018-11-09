using Academy.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Student.Models
{
    public class AssignmentViewModel
    {
        public AssignmentViewModel(Assignment assignment)
        {
            this.Id = assignment.Id;
            this.Name = assignment.Name;
            this.MaxPoints = assignment.MaxPoints;
            this.Date = assignment.DateTime;
        }
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(35)]
        public string Name { get; set; }
        [Range(0,100)]
        public int MaxPoints { get; set; }
        public DateTime Date { get; set; }
    }
}
