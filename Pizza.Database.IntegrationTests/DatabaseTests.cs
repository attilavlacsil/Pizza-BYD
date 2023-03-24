using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Pizza.Database.IntegrationTests;

public abstract class DatabaseTests
{
    private sealed class TestDbContext : PizzaDbContext, IDisposable, IAsyncDisposable
    {
        private readonly DbTransaction? transaction;

        public TestDbContext(DbContextOptions<PizzaDbContext> options, DbTransaction? transaction) : base(options)
        {
            this.transaction = transaction;

            if (transaction != null)
            {
                Database.UseTransaction(transaction);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            transaction?.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync().ConfigureAwait(false);

            if (transaction != null)
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    private DbConnection connection = null!;

    private string GetConnectionString()
    {
        var builder = new SqlConnectionStringBuilder
        {
            InitialCatalog = $"Pizza_{Guid.NewGuid():N}",
            ConnectRetryCount = 0,
            Encrypt = false
        };

        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        if (dbUser != null && dbPassword != null)
        {
            builder.DataSource = "localhost";
            builder.UserID = dbUser;
            builder.Password = dbPassword;
            builder.PersistSecurityInfo = true; // required for multiple connection open
        }
        else
        {
            builder.DataSource = "(localdb)\\MSSQLLocalDB";
            builder.IntegratedSecurity = true;
        }

        return builder.ConnectionString;
    }

    protected PizzaDbContext CreateContext(bool beginTransaction = true)
    {
        var builder = new DbContextOptionsBuilder<PizzaDbContext>();
        builder.UseSqlServer(connection);

        DbTransaction? transaction = beginTransaction ? connection.BeginTransaction() : null;
        return new TestDbContext(builder.Options, transaction);
    }

    [OneTimeSetUp]
    public void DatabaseOneTimeSetUp()
    {
        var connectionString = GetConnectionString();
        connection = new SqlConnection(connectionString);

        using (var context = CreateContext(beginTransaction: false))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        
        // reopen the connection after the database has been created
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
    }

    [OneTimeTearDown]
    public void DatabaseOneTimeTearDown()
    {
        using var context = CreateContext(beginTransaction: false);
        context.Database.EnsureDeleted();

        connection.Close();
        connection.Dispose();
    }
}