using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReadTrack.Web.Mvc.Models;

namespace ReadTrack.Web.Mvc.Controllers;

public class DashboardController : Controller
{    
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
