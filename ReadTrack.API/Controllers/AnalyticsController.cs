using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadTrack.API.Extensions;
using ReadTrack.API.Services;
using ReadTrack.Shared.Models.Analytics;
using Swashbuckle.AspNetCore.Annotations;

namespace ReadTrack.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "authPolicy")]
public class AnalyticsController : BaseController<AnalyticsController, IAnalyticsService>
{
    private readonly IUserService userService;
    public AnalyticsController(ILogger<AnalyticsController> logger, IAnalyticsService service, IUserService userService) : base(logger, service)
        => this.userService = userService;

    [HttpGet]
    [Route("/api/analytics/books/{start}/{end}")]
    [SwaggerOperation(nameof(GetBooksReadAsync))]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<TimeSegment<int>>), description: "Success")]
    public async Task<IActionResult> GetBooksReadAsync(DateTime start, DateTime end)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }    

            return Ok(await Service.GetBooksReadAsync(user.Id, start, end));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("/api/analytics/time/{start}/{end}")]
    [SwaggerOperation(nameof(GetReadingTimeAsync))]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<TimeSegment<int>>), description: "Success")]
    public async Task<IActionResult> GetReadingTimeAsync(DateTime start, DateTime end)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }    

            return Ok(await Service.GetReadingTimeAsync(user.Id, start, end));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }
}