namespace TaskCBT.Application.Exceptions
{
    public class ExpiredVerificationCodeException : Exception
    {
        public ExpiredVerificationCodeException()
        {
        }

        public ExpiredVerificationCodeException(string message) : base(message)
        {
        }

        public ExpiredVerificationCodeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
