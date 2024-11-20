using AutoBogus;
using Bogus;
using Bogus.DataSets;
using SuccessAppraiser.Api.Auth.Contracts;
using SuccessAppraiser.Api.Goal.Contracts;
using SuccessAppraiser.Api.Goal.Contracts.Validation;

namespace SuccessAppraiser.Api.IntegrationTests.Fakers;

internal class CreateDayStateDtoFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<CreateDayStateDto> Faker = new AutoFaker<CreateDayStateDto>()
       .RuleFor(x => x.Name, f => f.Random.Guid().ToString())
       .RuleFor(x => x.Color, f => $"#{f.Random.Int(0x000000, 0xFFFFFF):X6}");

    public static List<CreateDayStateDto> Generate(int n)
    {
        lock (Lock)
        {
            return Faker.Generate(n).ToList();
        }
    }
}
