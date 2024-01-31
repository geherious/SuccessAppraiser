using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Services.Goal.DTO
{
    public record GoalDateDto(Guid Id, DateOnly Date, string? Comment, DayState State);
}
