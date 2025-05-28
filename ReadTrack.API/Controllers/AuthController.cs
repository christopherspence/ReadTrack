using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;
using ReadTrack.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ReadTrack.API.Controllers;

public class AuthController : BaseController<AuthController, IAuthService>
{
    public AuthController(ILogger<AuthController> logger, IAuthService service) : base(logger, service) { }

    [HttpPost]
    [AllowAnonymous]
    [Route("/api/auth/login")]
    [SwaggerOperation("LoginAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(TokenResponse), description: "Success")]
    public async Task<IActionResult> LoginAsync(AuthRequest request)
    {
        try 
        {
            var result = await Service.LoginAsync(request);

            if (result == null)
            {
                return Unauthorized();
            }

            return Created(string.Empty, result);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }        
    }
}