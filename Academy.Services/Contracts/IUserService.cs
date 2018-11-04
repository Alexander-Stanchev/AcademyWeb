using Academy.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Academy.Services.Contracts
{
    public interface IUserService
    {
        Task EvaluateStudentAsync(int studentId, int assignmentId, int grade, int teacherId);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateRoleAsync(int userId, int newRoleId);
        Task<IEnumerable<User>> RetrieveUsers();
    }
}