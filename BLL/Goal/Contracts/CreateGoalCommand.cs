using System;

namespace SuccessAppraiser.BLL.Goal.Contracts
{
    public record CreateGoalCommand(string Name, string? Description, int DaysNumber, DateOnly DateStart, Guid TemplateId);
}
