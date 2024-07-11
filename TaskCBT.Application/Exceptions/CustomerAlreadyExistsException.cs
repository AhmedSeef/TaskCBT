using TaskCBT.Application.Dtos;
using TaskCBT.Domain.enums;

namespace TaskCBT.Application.Exceptions
{
    public class CustomerAlreadyExistsException : Exception
    {
        public RegistrationStatus RegistrationStatus { get; }

        public CustomerAlreadyExistsException(string message, CustomerResponseDto customerData) : base(message)
        {
            RegistrationStatus = customerData.RegistrationStatus;
        }
    }
}
