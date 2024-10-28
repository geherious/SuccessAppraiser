using Bogus;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.UnitTests.TestObjects;
internal class StateFaker
{
    private static object locker = new object();

    private static Faker<DayState> Faker = new Faker<DayState>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Name, f => f.Lorem.Word())
        .RuleFor(x => x.Color, f => $"#{f.Random.Int(0, 0xFFFFFF):X6}");

    public static List<DayState> Generate(int number, GoalTemplate? template = null)
    {
        lock (locker)
        {
            return template == null
                ? Faker.Generate(number)
                : Faker.RuleFor(x => x.Templates, f => [template])
                  .Generate(number);
        }
    }
}
