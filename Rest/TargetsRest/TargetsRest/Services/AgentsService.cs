﻿using Microsoft.EntityFrameworkCore;
using TargetsRest.Data;
using TargetsRest.Dto;
using TargetsRest.Models;
using TargetsRest.Utils;

namespace TargetsRest.Services
{
    public class AgentsService(ApplicationDbContext context) : IAgentsService
    {
        private readonly Dictionary<string, (int X, int Y)> directions = new()
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

        public async Task<int> CreateAgentAsync(AgentDto agentDto)
        {
            AgentModel agent = new()
            {
                NickName = agentDto.Nickname,
                ImageLink = agentDto.PhotoUrl,
                Status = AgentStatus.dormant
            };

            await context.Agents.AddAsync(agent);
            await context.SaveChangesAsync();
            return agent.Id;

        }

        //בדיקה בעת הוספה
        public async Task CreateMissionForAgentAsync(AgentModel agent)
        {
            var targets = await context.Targets
                .Where(t => t.Status == TargetStatus.Live)
                .ToListAsync();

            var missionsToAdd = targets
                .Select(t => new
                {
                    Target = t,
                    Distance = DistanceCal.CalculateDistance(agent.x, agent.y, t.x, t.y)
                })
                .Where(x => x.Distance < 200)
                .Select(x => new MissionModel
                {
                    AgentId = agent.Id,
                    TargetId = x.Target.Id,
                    Agent = agent,
                    Target = x.Target,
                    TimeLeft = Math.Round(x.Distance / 5.0, 2),
                    ActualTime = 0.0,
                    MissionStatus = MissionStatus.Proposal
                })
                .ToList();

            if (missionsToAdd.Any())
            {
                await context.Missions.AddRangeAsync(missionsToAdd);
                await context.SaveChangesAsync();
            }
        }

        public async Task<MissionModel?> GetMissionByAgentAsync(int AgentId)
        {
            
            return await context.Missions
                .Include(a => a.Agent)
                .Include(t => t.Target)
                .Where(m => m.AgentId == AgentId).FirstOrDefaultAsync();

        }


        public async Task<AgentModel?> GetAgentByIdAsync(int id) =>
            await context.Agents.FindAsync(id);

        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
            await context.Agents.ToListAsync();

        
        public async Task<AgentModel> DeterminingAStartingPosition(int id, PositionDto possitionDto)
        {
            var agent = await GetAgentByIdAsync(id);
            if (agent == null)
            {
                throw new Exception("Agent not found");
            }
            agent.x = possitionDto.x;
            agent.y = possitionDto.y;
            await context.SaveChangesAsync();
            return agent;
        }

        public async Task<AgentModel?> MoveAgent(int agentId, MoveDto direction)
        {
            var agent = await GetAgentByIdAsync(agentId);
            if (agent == null)
            {
                throw new Exception("Agent not found");
            }

            CheckIfAgentIsActive(agent);

            return await MoveAgentPosition(agent, direction);
        }

        private void CheckIfAgentIsActive(AgentModel agent)
        {
            if (agent.Status == AgentStatus.activity)
            {
                throw new Exception("The agent is active and cannot be moved.");
            }
        }

        private async Task<AgentModel?> MoveAgentPosition(AgentModel agent, MoveDto moveDto)
        {
            if (directions.TryGetValue(moveDto.Direction, out var move))
            {
                if (agent.x > 1 && agent.x < 999)
                {
                    agent.x += move.X;
                }
                if (agent.y > 1 && agent.y < 999)
                {
                    agent.y += move.X;
                }

                await context.SaveChangesAsync();
                return agent;
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
