using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StayNestVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public string GetVillas()
        {
            return "Here is the list of villas";
        }
    }
}
