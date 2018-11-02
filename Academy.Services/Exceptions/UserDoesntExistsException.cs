using System;

namespace demo_db.Common.Exceptions
{
    public class UserDoesntExistsException : Exception
    {
        public UserDoesntExistsException(string message) : base(message)
        {

        }
    }
}
