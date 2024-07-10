namespace TaskCBT.Application.Exceptions
{
    public class InvalidVerificationCodeException : Exception
    {
        public InvalidVerificationCodeException()
        {
        }

        public InvalidVerificationCodeException(string message) : base(message)
        {
        }

        public InvalidVerificationCodeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
