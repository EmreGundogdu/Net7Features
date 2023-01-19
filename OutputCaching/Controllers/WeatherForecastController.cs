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
    }
}