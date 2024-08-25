using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TargetsRest.Dto;
using TargetsRest.Models;
using TargetsRest.Services;

namespace TargetsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(IMissionsService missionsService) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MissionModel>> GetMission(int id)
        {
            var mission = await missionsService.GetMissionById(id);
            if (mission == null)
            {
                return NotFound("Mission not found");
            }
            return Ok(mission);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<MissionModel>>> GetAllMissions()
        {
            var missions = await missionsService.GetAllMission();
            if (missions == null)
            {
                return NotFound("Missions not found");
            }
            return Ok(missions);
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateMissions()
        {
            try
            {
                await missionsService.UpdateAllMissionsAsync();
                return Ok("Missions updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("find")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> FindNewMissions()
        {
            try
            {
                await missionsService.FindAgentsAndTargetsToMissionAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MissionModel>> UpdateMissionStatusToAssigned(int id, [FromBody] MissionDto missionDto)
        {
            try
            {
                if (missionDto.status == MissionStatus.assigned.ToString())
                    await missionsService.UpdateMissionToAssingdAsync(id);
                return Ok("updatedMissionStatus");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
