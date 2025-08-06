using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{ opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")); });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
//app.Run()	Starts Kestrel web server and listens for requests
//Yes, it’s required to keep the app alive and responsive