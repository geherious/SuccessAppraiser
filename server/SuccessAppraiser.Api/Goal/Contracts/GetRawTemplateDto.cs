namespace SuccessAppraiser.Api.Goal.Contracts
{
    public record GetRawTemplateDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
