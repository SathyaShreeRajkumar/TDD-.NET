using MongoDB.Driver;
using AirlineReservation.Models.Configuration;
using AirlineReservation.Services.Airline;
using AirlineReservation.Services.Database;
using AirlineReservation.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AirlineReservation.Models.Data;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AirlineReservation.Auth.AuthService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddSingleton<IAirlineService, AirlineService>();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.Configure<AirlineDataBaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var connectionString = builder.Configuration.GetSection("ConnectionStrings:ConnectionUrl").Value;
    return new MongoClient(connectionString);
});

var configuration = builder.Configuration;
var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline. hisdes
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
