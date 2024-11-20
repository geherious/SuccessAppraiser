using AutoBogus;
using Bogus;
using SuccessAppraiser.Api.Goal.Contracts;

namespace SuccessAppraiser.Api.IntegrationTests.Fakers;

internal class CreateGoalDtoFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<CreateGoalDto> Faker = new AutoFaker<CreateGoalDto>()
        .RuleFor(x => x.Name, f => f.Lorem.Word())
        .RuleFor(x => x.Description, f => f.Lorem.Word())
        .RuleFor(x => x.DateStart, f => DateOnly.FromDateTime(f.Date.Recent(50)))
        .RuleFor(x => x.DaysNumber, f => f.Random.Int(1, 365));

    public static CreateGoalDto Generate(Guid templateId)
    {
        lock (Lock)
        {
            return Faker
                .RuleFor(x => x.TemplateId, f => templateId)
                .Generate();
        }
    }
}
