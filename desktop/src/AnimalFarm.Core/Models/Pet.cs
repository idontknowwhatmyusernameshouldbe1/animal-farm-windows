namespace AnimalFarm.Core.Models;

public sealed class Pet
{
    public required string Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImagePath { get; set; }
    public required DateTimeOffset CreatedAt { get; init; }
}
