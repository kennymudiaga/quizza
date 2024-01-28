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
using Quizza.Users.Application.Options;
using JwtFactory;
using Quizza.Users.Application.Mappers;
using Quizza.Users.Domain.Models;
using Feral.SmtpMailer;
using Feral.Mailer.Config;

var builder = WebApplication.CreateBuilder(args);

// Add a local override json file for developer/environment-centric settings
var overrideEnv = Environment.GetEnvironmentVariable("ENVIRONMENT_OVERRIDE");
if (!string.IsNullOrWhiteSpace(overrideEnv))
{
    builder.Configuration.AddJsonFile($"appsettings.{overrideEnv}.json", optional: true);
}

// Add Options
builder.Services.AddOptions();
builder.Services.ConfigureOptions(builder.Configuration, 
    typeof(InitializationOptions).Assembly,
    typeof(AppInfoOptions).Assembly);

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuizzaUsers")));
builder.Services.AddScoped<IPasswordHasher<UserProfile>, PasswordHasher<UserProfile>>();
builder.Services.AddAutoMapper(typeof(Program), typeof(UserMappingProfile));

// Add mediator and mediator-pipeline-behaviors
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(SignUpCommandHandler).Assembly);
    config.Lifetime = ServiceLifetime.Scoped;
});

// Add pipeline behaviours - in order of execution
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IPipelineBehavior<SignUpCommand, Result<LoginResponse>>, SignUpEmailExistsBehavior>();
builder.Services.AddScoped<IPipelineBehavior<SignUpCommand, Result<LoginResponse>>, AdminInitializationBehavior>();


// Register Validators
builder.Services.AddValidatorsFromAssembly(typeof(SignUpCommandValidator).Assembly);

builder.Services.AddResponseCaching();
builder.Services.AddOutputCache();
builder.Services.AddControllers();


// Configure Authentication with JwtProvider - from JwtFactory nuget
builder.Services.AddJwtProvider(builder.Configuration.GetSection("JWT").Get<JwtInfo>());

// Configure SMPT mailer
var smtpCredentials = builder.Configuration.GetSection("Smtp").Get<SmtpCredentials>();
var emailTemplateConfig = new EmailTemplateConfig
{
    Path = Path.Combine(builder.Environment.ContentRootPath, "EmailTemplates")
};
builder.Services.AddSmtpMailer(smtpCredentials, emailTemplateConfig);

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

app.UseAuthentication();
app.UseAuthorization();

// UseCors must be called before UseResponseCaching
//app.UseCors();

app.UseResponseCaching();
app.UseOutputCache();

app.MapControllers();

app.Run();
