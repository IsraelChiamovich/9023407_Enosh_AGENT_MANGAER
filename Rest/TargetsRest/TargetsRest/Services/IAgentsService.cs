using TargetsRest.Dto;
using TargetsRest.Models;

namespace TargetsRest.Services
{
    public interface IAgentsService
    {
        Task<int> CreateAgentAsync(AgentDto agentDto);
        Task<AgentModel> DeterminingAStartingPosition(int id, PositionDto possitionDto);
        Task<AgentModel?> GetAgentByIdAsync(int id);
        Task<List<AgentModel>> GetAllAgentsAsync();
        Task<AgentModel?> MoveAgent(int agentId, MoveDto direction);
        Task<MissionModel?> GetMissionByAgentAsync(int AgentId);

        //Task<UserModel> AuthenticateAsync(string email, string password);
    }
}
