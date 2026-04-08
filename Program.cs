using SorTechTask.ahmedmohamedelameen.BackgroundServices;
using SorTechTask.ahmedmohamedelameen.GeolocationService;
using SorTechTask.ahmedmohamedelameen.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<MemoryStorage>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IGeolocationService, GeolocationService>(client => {
    client.BaseAddress = new Uri(builder.Configuration["GeolocationSettings:BaseUrl"]);
});
builder.Services.AddHostedService<BlockedCountryCleanupService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
