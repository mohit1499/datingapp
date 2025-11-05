using System.Text;
using API.Data;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{ opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")); });

builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IMemberRepository,MemberRepository>(); 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options =>
  {
    var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("cannot get token key");
    Options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
      ValidateIssuer = false,
      ValidateAudience = false
    };
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https:localhost:4200"));

//Service locator pattern, we dont have access to dependecy injection in Program.cs

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
  var context = services.GetRequiredService<AppDbContext>();
  await context.Database.MigrateAsync(); // Add pending migrations and create database if not already created
  await Seed.SeedUsers(context);   
}
catch (Exception ex)
{
  // We used Ilogger<Program>, it will add category to logs
  var logger = services.GetRequiredService<ILogger<Program>>(); //logger is a service provided by Microsoft.Extensions.Logging>
  logger.LogError(ex, "An error occurred during migration");
}

app.Run();
//app.Run()	Starts Kestrel web server and listens for requests
//Yes, it’s required to keep the app alive and responsive