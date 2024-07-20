using AutoMapper;
using SuccessAppraiser.BLL.Auth.Contracts;

namespace SuccessAppraiser.Api.Auth.Contracts
{
    public class AuthApiMapper : Profile
    {
        public AuthApiMapper()
        {
            CreateMap<RegisterDto, RegisterCommand>().ReverseMap();
            CreateMap<LoginQuerry, LoginDto>().ReverseMap();
        }
    }
}
