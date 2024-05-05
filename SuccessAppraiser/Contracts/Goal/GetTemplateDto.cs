using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Contracts.Goal
{
    public record GetTemplateDto(Guid Id, string Name, List<DayState> States);
}
