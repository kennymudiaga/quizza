using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.PipelineBehaviours;
using Quizza.Common.Results;
using Quizza.Common.Web.Configuration;
using Quizza.Users.Application.Commands;
using Quizza.Users.Domain.Models.Entities;
using Quizza.Users.Application.Validators;
using Quizza.Users.Application.Infrastructure;
using Quizza.Users.Application.PipelineBehaviours;
using Quizza.Users.Application.Config;
using Quizza.Users.Application.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add a local override json file for developer/environment-centric settings
var overrideEnv = Environment.GetEnvironmentVariable("ENVIRONMENT_OVERRIDE");
if (!string.IsNullOrWhiteSpace(overrideEnv))
{
    builder.Configuration.AddJsonFile($"appsettings.{overrideEnv}.json", optional: true);
}

// Add Options
builder.Services.AddOptions();
builder.Services.ConfigureOptions(builder.Configuration, typeof(InitializationOptions).Assembly);

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuizzaUsers")));
builder.Services.AddScoped<IPasswordHasher<UserProfile>, PasswordHasher<UserProfile>>();

// Add mediator and mediator-pipeline-behaviors
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(SignupCommandHandler).Assembly);
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
