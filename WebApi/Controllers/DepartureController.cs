using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DepartureController : ControllerBase
{
    private readonly ILogger<DepartureController> _logger;
    private readonly IDepartureService _departureService;

    public DepartureController(ILogger<DepartureController> logger, IDepartureService departureService)
    {
        _logger = logger;
        _departureService = departureService;
    }
    
    [HttpPut]
    public async Task<IActionResult> ChangeTime([FromBody] DepartureTimeChangeDto request)
    {
        var result = await _departureService.ChangeTimeAsync(request);
        if (!result.IsValid)
        {
            return UnprocessableEntity(result);
        }
        
        return Ok(result);
    }
}