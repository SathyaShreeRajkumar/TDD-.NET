using MongoDB.Driver;
using AirlineReservation.Models.Configuration;
using AirlineReservation.Services.Airline;
using AirlineReservation.Services.Database;
using AirlineReservation.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddSingleton<IAirlineService, AirlineService>();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.Configure<AirlineDataBaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var connectionString = builder.Configuration.GetSection("ConnectionStrings:ConnectionUrl").Value;
    return new MongoClient(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline. hisdes
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
