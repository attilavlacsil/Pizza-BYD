namespace Pizza.Database.Model;

public class Topping : PrimaryKey<int>
{
    public required ToppingType Type { get; init; }
    public required string Name { get; init; }
}
