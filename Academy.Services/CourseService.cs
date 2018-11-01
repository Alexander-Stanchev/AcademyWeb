using Academy.Data;
using Academy.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.Services
{
    class CourseService
    {
        private readonly AcademySiteContext context;
        public CourseService(AcademySiteContext context)
        {
            this.context = context;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return this.context.Courses;
        }

        public Course GetCourseById(int id)
        {
            Validations.RangeNumbers(0, int.MaxValue, id, "The id if a course can be only a postive number.");
            return this.context.Courses.Find(id);
        }
    }
}
