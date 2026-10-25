using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PricingService.Api.Commands;

namespace PricingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    private readonly IMediator mediator;

    public PricingController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CalculatePriceCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            return new JsonResult(result);
        }
        catch (ValidationException)
        {
            return BadRequest();
        }
    }
}

