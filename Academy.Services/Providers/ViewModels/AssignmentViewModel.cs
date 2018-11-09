using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.Services.Providers.ViewModels
{
    public class AssignmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseViewModel Course { get; set; }
        public int MaxPoints { get; set; }
    }
}
