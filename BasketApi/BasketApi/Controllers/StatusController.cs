using Microsoft.AspNetCore.Mvc;

namespace BasketApi.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        /// <summary>
        /// Returns 200 OK, can be used as a basic check for connectivity and if the API is available.
        /// </summary>
        /// <returns>200 OK</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
