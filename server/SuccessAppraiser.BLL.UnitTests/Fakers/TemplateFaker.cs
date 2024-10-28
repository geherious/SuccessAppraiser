using AutoBogus;
using Bogus;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.UnitTests.TestObjects;
internal class TemplateFaker
{
    private static object locker = new object();

    private static Faker<GoalTemplate> Faker = new AutoFaker<GoalTemplate>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Name, f => f.Company.CatchPhrase())
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.User, f => null);

    public static List<GoalTemplate> Generate(int number, int stateNumber = 3)
    {
        lock (locker)
        {
            return Faker
                .RuleFor(x => x.States, f => StateFaker.Generate(stateNumber))
                .Generate(number);
        }
    }
}
