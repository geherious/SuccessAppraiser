using AutoMapper;
using Microsoft.Extensions.Logging;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Goal.DTO;

namespace SuccessAppraiser.Services.Goal
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AddGoalDto, GoalItem>().ReverseMap();
            CreateMap<GetUserGoalDto, GoalItem>().ReverseMap();
            CreateMap<GoalDateDto, GoalDateDto>().ReverseMap();
        }
    }
}
