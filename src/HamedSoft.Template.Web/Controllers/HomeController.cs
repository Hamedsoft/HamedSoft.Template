using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var isAuthenticated = User.Identity?.IsAuthenticated;

        var name = User.Identity?.Name;
        ViewData["name"] = name;
        ViewData["isAuthenticated"] = isAuthenticated;
        return View();
    }
}
