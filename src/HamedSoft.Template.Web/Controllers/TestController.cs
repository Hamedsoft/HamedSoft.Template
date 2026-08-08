using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Web.Security;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers
{
    public class TestController : Controller
    {
        [Permission(PermissionConstants.Test.View)]
        public IActionResult Index()
        {
            return Content("Permission Granted");
        }
    }
}
