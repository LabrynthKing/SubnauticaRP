namespace SubnauticaRP.Maps;

public readonly struct BiomeData
{
    public required string Details { get; init; }
    public required string LargeImageKey { get; init; }
    public required bool IsDeep { get; init; }
}