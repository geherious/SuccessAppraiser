using AutoBogus;
using Bogus;
using SuccessAppraiser.Api.Auth.Contracts;

namespace SuccessAppraiser.Api.IntegrationTests.Fakers;

internal class RegisterDtoFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<RegisterDto> Faker = new AutoFaker<RegisterDto>()
       .RuleFor(x => x.Username, f => "guid_" + f.Random.Guid().ToString())
       .RuleFor(x => x.Email, f => f.Internet.Email())
       .RuleFor(x => x.Password, f => "guid_123_" + f.Internet.Password());

    public static RegisterDto Generate()
    {
        lock (Lock)
        {
            return Faker.Generate();
        }
    }
}