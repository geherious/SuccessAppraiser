using AutoBogus;
using Bogus;
using SuccessAppraiser.BLL.UnitTests.TestObjects;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.UnitTests.Fakers;
internal static class GoalFaker
{
    private static object locker = new object();

    private static Faker<GoalItem> Faker = new AutoFaker<GoalItem>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Name, f => f.Name.Random.String(5, 128))
        .RuleFor(x => x.Description, f => f.Name.Random.String(5, 128))
        .RuleFor(x => x.DaysNumber, f => f.Random.Number(12, 66))
        .RuleFor(x => x.DateStart, f => DateOnly.FromDateTime(f.Date.PastOffset().UtcDateTime))
        .RuleFor(x => x.Dates, new List<GoalDate>());

    public static List<GoalItem> Generate(int number, GoalTemplate? template = null)
    {
        lock (locker)
        {
            if (template == null)
                template = TemplateFaker.Generate(1)[0];
            return Faker
                .RuleFor(x => x.Template, f => template)
                .RuleFor(x => x.TemplateId, f => template.Id)
                .RuleFor(x => x.UserId, f => template.UserId)
                .Generate(number);
        }
    }

    public static GoalDate AddRandomDate(this GoalItem goal)
    {
        DateOnly date = DateFaker
            .GetRandomDateTime(goal.DateStart, goal.DateStart.AddDays(goal.DaysNumber));
        DayState state = goal.Template!.States[0];

        GoalDate goalDate = DateFaker.Generate(1, date, state, goal)[0];

        goal.Dates.Add(goalDate);

        return goalDate;
    }
}