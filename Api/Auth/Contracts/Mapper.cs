using AutoMapper;
using SuccessAppraiser.BLL.Auth.Contracts;

namespace Api.Auth.Contracts
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<RegisterDto, RegisterCommand>().ReverseMap();
        }
    }
}
