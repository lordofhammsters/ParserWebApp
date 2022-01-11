using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;
    private string? _originalPath;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult HttpStatusCodeHandler(int id)
    {
        var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        _originalPath = feature != null ? (feature.OriginalPath + feature.OriginalQueryString) : null;

        switch (id)
        {
            case 400:
                return BadRequestPage();
            case 403:
                return Forbidden();
            case 404:
                return PageNotFound();
            case 500:
                return InternalServerError();
        }
        return Error();
    }

    // 404
    public IActionResult PageNotFound()
    {
        var logger = NLog.LogManager.GetLogger("404");
        logger.Error($"Page not found {_originalPath}");

        Response.StatusCode = 404;
        return View("PageNotFound");
    }
    
    // 400
    public ActionResult BadRequestPage()
    {
        _logger.LogError($"BadRequest {_originalPath}");
        
        Response.StatusCode = 400;
        return View("BadRequestPage");
    }
    
    // 403
    public ActionResult Forbidden()
    {
        _logger.LogError($"Forbidden {_originalPath}");
        
        Response.StatusCode = 403;
        return View("Forbidden");
    }

    // 500
    public ActionResult InternalServerError()
    {
        Response.StatusCode = 500;
        return View("InternalServerError");
    }
    
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}