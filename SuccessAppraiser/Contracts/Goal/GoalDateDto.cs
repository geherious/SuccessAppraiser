using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Contracts.Goal
{
    public record GoalDateDto(Guid Id, DateOnly Date, string? Comment, DayState State);
}
