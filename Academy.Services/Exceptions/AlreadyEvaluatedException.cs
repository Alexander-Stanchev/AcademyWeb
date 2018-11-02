using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.Services.Exceptions
{
    public class AlreadyEvaluatedException : Exception
    {
        public AlreadyEvaluatedException(string message) : base(message)
        {
        }
    }
}
