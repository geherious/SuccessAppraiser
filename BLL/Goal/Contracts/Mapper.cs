using AutoMapper;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.Data.Entities;

namespace BLL.Goal.Contracts
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<GoalItem, CreateGoalCommand>().ReverseMap();
            CreateMap<GoalDate, CreateGoalDateCommand>().ReverseMap();
        }
    }
}
