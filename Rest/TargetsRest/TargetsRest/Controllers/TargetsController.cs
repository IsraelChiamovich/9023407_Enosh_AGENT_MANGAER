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
    public class targetsController(ITargetService targetService) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetModel>> GetTarget(int id)
        {
            var target = await targetService.GetTargetByIdAsync(id);
            if (target == null)
            {
                return NotFound("Target not found");
            }
            return Ok(target);
        }
        /*[Authorize]*/
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TargetModel>>> GetAllTargets()
        {
            var targets = await targetService.GetAllTargetsAsync();
            if (targets == null)
            {
                return NotFound("Targets not found");
            }
            return Ok(targets);
        }

        /*[Authorize]*/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TargetModel?>> CreateTarget([FromBody] TargetDto targetDto)
        {
            try
            {
                var targetId = await targetService.CreateTargetAsync(targetDto);
                IdDto idDto = new();
                idDto.id = targetId;
                return Created("Target created successfully", idDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[Authorize]*/
        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetModel>> UpdateTargetStarting(int id, [FromBody] PositionDto positionDto)
        {
            try
            {
                var updatedTarget = await targetService.DeterminingAStartingPosition(id, positionDto);
                return Ok(updatedTarget);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /*[Authorize]*/
        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateMoveTarget(int id, [FromBody] MoveDto moveDto)
        {
            try
            {
                var newPossitionawait = await targetService.MoveTarget(id, moveDto);
                return Ok(newPossitionawait);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        
    }
}
