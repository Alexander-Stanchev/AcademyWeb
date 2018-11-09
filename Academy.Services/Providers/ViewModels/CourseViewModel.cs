using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.Services.Providers.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int TeacherId { get; set; }
        public UserViewModel Teacher { get; set; }
    }
}
