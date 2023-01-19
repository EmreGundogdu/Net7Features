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
        //EnableRateLimiting Attribute: Controller yahut action seviyesinde istenilen politikada rate limiti devreye sokmamızı sağlayan bir attributedir
        //DisableRateLimiting Attribute: Controller seviyesinde devreye sokulmuş bir rate limit politikasının action seviyesinde pasifleştirilmesini sağlayan bir attributedir.

        public async Task<IActionResult> Get() //async metodlar da conccurrency rate limiter geçerlidir.
        {
            return Ok();
        }
    }
}
