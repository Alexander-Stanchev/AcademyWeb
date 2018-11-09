using Academy.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.Services.Providers.ViewModels
{
    public class GradeViewModel
    {
        public AssignmentViewModel Assaingment { get; set; }
        public UserViewModel Student { get; set; }
        public double Score { get; set; }
    }
}
 