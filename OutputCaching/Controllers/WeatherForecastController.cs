using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace OutputCaching.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[OutputCache]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        [OutputCache] // ister action ister controller seviyesinde eklenebilir
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("[action]")]
        [OutputCache(PolicyName ="CustomPolicy")] // 10 saniye verdi�im cache s�resinden sonra data d�nmedi
        public string dsadas()
        {
            return "Halllooo";
        }
    }
}