using Academy.Services.Providers.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Academy.Services.Contracts
{
    public interface IGradeService
    {
        Task EvaluateStudentAsync(string username, int assignmentId, int grade, string teacherUsername);

        Task<IList<GradeViewModel>> RetrieveGradesAsync(string username, string coursename = "");
    }
}