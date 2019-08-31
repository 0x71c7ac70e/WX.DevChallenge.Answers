using System;

namespace WX.DevChallenge.Answers.Services
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message) { }
    }
}
