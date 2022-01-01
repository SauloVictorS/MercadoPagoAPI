using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MercadoPagoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebHookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Teste([FromBody]dynamic teste)
        {

             return NoContent();
        }
    }
}
