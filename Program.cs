using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Data.AsyncDataServices;
using ProfileService.Data.Interfaces;
using ProfileService.Data.Repositories;
using ProfileService.Data.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("---> Using SQL Server Database");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MakersConn")));
}
else
{
    Console.WriteLine("---> Using In Memory Database");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}
builder.Services.AddScoped<IMakerRepo, MakerRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<IProductDataClient, HttpProductDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

Console.WriteLine($"---> Product Service endpoint: {builder.Configuration["ProductService"]}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

app.Run();
