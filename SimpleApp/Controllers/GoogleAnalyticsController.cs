using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace SimpleApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleAnalyticsController : ControllerBase
    {
        public GoogleAnalyticsController()
        {
        }

        [Route("generate-dummy-event")]
        [HttpGet]
        public async Task<IActionResult> GenerateDummyEvent()
        {
            var tid = "UA-";
            var helper = new GoogleAnalyticsHelper(tid);
            await helper.GenerateEvent();
            return Ok();
        }
    }
}
