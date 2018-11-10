using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<CourseViewModel> Courses { get; set; }
    }
}
