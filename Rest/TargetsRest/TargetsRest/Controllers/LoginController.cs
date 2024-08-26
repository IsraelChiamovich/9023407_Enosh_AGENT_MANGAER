using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using TargetsRest.Dto;
using TargetsRest.Services;

namespace TargetsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private static readonly ImmutableList<string> allowedServersNames = ImmutableList.Create("SimulationServer", "APIServer");
        private readonly IJwtService _jwtService;

        public LoginController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        [AllowAnonymous] 
        public ActionResult<string> Login([FromBody] LoginDto loginDto)
        {
            if (allowedServersNames.Contains(loginDto.id))
            {
                var token = _jwtService.CreateToken(loginDto.id);
                return Ok(token);
            }
            return BadRequest("No good");
        }

        
    }
}
