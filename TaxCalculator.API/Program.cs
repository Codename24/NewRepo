using Microsoft.EntityFrameworkCore;
using TaxCalculator.API.Middleware;
using TaxCalculator.Application.Handlers;
using TaxCalculator.Domain;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Services;
using TaxCalculator.Infrastructure.Configuration;
using TaxCalculator.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
builder.Services.AddDbContext<TaxDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly("TaxCalculator.Infrastructure")));
builder.Services.Configure<TaxBandSettings>(builder.Configuration.GetSection("TaxBandSettings"));
builder.Services.AddAutoMapper(typeof(DomainMappingProfile));
builder.Services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();
builder.Services.AddScoped<ITaxDataService, TaxDataService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveTaxResultHandler).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaxDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Migration Started");
        dbContext.Database.Migrate();
        logger.LogInformation("Migration Finished");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying migrations.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

