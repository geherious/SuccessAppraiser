using SuccessAppraiser.Data.Entities;

namespace Api.Goal.Contracts
{
    public record GetTemplateDto(Guid Id, string Name, List<DayState> States);
}
