using Manager_Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Manager_Service.Services;
using Manager_Service.Context;
using User_Service.Services;
using User_Service.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<Handbook_Service.Context.ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<User_Service.Context.ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProgramsService, ProgramsService>();
builder.Services.AddScoped<IApplicationsService, ApplicationsService>();

builder.Configuration.AddJsonFile("appsettings.json");
string jwtSecret = builder.Configuration["TokenSettings:JwtSecret"];
string refreshSecret = builder.Configuration["TokenSettings:RefreshSecret"];
// Add services to the container.
//builder.Services.AddScoped<IManageFileService, ManageFileService>();
builder.Services.AddSingleton(provider => new TokenService(jwtSecret, refreshSecret));
builder.Services.AddScoped<ITokenService, TokenService>(provider => new TokenService(jwtSecret, refreshSecret));
builder.Services.AddScoped<IUserService, UserService>();
// Add services to the container.
/*builder.Services.AddScoped<IManagerService, Manager_Service.Services.ManagerService>();*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
