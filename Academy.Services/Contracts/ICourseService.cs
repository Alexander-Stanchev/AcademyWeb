using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Academy.Data;

namespace Academy.Services.Contracts
{
    public interface ICourseService
    {
        Task<Course> AddCourseAsync(int teacherId, DateTime start, DateTime end, string courseName);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task <Course> GetCourseByIdAsync(int id);
        Task EnrollStudentToCourseAsync(int studentId, int courseId);
        Task<IEnumerable<User>> RetrieveStudentsInCourseAsync(int courseId, int roleId, int userId);
    }
}