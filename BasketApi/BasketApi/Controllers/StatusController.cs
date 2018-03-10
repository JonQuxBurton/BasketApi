using Microsoft.AspNetCore.Mvc;

namespace BasketApi.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
