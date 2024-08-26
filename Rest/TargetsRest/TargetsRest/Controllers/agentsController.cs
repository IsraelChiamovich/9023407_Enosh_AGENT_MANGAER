using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TargetsRest.Dto;
using TargetsRest.Models;
using TargetsRest.Services;

namespace TargetsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class agentsController(IAgentsService agentsService) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetModel>> GetAgent(int id)
        {
            var agent = await agentsService.GetAgentByIdAsync(id);
            if (agent == null)
            {
                return NotFound("Agent not found");
            }
            return Ok(agent);
        }
        [Authorize]
        [HttpGet("{id}/Missions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetModel>> GetMissionsByAgent(int id)
        {
            var MissionByAgent = await agentsService.GetMissionByAgentAsync(id);
            if (MissionByAgent == null)
            {
                return NotFound("Agent not found");
            }
            return Ok(MissionByAgent);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AgentModel>>> GetAllAgents()
        {
            var agents = await agentsService.GetAllAgentsAsync();
            if (agents == null)
            {
                return NotFound("Agents not found");
            }
            return Ok(agents);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentModel?>> CreateTarget([FromBody] AgentDto agentDto)
        {
            try
            {
                var agentId = await agentsService.CreateAgentAsync(agentDto);
                IdDto idDto = new();
                idDto.id = agentId;
                return Created("Agent created successfully", idDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentModel>> UpdateAgentStarting(int id, [FromBody] PositionDto positionDto)
        {
            try
            {
                var updatedAgent = await agentsService.DeterminingAStartingPosition(id, positionDto);
                return Ok(updatedAgent);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateMoveAgent(int id, [FromBody] MoveDto moveDto)
        {
            try
            {
                var newPossition = await agentsService.MoveAgent(id, moveDto);
                return Ok(newPossition);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

    }
}
