using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadTrack.Shared;
using ReadTrack.Shared.Requests;
using ReadTrack.API.Services;
using Swashbuckle.AspNetCore.Annotations;
using ReadTrack.API.Extensions;

namespace ReadTrack.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "authPolicy")]
public class SessionController : BaseController<SessionController, ISessionService>
{
    private readonly IUserService userService;

    public SessionController(ILogger<SessionController> logger, IUserService userService, ISessionService service) : base(logger, service)
        => this.userService = userService;

    [HttpGet]
    [Route("/api/session/count/{sessionId}")]
    [SwaggerOperation("GetSessionCountAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Succeeded")]
    public async Task<IActionResult> GetSessionCountAsync(int sessionId)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }


            return Ok(await Service.GetSessionCountAsync(user.Id, sessionId));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("/api/session/{sessionId}/{offset}/{count}")]
    [SwaggerOperation("GetSessionsAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Session>), description: "Succeeded")]
    public async Task<IActionResult> GetSessionsAsync(int sessionId, int offset, int count)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(await Service.GetSessionsAsync(user.Id, sessionId, offset, count));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpPost]
    [Route("/api/session/{SessionId}")]
    [SwaggerOperation("CreateSessionAsync")]
    [SwaggerResponse(statusCode: 201, type: typeof(Session), description: "Created")]
    public async Task<IActionResult> CreateSessionAsync(CreateSessionRequest request)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Created(string.Empty, await Service.CreateSessionAsync(user.Id, request));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpPut]
    [Route("/api/session/{id}")]
    [SwaggerOperation("UpdateSessionAsync")]
    [SwaggerResponse(statusCode: 204, description: "Updated")]
    public async Task<IActionResult> UpdateSessionAsync(int id, Session session)
    {
        try
        {
            if (id != session.Id)
            {
                return BadRequest();
            }

            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (!await Service.UpdateSessionAsync(user.Id, session))
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

    [HttpDelete]
    [Route("/api/session/{id}")]
    [SwaggerOperation("DeleteSessionAsync")]
    [SwaggerResponse(statusCode: 204, description: "Updated")]
    public async Task<IActionResult> DeleteSessionAsync(int id)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (!await Service.DeleteSessionAsync(user.Id, id))
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