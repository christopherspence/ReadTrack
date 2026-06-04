using Microsoft.AspNetCore.Mvc;

namespace ReadTrack.Web.Mvc.Controllers;

public class LoginController : Controller
{
    public ViewResult Index()
    {
        return View();
    }
}