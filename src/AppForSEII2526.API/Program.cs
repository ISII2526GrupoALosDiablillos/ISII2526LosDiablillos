using System.Reflection;
using System.Text.Json.Serialization;
using System.Data.Common;
using TodoApi.Logging;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

using AppForSEII2526.API.Logging;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;

var builder = WebApplication.CreateBuilder(args);

// --- Logging RabbitMQ ---
builder.Logging.AddRabbitMQ(builder.Configuration.GetSection("RabbitMQ"));

// --- Add services to the container ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// --- Database connections ---
string? connection2Database = Environment.GetEnvironmentVariable("DBConnection2Use");

switch (connection2Database)
{
    case "SQLite":
        DbConnection _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(_connection));
        break;

    case "AzureSQL":
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(Environment.GetEnvironmentVariable("AzureSQL")));
        break;

    default:
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        break;
}
<<<<<<< HEAD
builder.Logging.AddRabbitMQ(builder.Configuration.GetSection("RabbitMQ"));
//”RabbitMQ” coincide con el nombre del bloque de propiedades en appsettings.json
//Add Identity services to the container
=======

// --- Identity ---
>>>>>>> origin/development
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
    new OpenApiInfo
    {
        Title = "AppForSEII2526.API",
        Version = "v1",
        Description = "API principal del Proyecto AppForSEII2526",
        License = new OpenApiLicense { Name = "MIT License", Url = new Uri("https://opensource.org/license/mit/") },
        Contact = new OpenApiContact { Name = "Software Engineering II Team", Email = "isii@on.uclm.es" },
    });

    options.CustomOperationIds(apiDescription =>
    {
        return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
});

var app = builder.Build();

// --- Log inicial ---
var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
startupLogger.LogInformation("API arrancada en {Env} a {Time}", app.Environment.EnvironmentName, DateTime.UtcNow);

<<<<<<< HEAD


//Map Identity routes
//app.MapIdentityApi<IdentityUser>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();



using (var scope = app.Services.CreateScope()) {
    try {

=======
// --- Inicialización BD ---
using (var scope = app.Services.CreateScope())
{
    try
    {
>>>>>>> origin/development
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (connection2Database == "SQLite")
            db.Database.EnsureCreated();
        else
            db.Database.Migrate();

        // SeedData.Initialize(db, scope.ServiceProvider, startupLogger);
    }
    catch (Exception ex)
    {
        startupLogger.LogError(ex, "Error creando o inicializando la base de datos.");
    }
}

// --- Middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.DisplayOperationId(); });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
