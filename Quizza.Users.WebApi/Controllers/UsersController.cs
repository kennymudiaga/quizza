using JwtFactory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizza.Common.Web;
using Quizza.Users.Application.Commands;

namespace Quizza.Users.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignupAsync(SignUpCommand signUp)
    {
        var result = await mediator.Send(signUp);
        return result.ToActionResult();
    }

    [HttpGet("demo-auth")]
    public IActionResult DemoAuth()
    {
        return Ok(new { User.Identity?.Name, Id = User.GetUserId() });
    }
}
