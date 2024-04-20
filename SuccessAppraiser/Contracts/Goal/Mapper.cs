using AutoMapper;
using Microsoft.Extensions.Logging;
using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Contracts.Goal
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AddGoalDto, GoalItem>().ReverseMap();
            CreateMap<GetUserGoalDto, GoalItem>().ReverseMap();
            CreateMap<GetGoalDateDto, GoalDate>().ReverseMap();

        }
    }
}
