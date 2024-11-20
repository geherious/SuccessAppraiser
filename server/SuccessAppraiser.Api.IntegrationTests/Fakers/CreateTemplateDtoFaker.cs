using AutoBogus;
using Bogus;
using SuccessAppraiser.Api.Auth.Contracts;
using SuccessAppraiser.Api.Goal.Contracts;

namespace SuccessAppraiser.Api.IntegrationTests.Fakers;

internal class CreateTemplateDtoFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<CreateTemplateDto> Faker = new AutoFaker<CreateTemplateDto>()
       .RuleFor(x => x.Name, f => f.Random.Guid().ToString())
       .RuleFor(x => x.States, f => CreateDayStateDtoFaker.Generate(3));

    public static CreateTemplateDto Generate()
    {
        lock (Lock)
        {
            return Faker.Generate();
        }
    }
}
