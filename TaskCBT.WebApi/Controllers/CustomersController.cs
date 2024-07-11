using Microsoft.AspNetCore.Mvc;
using TaskCBT.Application.Dtos;
using TaskCBT.Application.Exceptions;
using TaskCBT.Application.Interfaces;

namespace TaskCBT.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerDto dto)
        {
            try
            {
                var customer = await _customerService.RegisterCustomerAsync(dto);
                return Ok(customer);
            }
            catch (CustomerAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message, registrationStatus = ex.RegistrationStatus });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCustomerDto dto)
        {
            var customer = await _customerService.LoginCustomerAsync(dto);
            return Ok(customer);
        }

        [HttpPost("verify-email-code")]
        public async Task<IActionResult> VerifyEmailCode([FromBody] VerifyEmailCodeDto dto)
        {
            var result = await _customerService.VerifyEmailCodeAsync(dto);
            return Ok(result);
        }

        [HttpPost("verify-phone-code")]
        public async Task<IActionResult> VerifyPhoneCode([FromBody] VerifyPhoneCodeDto dto)
        {
            var result = await _customerService.VerifyPhoneCodeAsync(dto);
            return Ok(result);
        }

        [HttpPost("agree-to-terms")]
        public async Task<IActionResult> AgreeToTerms([FromBody] AgreeToTermsDto dto)
        {
            var result = await _customerService.AgreeToTermsAsync(dto);
            return Ok(result);
        }

        [HttpPost("update-phone")]
        public async Task<IActionResult> UpdatePhone([FromBody] UpdateCustomerPhoneDto dto)
        {
            var result = await _customerService.UpdateCustomerPhoneAsync(dto);
            return Ok(result);
        }

        [HttpPost("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateCustomerEmailDto dto)
        {
            var result = await _customerService.UpdateCustomerEmailAsync(dto);
            return Ok(result);
        }

        [HttpPost("set-pin")]
        public async Task<IActionResult> SetPinCode([FromBody] SetPinCodeDto dto)
        {
            var result = await _customerService.SetPinCodeAsync(dto);
            return Ok(result);
        }

        [HttpPost("set-fingerprint")]
        public async Task<IActionResult> SetFingerprint([FromBody] SetFingerprintDto dto)
        {
            var result = await _customerService.SetFingerprintAsync(dto);
            return Ok(result);
        }
    }
}
