using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Common.Exceptions
{
    public class CourseDoesntExistsException : Exception
    {
        public CourseDoesntExistsException(string message) : base(message)
        {

        }
    }
}
