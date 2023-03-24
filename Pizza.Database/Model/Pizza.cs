namespace Pizza.Database.Model;

public class Pizza : PrimaryKey<int>
{
    public required string Name { get; set; }
    public DateTime Timestamp { get; init; }

    public Sauce Sauce { get; set; } = null!;

    public ICollection<Topping> Toppings { get; set; } = new List<Topping>();
}