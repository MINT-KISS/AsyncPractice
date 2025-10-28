using AsyncDownloader.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddBusinessLogic();
builder.Services.AddHttpClient("ExternalApiClient",
                                c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
