using AutoMapper;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.Goal.Contracts
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
