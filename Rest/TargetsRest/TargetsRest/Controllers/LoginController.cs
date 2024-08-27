using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using TargetsRest.Dto;
using TargetsRest.Services;
using TargetsRest.Models;

namespace TargetsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private static readonly ImmutableList<string> allowedServersNames = [
            "SimulationServer", "APIServer"
        ];
        private readonly IJwtService _jwtService;

        public LoginController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        public ActionResult<TokenModel> Login([FromBody] LoginDto loginDto)
        {
            Console.WriteLine("aaaa");
            if (allowedServersNames.Contains(loginDto.id))
            {
                TokenModel tokenModel = new()
                {
                    token = _jwtService.CreateToken(loginDto.id)
                };
                return Ok(tokenModel);
            }
            return BadRequest("No good");
        }

        
    }
}
