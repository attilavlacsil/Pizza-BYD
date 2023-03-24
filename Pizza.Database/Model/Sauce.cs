namespace Pizza.Database.Model;

public class Sauce : PrimaryKey<int>
{
    public required string Name { get; init; }
    public required bool IsLactoseFree { get; init; }
}
