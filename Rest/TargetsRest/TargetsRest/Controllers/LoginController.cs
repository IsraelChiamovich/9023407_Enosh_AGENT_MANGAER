using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using TargetsRest.Dto;
using TargetsRest.Services;

namespace TargetsRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IJwtService jwtService) : ControllerBase
    {
        private static readonly ImmutableList<string> allowedNames = [
            "enosh", "avi"
        ];

        [HttpPost]
        public ActionResult<string> Login([FromBody] LoginDto loginDto) =>
            allowedNames.Contains(loginDto.id)
                ? Ok(jwtService.CreateToken(loginDto.id))
                : BadRequest();

        [Authorize]
        [HttpGet("protected")]
        public ActionResult<string> Protected()
        {
            return Ok("Yay!!");
        }

    }
}
