using System;

namespace demo_db.Common.Exceptions
{
    public class IncorrectPermissionsException : Exception
    {
        public IncorrectPermissionsException(string message) : base(message)
        {

        }
    }
}
