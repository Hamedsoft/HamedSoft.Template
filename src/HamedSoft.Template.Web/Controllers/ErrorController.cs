using HamedSoft.Template.Web.ViewModels.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public sealed class ErrorController : Controller
{
    [AllowAnonymous]
    [Route("Error/{statusCode:int}")]
    public IActionResult Index(int statusCode)
    {
        Response.StatusCode = statusCode;

        var model = new ErrorViewModel
        {
            StatusCode = statusCode,
            Title = GetTitle(statusCode),
            Message = GetMessage(statusCode),
            CorrelationId =
                HttpContext.Items["X-Correlation-ID"]?.ToString(),
            TraceId = HttpContext.TraceIdentifier
        };

        return View(model);
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status403Forbidden =>
                "دسترسی غیرمجاز",

            StatusCodes.Status404NotFound =>
                "صفحه پیدا نشد",

            StatusCodes.Status500InternalServerError =>
                "خطای داخلی سرور",

            _ =>
                "خطایی رخ داده است"
        };
    }

    private static string GetMessage(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status403Forbidden =>
                "شما مجوز دسترسی به این بخش را ندارید.",

            StatusCodes.Status404NotFound =>
                "صفحه یا منبع موردنظر پیدا نشد.",

            StatusCodes.Status500InternalServerError =>
                "خطایی در پردازش درخواست رخ داد.",

            _ =>
                "در پردازش درخواست مشکلی رخ داده است."
        };
    }
}