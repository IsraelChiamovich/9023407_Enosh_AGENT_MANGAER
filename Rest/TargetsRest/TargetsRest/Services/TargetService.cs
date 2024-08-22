﻿using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TargetsRest.Data;
using TargetsRest.Dto;
using TargetsRest.Models;

namespace TargetsRest.Services
{
    public class TargetService(ApplicationDbContext context) : ITargetService
    {
        private readonly Dictionary<string, (int x, int y)> directions = new()
        {
            { "n",  (0, -1) },  // צפון
            { "s",  (0,  1) },  // דרום
            { "e",  (1,  0) },  // מזרח
            { "w",  (-1, 0) },  // מערב
            { "ne", (1, -1) },  // צפון-מזרח
            { "nw", (-1, -1) },  // צפון-מערב
            { "se", (1,  1) },  // דרום-מזרח
            { "sw", (-1, 1) },  // דרום-מערב
        };

        public async Task<int> CreateTargetAsync(TargetDto targetDto)
        {
            TargetModel target = new()
            {
                Name = targetDto.Name,
                ImageLink = targetDto.PhotoUrl,
                Role = targetDto.Position,
                Status = TargetStatus.Live
            };

            await context.Targets.AddAsync(target);
            await context.SaveChangesAsync();
            return target.Id;

        }

        public async Task<TargetModel?> GetTargetByIdAsync(int id) =>
            await context.Targets.FindAsync(id);

        public async Task<List<TargetModel>> GetAllTargetsAsync() =>
            await context.Targets.ToListAsync();

        public async Task<TargetModel> DeterminingAStartingPosition(int id, PositionDto possitionDto)
        {
            var target = await GetTargetByIdAsync(id);
            if (target == null)
            {
                throw new Exception("Target not found");
            }
            target.x = possitionDto.x;
            target.y = possitionDto.y;
            await context.SaveChangesAsync();
            return target;
        }

        public async Task<TargetModel?> MoveTarget(int targetId, MoveDto direction)
        {
            var target = await GetTargetByIdAsync(targetId);
            if (target == null)
            {
                throw new Exception("Agent not found");
            }

            if (directions.TryGetValue(direction.Direction, out var move))
            {
                target.x += move.x;
                target.y += move.y;
                await context.SaveChangesAsync();
                return target;
            }
            else
            {
                throw new Exception("Invalid direction");
            }


        }
        

        /*public async Task<UserModel> AuthenticateAsync(string email, string password)
        {
            var user = await FindByEmailAsync(email) ?? throw new Exception($"The user by email {email} does not found");
            var isValidPwd = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!isValidPwd)
            {
                throw new Exception("Wrong connection details");
            }
            return user;
        }*/
    }
}
