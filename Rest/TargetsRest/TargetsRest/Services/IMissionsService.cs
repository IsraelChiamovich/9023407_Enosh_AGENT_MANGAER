using TargetsRest.Models;

namespace TargetsRest.Services
{
    public interface IMissionsService
    {
        Task<List<MissionModel>> GetAllMission();
        Task<MissionModel> GetMissionById(int id);
        Task<List<MissionModel>> FindAgentsAndTargetsToMissionAsync();
        Task<MissionModel?> CreateMissionAsync(AgentModel agent, TargetModel target);
        Task AssignAgentToMissionAsync(int missionId, int agentId);
        Task UpdateAllMissionsAsync();
        Task UpdateMissionAsync(int missionId);
        Task UpdateMissionToAssingdAsync(int missionId);
    }
}
