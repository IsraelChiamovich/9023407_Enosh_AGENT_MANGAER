﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpPut("{id}/move")]
        /*[Authorize]*/
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

        /*[HttpPost("create-token")]
        public ActionResult<string> CreateToken([FromBody] UserModel user)
        {
            return Ok(jwtService.GenerateToken(user));
        }


        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Auth([FromBody] LoginDto loginDto)
        {
            try
            {
                UserModel authenticad = await userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
                return Ok(jwtService.GenerateToken(authenticad));

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }*/
    }
}
