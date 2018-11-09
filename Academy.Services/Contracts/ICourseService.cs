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
        Task<IEnumerable<Course>> RetrieveCoursesByTeacherAsync(int teacherId);
        Task<IEnumerable<Course>> RetrieveCoursesByStudentAsync(int studentId);
        Task<Assignment> AddAssignment(int courseId, int teacherId, int maxPoints, string name, DateTime dueDate);
    }
}