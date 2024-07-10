using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskCBT.Application.Dtos;
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
            var customer = await _customerService.RegisterCustomerAsync(dto);
            return Ok(customer);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCustomerDto dto)
        {
            var customer = await _customerService.LoginCustomerAsync(dto);
            return Ok(customer);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
        {
            var result = await _customerService.VerifyEmailAsync(dto.Email);
            return Ok(result);
        }

        [HttpPost("verify-phone")]
        public async Task<IActionResult> VerifyPhoneNumber([FromBody] VerifyPhoneDto dto)
        {
            var result = await _customerService.VerifyPhoneNumberAsync(dto.MobileNumber);
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
    }
}
