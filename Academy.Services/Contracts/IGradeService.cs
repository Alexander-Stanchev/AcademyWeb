using System.Threading.Tasks;

namespace Academy.Services.Contracts
{
    public interface IGradeService
    {
        Task EvaluateStudentAsync(string username, int assignmentId, int grade, string teacherUsername);
    }
}