using TaskCBT.Domain.enums;

namespace TaskCBT.Application.Dtos
{
    public class CustomerResponseDto
    {
        public string CustomerName { get; set; }
        public string ICNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public bool HasAgreedToTerms { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
    }
}
