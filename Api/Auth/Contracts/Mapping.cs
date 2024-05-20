using AutoMapper;
using SuccessAppraiser.BLL.Auth.Contracts;

namespace Api.Auth.Contracts
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<NewRegisterDto, RegisterCommand>().ReverseMap();
        }
    }
}
