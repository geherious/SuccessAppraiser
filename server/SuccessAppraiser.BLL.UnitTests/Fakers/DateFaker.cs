using AutoBogus;
using Bogus;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.UnitTests.Fakers;
internal class DateFaker
{
    private static object locker = new object();

    private static Faker<GoalDate> Faker = new AutoFaker<GoalDate>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Comment, f => f.Lorem.Word());

    public static List<GoalDate> Generate(int number, DateOnly date, DayState state, GoalItem goal)
    {
        lock (locker)
        {
            return Faker
                .RuleFor(x => x.Date, f => date)
                .RuleFor(x => x.Goal, f => goal)
                .RuleFor(x => x.GoalId, f => goal.Id)
                .RuleFor(x => x.State, f => state)
                .RuleFor(x => x.StateId, f => state.Id)
                .Generate(number);
        }
    }
    public static DateTime GetRandomDateTime(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Start date must be earlier than end date.");

        var range = (end - start).Ticks;

        var randomTicks = (long)(Random.Shared.NextDouble() * range);

        return start.AddTicks(randomTicks);
    }

    public static DateOnly GetRandomDateTime(DateOnly start, DateOnly end)
    {
        return DateOnly.FromDateTime(GetRandomDateTime(
            start.ToDateTime(TimeOnly.MinValue),
            end.ToDateTime(TimeOnly.MinValue)));
    }
}