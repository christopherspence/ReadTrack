using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadTrack.Shared;
using ReadTrack.API.Services;
using Swashbuckle.AspNetCore.Annotations;
using ReadTrack.API.Extensions;

namespace ReadTrack.API.Controllers;

public class UserController : BaseController<UserController, IUserService>
{
    public UserController(ILogger<UserController> logger, IUserService service) : base(logger, service) { }

    [HttpGet]
    [Route("api/user")]
    [SwaggerOperation("GetCurrentUserAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(User), description: "Succeeded")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "authPolicy")]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        try
        {
            var user = await Service.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(await Service.GetUserAsync(user.Id));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

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

    [HttpPut]
    [Route("/api/user/{id}")]
    [SwaggerOperation("UpdateUserAsync")]
    [SwaggerResponse(statusCode: 204, description: "Updated")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "authPolicy")]
    public async Task<IActionResult> UpdateUserAsync(int id, User user)
    {
        try
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var userByEmail = await Service.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (userByEmail == null)
            {
                return NotFound();
            }

            var userId = userByEmail.Id;

            if (!await Service.UpdateUserAsync(userId, user))
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }
}