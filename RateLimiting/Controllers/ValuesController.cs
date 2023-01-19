using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace RateLimiting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Concurrency")] //basic isimli rate limit çalışıcak
    public class ValuesController : ControllerBase
    {
        public async Task<IActionResult> Get() //async metodlar da conccurrency rate limiter geçerlidir.
        {
            return Ok();
        }
    }
}
