using HamedSoft.Template.Web.Security;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers
{
    public class TestController : Controller
    {
        [Permission("Users.View")]
        public IActionResult Index()
        {
            return Content("Permission Granted");
        }
    }
}
