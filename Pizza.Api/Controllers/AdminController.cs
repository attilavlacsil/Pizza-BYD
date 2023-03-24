using Microsoft.AspNetCore.Mvc;
using Pizza.Database;

namespace Pizza.Api.Controllers;

[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly PizzaDbContext context;

    public AdminController(PizzaDbContext context)
    {
        this.context = context;
    }

    [HttpPost("createDatabase")]
    public async Task CreateDatabaseAsync(CancellationToken cancellationToken)
    {
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    }
}