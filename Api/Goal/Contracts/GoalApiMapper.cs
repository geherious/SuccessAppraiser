using AutoMapper;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.Data.Entities;

namespace Api.Goal.Contracts
{
    public class GoalApiMapper : Profile
    {
        public GoalApiMapper()
        {
            CreateMap<GetUserGoalDto, Goal>().ReverseMap();
            CreateMap<CreateGoalDto, CreateGoalCommand>().ReverseMap();
            CreateMap<CreateGoalDateDto, CreateGoalDateCommand>().ReverseMap();
            CreateMap<GetGoalDatesByMonthDto, GetGoalDatesByMonthQuerry>().ReverseMap();
            CreateMap<GoalDate, GetGoalDateDto>().ReverseMap();
            CreateMap<GoalTemplate, GetTemplateDto>().ReverseMap();
        }
    }
}
