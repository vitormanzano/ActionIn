using ActionIn.WebApp.Exceptions;
using ActionIn.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCustomServices();

builder.Services.AddProblemDetails();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");

builder.Services.AddDbContext(connectionString);

builder.Services.Mediator();
builder.Services.AddCustomCommands();
builder.Services.AddSmtp(builder.Configuration);

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
