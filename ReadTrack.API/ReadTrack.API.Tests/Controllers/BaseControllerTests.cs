using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReadTrack.API.Tests.Controllers;

public abstract class BaseControllerTests
{
    protected static ControllerContext CreateControllerContext(string email)
        => new()
        {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(
                    [
                        new(ClaimTypes.Email, email)
                    ], "mock"))
                }
        };
}
