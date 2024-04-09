using System.Reflection;
using Acme.Customers.API.Authentication;
using Acme.Customers.API.Authentication.Contracts;
using Acme.Customers.API.Extensions;
using Acme.Customers.API.Filters;
using Acme.Customers.Domain;
using Acme.Customers.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IApiKeyValidator, ApiKeyValidator>();
builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddControllers(c =>
{
    c.Filters.Add(new ModelValidationAttribute());
    c.Filters.Add(new AcmeExceptionFilter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/healthz");
app.MapControllers();

app.InitializeDatabase();

app.Run();

public partial class Program { }