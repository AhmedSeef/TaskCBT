using TaskCBT.Application.Dtos;
using TaskCBT.Domain.Entities;

namespace TaskCBT.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> RegisterCustomerAsync(RegisterCustomerDto dto);
        Task<Customer> LoginCustomerAsync(LoginCustomerDto dto);
        Task<bool> VerifyEmailAsync(string email);
        Task<bool> VerifyPhoneNumberAsync(string mobileNumber);
        Task<bool> AgreeToTermsAsync(AgreeToTermsDto dto);
        Task<bool> UpdateCustomerPhoneAsync(UpdateCustomerPhoneDto dto);
        Task<bool> UpdateCustomerEmailAsync(UpdateCustomerEmailDto dto);
        Task<bool> VerifyEmailCodeAsync(VerifyEmailCodeDto dto);
        Task<bool> VerifyPhoneCodeAsync(VerifyPhoneCodeDto dto);
    }
}
