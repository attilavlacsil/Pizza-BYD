using Microsoft.EntityFrameworkCore;
using Pizza.Database.Model;

namespace Pizza.Database.IntegrationTests;

public class PizzaTests : DatabaseTests
{
    [Test]
    public async Task AddPizza_NewSauceAndToppings_CreatesPizzaWithSauceAndToppings()
    {
        // Arrange
        await using var context = CreateContext();

        var newPizza = new Model.Pizza
        {
            Name = "best",
            Sauce = new Sauce
            {
                Name = "tomato",
                IsLactoseFree = true
            },
            Toppings = new List<Topping>
            {
                new Topping
                {
                    Name = "ham",
                    Type = ToppingType.Meat
                },
                new Topping
                {
                    Name = "mushroom",
                    Type = ToppingType.Vegetable
                },
                new Topping
                {
                    Name = "corn",
                    Type = ToppingType.Vegetable
                }
            }
        };

        // Act
        await context.Pizzas.AddAsync(newPizza);
        await context.SaveChangesAsync();

        // Assert
        var pizza = await context.Pizzas.AsNoTrackingWithIdentityResolution().SingleOrDefaultAsync();
        Assert.That(pizza, Is.Not.Null, "pizza not created");
        Assert.Multiple(() =>
        {
            Assert.That(pizza.Name, Is.EqualTo("best"), "pizza name mismatch");
            Assert.That(pizza.Timestamp, Is.GreaterThan(DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(5))), "pizza bad timestamp");
            Assert.That(pizza.Sauce.Name, Is.EqualTo("tomato"), "pizza sauce mismatch");
            Assert.That(pizza.Toppings, Has.Count.EqualTo(3), "pizza toppings count mismatch");
        });
    }
}