using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizza.Database;

namespace Pizza.Api.Controllers;

[ApiController]
[Route("api/pizza")]
public class PizzaController : ControllerBase
{
    private readonly PizzaDbContext context;

    public PizzaController(PizzaDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async IAsyncEnumerable<Database.Model.Pizza> GetAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var pizzas = context.Pizzas
            .AsNoTrackingWithIdentityResolution()
            .ToAsyncEnumerable();

        await foreach (var pizza in pizzas.WithCancellation(cancellationToken))
        {
            yield return pizza;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var pizza = await context.Pizzas
            .AsNoTrackingWithIdentityResolution()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        return pizza != null
            ? Ok(pizza)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Database.Model.Pizza pizza, CancellationToken cancellationToken)
    {
        await context.Pizzas.AddAsync(pizza, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return CreatedAtAction("Get", new { id = pizza.Id }, null);
    }
}