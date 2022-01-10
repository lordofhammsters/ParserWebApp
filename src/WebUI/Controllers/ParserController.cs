using Application.Statistics.Commands.ParseUrl;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class ParserController : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Parse(ParseUrlCommand command)
    {
        return Json(await Mediator.Send(command));
    }
}