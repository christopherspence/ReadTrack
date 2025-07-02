using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadTrack.Shared.Models;
using ReadTrack.API.Services;
using Swashbuckle.AspNetCore.Annotations;
using ReadTrack.Shared.Models.Requests;

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
            var userId = (await Service.GetUserByEmailAsync(User.FindFirstValue(ClaimTypes.Email))).Id;

            return Ok(await Service.GetUserAsync(userId));
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

            var userId = (await Service.GetUserByEmailAsync(User.FindFirstValue(ClaimTypes.Email))).Id;

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