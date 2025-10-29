using AsyncDownloader.Cli;
using AsyncDownloader.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddBusinessLogic();
builder.Services.AddAppLogging();
builder.Services.AddCli();

builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();


try
{
    await app.StartAsync();
    
    using var scope = app.Services.CreateScope();
    var console = scope.ServiceProvider.GetRequiredService<ConsoleApp>();
    await console.RunAsync(app.Lifetime.ApplicationStopping);
    Environment.ExitCode = 0;
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "Fatal error");
    Console.WriteLine("Critical error. Application is shutting down.");
    Environment.ExitCode = 1;
}
finally
{
    await app.StopAsync();
    await app.DisposeAsync();
}