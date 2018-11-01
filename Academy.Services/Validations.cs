using System;
using System.Text.RegularExpressions;

namespace Academy.Services
{
    public static class Validations
    {
        public const int MIN_USERNAME = 3;
        public const int MAX_USERNAME = 35;
        public const int MIN_FULLNAME = 5;
        public const int MAX_FULLNAME = 40;
        public const int MIN_COURSENAME = 3;
        public const int MAX_COURSENAME = 50;
        public const int MIN_ASS = 3;
        public const int MAX_ASS = 50;
        public const int MIN_PASSWORD = 3;
        public const int MAX_PASSWORD = 35;

        public static void ValidateLength(int min, int max, string input, string message)
        {
            if (input.Length < min || input.Length > max)
            {
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void RangeNumbers(int min, int max, int id, string message)
        {
            if (id < min || id > max)
            {
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void VerifyUserName(string username)
        {
            Regex regex = new Regex(@"^[A-Za-z0-9]+(?:[_-][A-Za-z0-9]+)*$");
            Match match = regex.Match(username);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid username");
            }
        }
    }
}