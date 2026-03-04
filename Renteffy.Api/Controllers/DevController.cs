using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renteffy.Infrastructure.Security;

namespace Renteffy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly IJwtKeyGenerator _keyGen;
        public DevController(IJwtKeyGenerator keyGen) => _keyGen = keyGen;

        [HttpPost]
        [Route("generate-jwt-key")]
        public IActionResult GenerateJwtKey([FromBody] string personalData)
        {
            var key = _keyGen.GenerateKey(personalData);
            return Ok(new { jwtKey = key });
        }
    }
}
