using Academy.Data;

namespace Academy.Services.Contracts
{
    public interface IUserService
    {
        void EvaluateStudent(int studentId, int assignmentId, int grade, int teacherId);
        User GetUserById(int id);
        void UpdateRole(int userId, int newRoleId);
    }
}