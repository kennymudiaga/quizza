using Microsoft.EntityFrameworkCore;
using Quizza.Users.WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add a local json file for developer-centric local settings
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true);

var conn = builder.Configuration.GetConnectionString("QuizzaUsers");

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuizzaUsers")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
