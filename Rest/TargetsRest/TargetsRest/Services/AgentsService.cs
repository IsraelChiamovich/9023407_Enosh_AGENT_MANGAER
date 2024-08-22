using Microsoft.EntityFrameworkCore;
using TargetsRest.Data;
using TargetsRest.Dto;
using TargetsRest.Models;

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

            if (directions.TryGetValue(direction.Direction, out var move))
            {
                agent.x += move.X;
                agent.y += move.Y;
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
