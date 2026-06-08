using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrbitGuard.Api.Data;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OrbitGuard Climate API",
        Version = "v1",
        Description = "API conteinerizada para monitoramento de areas de risco climatico usando dados simulados de satelite."
    });
});

builder.Services.AddDbContext<OrbitGuardDbContext>(options =>
{
    options.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 36))
);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrbitGuardDbContext>();

    var attempts = 0;
    var maxAttempts = 10;

    while (true)
    {
        try
        {
            dbContext.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            attempts++;

            if (attempts >= maxAttempts)
            {
                Console.WriteLine("Could not connect to database after several attempts.");
                Console.WriteLine(ex.Message);
                throw;
            }

            Console.WriteLine($"Database not ready yet. Attempt {attempts}/{maxAttempts}. Waiting...");
            Thread.Sleep(3000);
        }
    }
}

app.MapGet("/", () => new
{
    application = "OrbitGuard Climate API",
    status = "running",
    database = "MySQL",
    containers = new[]
    {
        "orbitguard-api-rm93032",
        "orbitguard-db-rm93032"
    }
});

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "OrbitGuard Climate API v1");
    options.RoutePrefix = "swagger";
});
app.MapControllers();

app.Run();