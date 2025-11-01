using Bootler.Infrastructure.Extensions;
using Bootler.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bootler.Api.Controllers;

[Route("api/[controller]")]
public class SeedController(IServiceProvider provider) : ApiControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> Seed()
    {
        try
        {
            await provider.SeedDatabase();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
