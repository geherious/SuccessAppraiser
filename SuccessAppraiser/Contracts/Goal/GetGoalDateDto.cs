using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Contracts.Goal
{
    public record GetGoalDateDto(DateOnly Date, string? Comment, DayState State);
}
