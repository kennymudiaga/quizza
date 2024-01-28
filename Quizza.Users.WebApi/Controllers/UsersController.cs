using JwtFactory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizza.Common.Constants;
using Quizza.Common.Web;
using Quizza.Users.Application.Commands;
using Quizza.Users.Application.Queries;
using Quizza.Users.WebApi.ViewModels;

namespace Quizza.Users.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SignupAsync(SignUpCommand signUp, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(signUp, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("exists")]
    [ResponseCache(Duration = Seconds.OneMinute * 10, VaryByQueryKeys = new string[] { "email" })]
    public async Task<IActionResult> EmailExistsAsync(
        [FromQuery]EmailExistsQuery emailExistsQuery, 
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(emailExistsQuery, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginCommand loginCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(loginCommand, cancellationToken); 
        return result.ToActionResult();
    }

    [HttpPut("password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel changePasswordModel, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ChangePasswordCommand
        {
            ConfirmPassword = changePasswordModel.ConfirmPassword,
            OldPassword = changePasswordModel.OldPassword,
            Password = changePasswordModel.Password,
            UserId = Guid.Parse(User.GetUserId()),
        }, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordCommand resetPasswordCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(resetPasswordCommand, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("password")]
    public async Task<IActionResult> SetPassordAsync(SetPasswordCommand setPasswordCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(setPasswordCommand, cancellationToken);
        return result.ToActionResult();
    }
}
