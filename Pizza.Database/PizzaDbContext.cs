using Microsoft.EntityFrameworkCore;

namespace Pizza.Database;

public class PizzaDbContext : DbContext
{
    public const int NameMaxLength = 128;

    public DbSet<Model.Pizza> Pizzas => Set<Model.Pizza>();
    public DbSet<Model.Sauce> Sauces => Set<Model.Sauce>();
    public DbSet<Model.Topping> Toppings => Set<Model.Topping>();

    protected PizzaDbContext()
    {
    }

    public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Model.Pizza>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(NameMaxLength);
            e.Property(x => x.Timestamp).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(x => x.Sauce).WithMany();
            e.HasMany(x => x.Toppings).WithMany();

            e.Navigation(x => x.Sauce).AutoInclude();
            e.Navigation(x => x.Toppings).AutoInclude();
        });

        modelBuilder.Entity<Model.Sauce>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(NameMaxLength);
        });

        modelBuilder.Entity<Model.Topping>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(NameMaxLength);
        });
    }
}
