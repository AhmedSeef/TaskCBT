using AutoMapper;
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
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, INotificationService notificationService, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<CustomerResponseDto> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            var existingCustomer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (existingCustomer is not null)
            {
                var customerData = _mapper.Map<CustomerResponseDto>(existingCustomer);

                if (existingCustomer.RegistrationStatus == RegistrationStatus.Completed)
                {
                    throw new CustomerAlreadyExistsException("There is an account registered with the IC number. Please login to continue.", customerData);
                }
                else
                {
                    throw new CustomerAlreadyExistsException("There is an account registered with the IC number. Registration is not completed. Please complete the registration process.", customerData);
                }
            }

            var customer = _mapper.Map<Customer>(dto);
            customer.IsEmailVerified = false;
            customer.IsPhoneNumberVerified = false;
            customer.HasAgreedToTerms = false;
            customer.RegistrationStatus = RegistrationStatus.Incomplete;
            customer.EmailVerificationCode = GenerateVerificationCode();
            customer.EmailVerificationCodeSentAt = DateTime.UtcNow;
            customer.PhoneVerificationCode = GenerateVerificationCode();
            customer.PhoneVerificationCodeSentAt = DateTime.UtcNow;

            await _customerRepository.AddAsync(customer);

            // Send verification codes
            await _notificationService.SendEmailAsync(customer.Email, "Verification Code", $"Your email verification code is: {customer.EmailVerificationCode}");
            await _notificationService.SendSmsAsync(customer.MobileNumber, $"Your phone verification code is: {customer.PhoneVerificationCode}");

            return _mapper.Map<CustomerResponseDto>(customer);
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

        public async Task<bool> SetPinCodeAsync(SetPinCodeDto dto)
        {
            var customer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            customer.PinCode = dto.PinCode;
            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> SetFingerprintAsync(SetFingerprintDto dto)
        {
            var customer = await _customerRepository.GetByICNumberAsync(dto.ICNumber);
            if (customer is null)
                throw new CustomerNotFoundException("Customer not found.");

            customer.IsFingerprintEnabled = dto.IsFingerprintEnabled;
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
