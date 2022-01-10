using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorController : Controller
{
    public IActionResult HttpStatusCodeHandler(int id)
    {
        // обрабатываем основные ошибки
        switch (id)
        {
            case 400:
                return BadRequest();
            case 403:
                return Forbidden();
            case 404:
                return NotFound();
            case 500:
                return InternalServerError();
        }
        return Error();
    }

    // 404
    public IActionResult NotFound()
    {
        Response.StatusCode = 404;
        return View("NotFound");
    }
    
    // 400
    public ActionResult BadRequest()
    {
        Response.StatusCode = 400;
        return View("BadRequest");
    }
    
    // 403
    public ActionResult Forbidden()
    {
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
        if (Response.StatusCode == 500)
            return InternalServerError();
        
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}