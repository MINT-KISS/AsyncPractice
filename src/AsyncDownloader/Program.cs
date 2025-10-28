using AsyncDownloader.Cli;
using AsyncDownloader.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddBusinessLogic();
builder.Services.AddTransient<ConsoleApp>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();


await app.StartAsync();

using var scope = app.Services.CreateScope();
var console = scope.ServiceProvider.GetRequiredService<ConsoleApp>();
await console.RunAsync(app.Lifetime.ApplicationStopping);

await app.StopAsync();