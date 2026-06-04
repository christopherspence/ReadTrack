using Microsoft.AspNetCore.Mvc;
using ReadTrack.Shared.Api;
using ReadTrack.Shared.Models.Requests;
using ReadTrack.Web.Mvc.Models;
using Refit;

namespace ReadTrack.Web.Mvc.Controllers;

public class AccountController : Controller
{
    private IAuthApi authApi;

    public AccountController(IAuthApi authApi)
    {
        this.authApi = authApi;
    }

    [HttpGet]
    public ViewResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await authApi.LoginAsync(new AuthRequest
                {
                    Email = model.Email,
                    Password = model.Password
                });    

                // TODO: save the user token in session

                return Redirect("/Dashboard");
            }
            catch (ApiException)
            {
                ModelState.AddModelError("", "There was a problem logging you in.");
            }
        }

        return View(model);
    }

    public ViewResult Register()
    {
        return View();
    }
}