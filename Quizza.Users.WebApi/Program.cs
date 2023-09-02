using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.PipelineBehaviours;
using Quizza.Common.Results;
using Quizza.Common.Web.Configuration;
using Quizza.Users.Domain.Commands;
using Quizza.Users.Domain.Models;
using Quizza.Users.Domain.Validators;
using Quizza.Users.WebApi.Infrastructure;
using Quizza.Users.WebApi.PipelineBehaviours;
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

// Add mediator and mediator-pipeline-behaviors
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(ValidationBehavior<,>).Assembly);
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<IPipelineBehavior<SignUpCommand, Result<UserProfile>>, SignUpEmailExistsBehavior>();
builder.Services.AddTransient<IPipelineBehavior<SignUpCommand, Result<UserProfile>>, AdminInitializationBehavior>();

// Register Validators
builder.Services.AddValidatorsFromAssembly(typeof(SignupCommandValidator).Assembly);

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
