using AutoMapper;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.Data.Entities;

namespace BLL.Goal.Contracts
{
    public class GoalServiceMapper : Profile
    {
        public GoalServiceMapper()
        {
            CreateMap<GoalItem, CreateGoalCommand>().ReverseMap();
            CreateMap<GoalDate, CreateGoalDateCommand>().ReverseMap();
        }
    }
}
