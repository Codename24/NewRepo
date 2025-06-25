using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaxCalculator.API.Middleware;
using TaxCalculator.Application.Handlers;
using TaxCalculator.Domain;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Services;
using TaxCalculator.Infrastructure.Configuration;
using TaxCalculator.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaxDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<TaxBandSettings>(builder.Configuration.GetSection("TaxBands"));
builder.Services.AddAutoMapper(typeof(DomainMappingProfile));
builder.Services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();
builder.Services.AddScoped<ITaxDataService, TaxDataService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

