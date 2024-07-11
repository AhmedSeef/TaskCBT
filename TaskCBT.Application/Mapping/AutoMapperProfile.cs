using AutoMapper;
using TaskCBT.Application.Dtos;
using TaskCBT.Domain.Entities;

namespace TaskCBT.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerResponseDto>();
            CreateMap<RegisterCustomerDto, Customer>();
            CreateMap<LoginCustomerDto, Customer>();
            CreateMap<VerifyEmailCodeDto, Customer>();
            CreateMap<VerifyPhoneCodeDto, Customer>();
            CreateMap<AgreeToTermsDto, Customer>();
            CreateMap<UpdateCustomerPhoneDto, Customer>();
            CreateMap<UpdateCustomerEmailDto, Customer>();
            CreateMap<SetPinCodeDto, Customer>();
            CreateMap<SetFingerprintDto, Customer>();
        }
    }
}
