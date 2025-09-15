using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Extensions;
using ReadTrack.Shared;
using ReadTrack.Shared.Requests;
using ReadTrack.API.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ReadTrack.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "authPolicy")]
public class BookController : BaseController<BookController, IBookService>
{
    private readonly IUserService userService;
    public BookController(ILogger<BookController> logger, IUserService userService, IBookService service) : base(logger, service)
        => this.userService = userService;

    [HttpGet]
    [Route("/api/book/count/{searchText?}")]
    [SwaggerOperation("GetBookCountAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Succeeded")]
    public async Task<IActionResult> GetBookCountAsync(string searchText = "")
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(await Service.GetBookCountAsync(user.Id, searchText));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("/api/book/{offset}/{count}/{searchText?}")]
    [SwaggerOperation("GetBooksAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Book>), description: "Succeeded")]
    public async Task<IActionResult> GetBooksAsync(int offset, int count, string searchText = "")
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(await Service.GetBooksAsync(user.Id, offset, count, searchText));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("/api/book/{id}")]
    [SwaggerOperation("GetBooksAsync")]
    [SwaggerResponse(statusCode: 200, type: typeof(Book), description: "Succeeded")]
    public async Task<IActionResult> GetBookAsync(int id)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(await Service.GetBookAsync(user.Id, id));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpPost]
    [Route("/api/book")]
    [SwaggerOperation("CreateBookAsync")]
    [SwaggerResponse(statusCode: 201, type: typeof(Book), description: "Created")]
    public async Task<IActionResult> CreateBookAsync(CreateBookRequest request)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Created(string.Empty, await Service.CreateBookAsync(user.Id, request));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpPut]
    [Route("/api/book/{id}")]
    [SwaggerOperation("UpdateBookAsync")]
    [SwaggerResponse(statusCode: 204, description: "Updated")]
    public async Task<IActionResult> UpdateBookAsync(int id, Book book)
    {
        try
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }


            if (!await Service.UpdateBookAsync(user.Id, book))
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
    [Route("/api/book/{id}")]
    [SwaggerOperation("DeleteBookAsync")]
    [SwaggerResponse(statusCode: 204, description: "Updated")]
    public async Task<IActionResult> DeleteBookAsync(int id)
    {
        try
        {
            var user = await userService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (!await Service.DeleteBookAsync(user.Id, id))
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