using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TargetsRest.Models;
using TargetsRest.Services;

namespace TargetsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeshbordController(IDeshbordService deshbordService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeshbordModel>> GetDeshbord()
        {

            var agents = await deshbordService.GetNumOfAllAgentsAsync();
            var ActiveAgents = await deshbordService.GetNumOfAllActiveAgentsAsync();
            var Targets = await deshbordService.GetNumOfAllTargetsAsync();
            var EliminatedTargets = await deshbordService.GetNumOfAllEliminateTargetsAsync();
            var missions = await deshbordService.GetNumOfAllMissionsAsync();
            var assignedMissions = await deshbordService.GetNumOfAllAssignedMissionsAsync();
            var rlevantMissions = await deshbordService.GetNumOfAllRelevantMissionsAsync();
            DeshbordModel model = new()
            {
                Agents = agents,
                ActiveAgents = ActiveAgents,
                Targets = Targets,
                EliminatedTargets = EliminatedTargets,
                Missions = missions,
                AssignedMissions = assignedMissions,
                RelevantMissions = rlevantMissions
            };
            if (model == null)
            {
                return NotFound("Model not found");
            }
            return Ok(model);
        }
    }
}
