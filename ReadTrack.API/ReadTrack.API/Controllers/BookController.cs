using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Models;
using ReadTrack.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ReadTrack.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "authPolicy")]
public class BookController : BaseController<BookController, IBookService>
{
    private readonly IUserService userService;
    public BookController(ILogger<BookController> logger, IUserService userService, IBookService service) : base(logger, service) 
        => this.userService = userService;

    [HttpGet]
    [Route("/books/count/{searchText?}")]
    [SwaggerOperation("GetBookCountAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Succeeded")]
    public async Task<IActionResult> GetBookCountAsync(string searchText = "")
    {
        try
        {
            var user = await userService.GetUserByEmailAsync(User.Identity.Name);

            return Ok(await Service.GetBookCountAsync(user.Id, searchText));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("/books/{offset}/{count}/{searchText?}")]
    [SwaggerOperation("GetBooksAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Book>), description: "Succeeded")]
    public async Task<IActionResult> GetBooksAsync(int offset, int count, string searchText = "")
    {
        try
        {
            var user = await userService.GetUserByEmailAsync(User.Identity.Name);

            return Ok(await Service.GetBooksAsync(user.Id, offset, count, searchText));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }
}