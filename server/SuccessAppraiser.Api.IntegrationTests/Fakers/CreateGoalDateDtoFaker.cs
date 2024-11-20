using AutoBogus;
using Bogus;
using SuccessAppraiser.Api.Goal.Contracts;

namespace SuccessAppraiser.Api.IntegrationTests.Fakers;

internal class CreateGoalDateDtoFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<CreateGoalDateDto> Faker = new AutoFaker<CreateGoalDateDto>()
        .RuleFor(x => x.Comment, f => f.Lorem.Word());

    public static CreateGoalDateDto Generate(Guid stateId, DateOnly startDate, int daysNumber)
    {
        lock (Lock)
        {
            DateTime start = startDate.ToDateTime(TimeOnly.MinValue);
            DateTime end = startDate.AddDays(daysNumber).ToDateTime(TimeOnly.MinValue);
            return Faker
                .RuleFor(x => x.Date, f 
                    => DateOnly.FromDateTime(
                        f.Date.Between(
                            start,
                            new DateTime(
                                Math.Min(
                                    DateTime.Now.Date.Ticks,
                                    startDate.AddDays(daysNumber - 1).ToDateTime(TimeOnly.MinValue).Ticks))))
                    )
                .RuleFor(x => x.StateId, f => stateId)
                .Generate();
        }
    }
}