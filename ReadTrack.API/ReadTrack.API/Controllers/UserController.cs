using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Models;
using ReadTrack.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ReadTrack.API.Controllers;

public class UserController : BaseController<UserController, IUserService>
{
    public UserController(ILogger<UserController> logger, IUserService service) : base(logger, service) { }

    [HttpPost]
    [Route("/api/user/register")]
    [SwaggerOperation("RegisterAsync")]
    [SwaggerResponse(statusCode: 201, type: typeof(TokenResponse), description: "Created")]
    public async Task<IActionResult> RegisterAsync(CreateUserRequest request)
    {
        try
        {
            var result = await Service.CreateUserAsync(request);

            return Created(string.Empty, result);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }
}