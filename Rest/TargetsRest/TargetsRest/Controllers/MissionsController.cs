using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TargetsRest.Services;

namespace TargetsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(IMissionsService missionsService) : ControllerBase
    {
    }
}
