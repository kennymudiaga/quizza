using Microsoft.EntityFrameworkCore;
using Quizza.Common.PipelineBehaviours;
using Quizza.Common.Web.Configuration;
using Quizza.Users.WebApi.Config;
using Quizza.Users.WebApi.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add a local json file for developer-centric local settings
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true);

// Add Options
builder.Services.AddOptions();
builder.Services.ConfigureOptions(builder.Configuration, Assembly.GetExecutingAssembly());

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuizzaUsers")));
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(ValidationBehaviour<,>).Assembly);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
var init = app.Services.GetService<InitializationOptions>();

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
