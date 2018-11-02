using System;

namespace demo_db.Common.Exceptions
{
    public class NotEnrolledInCourseException : Exception
    {
        public NotEnrolledInCourseException(string message) : base(message)
        {

        }
    }
}
