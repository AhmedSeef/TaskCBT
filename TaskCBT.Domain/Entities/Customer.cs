using TaskCBT.Domain.enums;

namespace TaskCBT.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ICNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public bool HasAgreedToTerms { get; set; }
        public string EmailVerificationCode { get; set; }
        public DateTime? EmailVerificationCodeSentAt { get; set; }
        public string PhoneVerificationCode { get; set; }
        public DateTime? PhoneVerificationCodeSentAt { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
    }
}
