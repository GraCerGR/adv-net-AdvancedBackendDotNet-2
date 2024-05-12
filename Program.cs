//using WebApplication1.Context;
//using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NotificationService.Services.Interfaces;
using NotificationService.Services;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
string jwtSecret = builder.Configuration["TokenSettings:JwtSecret"];
string refreshSecret = builder.Configuration["TokenSettings:RefreshSecret"];

// Add services to the container.
//builder.Services.AddSingleton<ITokenService>(new TokenService(jwtSecret, refreshSecret));
//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddSingleton(provider => new TokenService(jwtSecret, refreshSecret));
//builder.Services.AddScoped<IManagerService, ManagerService>();

//builder.Services.AddScoped<INotificationService, NotificationService>();
//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddScoped<ITokenService>(s => new TokenService(jwtSecret, refreshSecret));

builder.Services.AddHostedService<NotificationService1>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandlerMiddleware();

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

