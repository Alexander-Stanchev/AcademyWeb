using System;
using System.Collections.Generic;
using Academy.Data;

namespace Academy.Services.Contracts
{
    public interface ICourseService
    {
        void AddCourse(int teacherId, DateTime start, DateTime end, int courseId);
        IEnumerable<Course> GetAllCourses();
        Course GetCourseById(int id);
        void EnrollStudentToCourse(int studentId, int courseId);
    }
}