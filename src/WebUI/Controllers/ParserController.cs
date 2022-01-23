using Microsoft.AspNetCore.Mvc;
using ParserWebApp.Application.Statistics.Commands.ParseUrl;
using ParserWebApp.Application.Statistics.Queries.GetStatisticsByUrl;

namespace ParserWebApp.WebUI.Controllers;

public class ParserController : ApiBaseController
{
    [HttpPost]
    public async Task<ActionResult<StatisticsItems>> Parse(ParseUrlCommand command)
    {
        return await Mediator.Send(command);
    }
}