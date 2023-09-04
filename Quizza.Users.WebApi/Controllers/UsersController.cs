using JwtFactory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Quizza.Common.Constants;
using Quizza.Common.Web;
using Quizza.Users.Application.Commands;
using Quizza.Users.Application.Queries;

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

    [HttpGet("demo-auth")]
    [Authorize]
    public IActionResult DemoAuth()
    {
        return Ok(new { User.Identity?.Name, Id = User.GetUserId() });
    }
}
