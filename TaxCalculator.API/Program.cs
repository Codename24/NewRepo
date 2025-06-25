using CorrelationId.DependencyInjection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TaxCalculator.API.Middleware;
using TaxCalculator.Application.Behaviours;
using TaxCalculator.Application.Handlers;
using TaxCalculator.Application.Validators;
using TaxCalculator.Domain;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Services;
using TaxCalculator.Infrastructure.Configuration;
using TaxCalculator.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});
builder.Services.AddHttpContextAccessor(); // Required for tracking correlation IDs
builder.Services.AddCorrelationId(options =>
{
    options.IncludeInResponse = true;
});


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
builder.Services.AddDbContext<TaxDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly("TaxCalculator.Infrastructure")));
builder.Services.Configure<TaxBandSettings>(builder.Configuration.GetSection("TaxBandSettings"));
builder.Services.AddAutoMapper(typeof(DomainMappingProfile));
RegisterBusinessLogicServices(builder.Services);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveTaxResultHandler).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<TaxCalculationValidator>();
RegisterPipelineBehaviors(builder.Services);
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

void RegisterPipelineBehaviors(IServiceCollection services)
{
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
}

void RegisterBusinessLogicServices(IServiceCollection services)
{
    services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();
    services.AddScoped<ITaxDataService, TaxDataService>();
}
