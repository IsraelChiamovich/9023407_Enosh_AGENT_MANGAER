using TargetsRest.Models;

namespace TargetsRest.Services
{
    public interface IMissionsService
    {
        Task<List<MissionModel>> GetAllMission();
        Task<MissionModel> GetMissionById(int id);

        Task<(List<AgentModel> agents, List<TargetModel> targets)> FindAgentsAndTargetsToMissionAsync();
        Task<MissionModel?> CreateMissionAsync(AgentModel agent, TargetModel target);
        Task AssignAgentToMissionAsync(int missionId, int agentId);
        Task UpdateMissionAsync(int missionId);
        Task UpdateAllMissionsAsync();
        Task UpdateMissionToAssingdAsync(int missionId);
    }
}
