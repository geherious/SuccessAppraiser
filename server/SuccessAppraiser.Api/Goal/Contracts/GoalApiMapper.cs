using AutoMapper;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Api.Goal.Contracts
{
    public class GoalApiMapper : Profile
    {
        public GoalApiMapper()
        {
            CreateMap<GetUserGoalDto, GoalItem>().ReverseMap();
            CreateMap<CreateGoalDto, CreateGoalCommand>().ReverseMap();
            CreateMap<CreateGoalDateDto, CreateGoalDateCommand>().ReverseMap();
            CreateMap<GetGoalDatesByMonthDto, GetGoalDatesByMonthQuerry>().ReverseMap();
            CreateMap<GoalDate, GetGoalDateDto>().ReverseMap();
            CreateMap<GoalTemplate, GetTemplateDto>().ReverseMap();
            CreateMap<UpdateGoalDateDto, UpdateGoalDateCoomand>().ReverseMap();
            CreateMap<GetRawTemplateDto, GoalTemplate>().ReverseMap();
        }
    }
}
