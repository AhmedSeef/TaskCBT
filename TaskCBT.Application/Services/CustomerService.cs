using TaskCBT.Application.Dtos;
using TaskCBT.Application.Exceptions;
using TaskCBT.Application.Interfaces;
using TaskCBT.Domain.Entities;
using TaskCBT.Domain.enums;
using TaskCBT.Domain.Interfaces;

namespace TaskCBT.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly INotificationService _notificationService;

        public CustomerService(ICustomerRepository customerRepository, INotificationService notificationService)
        {
            _customerRepository = customerRepository;
            _notificationService = notificationService;
        }

        public async Task<CustomerResponseDto> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            var existingCustomer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (existingCustomer is not null)
                throw new CustomerAlreadyExistsException("Customer already exists with the provided IC number.");

            var customer = new Customer
            {
                CustomerName = dto.CustomerName,
                ICNumber = dto.ICNumber,
                MobileNumber = dto.MobileNumber,
                Email = dto.Email,
                IsEmailVerified = false,
                IsPhoneNumberVerified = false,
                HasAgreedToTerms = false,
                RegistrationStatus = RegistrationStatus.Incomplete,
                EmailVerificationCode = GenerateVerificationCode(),
                EmailVerificationCodeSentAt = DateTime.UtcNow,
                PhoneVerificationCode = GenerateVerificationCode(),
                PhoneVerificationCodeSentAt = DateTime.UtcNow
            };

            await _customerRepository.AddAsync(customer);

            // Send verification codes
            await _notificationService.SendEmailAsync(customer.Email, "Verification Code", $"Your email verification code is: {customer.EmailVerificationCode}");
            await _notificationService.SendSmsAsync(customer.MobileNumber, $"Your phone verification code is: {customer.PhoneVerificationCode}");

            return new CustomerResponseDto
            {
                CustomerName = customer.CustomerName,
                ICNumber = customer.ICNumber,
                MobileNumber = customer.MobileNumber,
                Email = customer.Email,
                IsEmailVerified = customer.IsEmailVerified,
                IsPhoneNumberVerified = customer.IsPhoneNumberVerified,
                HasAgreedToTerms = customer.HasAgreedToTerms,
                RegistrationStatus = customer.RegistrationStatus
            };
        }

        public async Task<Customer> LoginCustomerAsync(LoginCustomerDto dto)
        {
            var customer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (customer is null)
                throw new CustomerNotFoundException("No existing customer found with the provided IC number.");
            return customer;
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            customer.IsEmailVerified = true;
            customer.RegistrationStatus = RegistrationStatus.EmailVerified;

            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> VerifyPhoneNumberAsync(string mobileNumber)
        {
            var customer = await _customerRepository.GetByMobileNumberAsync(mobileNumber);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            customer.IsPhoneNumberVerified = true;
            customer.RegistrationStatus = RegistrationStatus.PhoneNumberVerified;

            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> AgreeToTermsAsync(AgreeToTermsDto dto)
        {
            var customer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            customer.HasAgreedToTerms = true;
            if (customer.IsEmailVerified && customer.IsPhoneNumberVerified)
                customer.RegistrationStatus = RegistrationStatus.Completed;

            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> UpdateCustomerPhoneAsync(UpdateCustomerPhoneDto dto)
        {
            var customer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            customer.MobileNumber = dto.NewPhoneNumber;
            customer.IsPhoneNumberVerified = true;
            if (customer.IsEmailVerified)
                customer.RegistrationStatus = RegistrationStatus.Completed;
            else
                customer.RegistrationStatus = RegistrationStatus.PhoneNumberVerified;

            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> UpdateCustomerEmailAsync(UpdateCustomerEmailDto dto)
        {
            var customer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            customer.Email = dto.NewEmail;
            customer.IsEmailVerified = true;
            if (customer.IsPhoneNumberVerified)
                customer.RegistrationStatus = RegistrationStatus.Completed;
            else
                customer.RegistrationStatus = RegistrationStatus.EmailVerified;

            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> VerifyEmailCodeAsync(VerifyEmailCodeDto dto)
        {
            var customer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            if (customer.EmailVerificationCode != dto.VerificationCode)
                throw new InvalidVerificationCodeException("Invalid verification code.");

            if (customer.EmailVerificationCodeSentAt < DateTime.UtcNow.AddMinutes(-30))
                throw new ExpiredVerificationCodeException("Verification code has expired.");

            customer.IsEmailVerified = true;
            customer.RegistrationStatus = RegistrationStatus.EmailVerified;

            if (customer.IsPhoneNumberVerified)
                customer.RegistrationStatus = RegistrationStatus.Completed;

            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> VerifyPhoneCodeAsync(VerifyPhoneCodeDto dto)
        {
            var customer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            if (customer.PhoneVerificationCode != dto.VerificationCode)
                throw new InvalidVerificationCodeException("Invalid verification code.");

            if (customer.PhoneVerificationCodeSentAt < DateTime.UtcNow.AddMinutes(-30))
                throw new ExpiredVerificationCodeException("Verification code has expired.");

            customer.IsPhoneNumberVerified = true;
            customer.RegistrationStatus = RegistrationStatus.PhoneNumberVerified;

            if (customer.IsEmailVerified)
                customer.RegistrationStatus = RegistrationStatus.Completed;

            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
