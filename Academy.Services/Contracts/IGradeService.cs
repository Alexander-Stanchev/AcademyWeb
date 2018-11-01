namespace Academy.Services.Contracts
{
    public interface IGradeService
    {
        void EvaluateStudent(string username, int assignmentId, int grade, string teacherUsername);
    }
}