using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.Services.Providers.Abstract
{
    public interface IExporter
    {
        string GenerateReport(IList<ViewModels.GradeViewModel> grades, string username);
    }
}
