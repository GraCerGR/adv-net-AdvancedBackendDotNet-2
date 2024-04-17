using WebApplication1.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Services.Interfaces;
using WebApplication1.Services;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
string jwtSecret = builder.Configuration["TokenSettings:JwtSecret"];
string refreshSecret = builder.Configuration["TokenSettings:RefreshSecret"];

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
//builder.Services.AddSingleton<ITokenService>(new TokenService(jwtSecret, refreshSecret));
//builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton(provider => new TokenService(jwtSecret, refreshSecret));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
//builder.Services.AddScoped<INotificationService, NotificationService>();
//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddScoped<ITokenService>(s => new TokenService(jwtSecret, refreshSecret));

builder.Services.AddHostedService<NotificationService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => {
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

//Токен
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "HITS",
        ValidateIssuer = true,
        ValidAudience = "HITS",
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("11234567890123456789012345678901234567890")),
        ValidateLifetime = true,
        LifetimeValidator = (before, expires, token, parameters) =>
        {
            var utcNow = DateTime.UtcNow;
            return before <= utcNow && utcNow <= expires;
        }
    };
});

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
