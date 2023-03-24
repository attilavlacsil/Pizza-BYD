using Microsoft.EntityFrameworkCore;
using Pizza.Database;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Pizza.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    if (builder.Environment.IsDevelopment())
                    {
                        options.JsonSerializerOptions.WriteIndented = true;
                    }

                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContext<PizzaDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Pizza"));
            });

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}