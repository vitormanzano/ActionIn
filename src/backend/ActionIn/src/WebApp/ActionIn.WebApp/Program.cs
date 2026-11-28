using ActionIn.WebApp.Exceptions;
using ActionIn.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddProblemDetails();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");

builder.Services.AddDbInterceptor();

builder.Services.AddAuthenticationModule(connectionString, builder.Configuration);

string? mediatRLicenseKey = builder.Configuration.GetValue<string>("MediatR:LicenseKey");

builder.Services.Mediator(mediatRLicenseKey!, AuthenticationModule.ApplicationAssembly);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
