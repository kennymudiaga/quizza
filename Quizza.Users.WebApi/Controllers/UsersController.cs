using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quizza.Common.Web;
using Quizza.Users.Domain.Commands;

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
    public async Task<IActionResult> SignupAsync(SignUpCommand signUp)
    {
        var result = await mediator.Send(signUp);
        return result.ToActionResult();
    }
}
