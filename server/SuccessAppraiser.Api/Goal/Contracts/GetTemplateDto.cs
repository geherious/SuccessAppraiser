using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Api.Goal.Contracts
{
    public record GetTemplateDto(Guid Id, string Name, List<DayState> States);
}
