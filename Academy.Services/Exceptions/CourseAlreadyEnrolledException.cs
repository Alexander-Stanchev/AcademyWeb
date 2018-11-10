using System;

namespace demo_db.Common.Exceptions
{
    public class CourseAlreadyEnrolledException : Exception
    {
        public CourseAlreadyEnrolledException(string message) : base(message)
        {

        }
    }
}
