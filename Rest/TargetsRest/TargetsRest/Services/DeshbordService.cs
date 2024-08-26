using Microsoft.EntityFrameworkCore;
using TargetsRest.Data;
using TargetsRest.Models;

namespace TargetsRest.Services
{
    public class DeshbordService(ApplicationDbContext context) : IDeshbordService
    {
        public async Task<int> GetNumOfAllAgentsAsync() =>
             await context.Agents.CountAsync();

        public async Task<int> GetNumOfAllActiveAgentsAsync() =>
             await context.Agents.CountAsync(a => a.Status == AgentStatus.activity);

        public async Task<int> GetNumOfAllTargetsAsync() =>
             await context.Targets.CountAsync();

        public async Task<int> GetNumOfAllEliminateTargetsAsync() =>
             await context.Targets.CountAsync(t => t.Status == TargetStatus.Eliminated);

        public async Task<int> GetNumOfAllMissionsAsync() =>
             await context.Missions.CountAsync();

        public async Task<int> GetNumOfAllAssignedMissionsAsync() =>
             await context.Missions.CountAsync(m => m.MissionStatus == MissionStatus.assigned);

        public async Task<int> GetNumOfAllRelevantMissionsAsync() =>
             await context.Missions.CountAsync(m => m.MissionStatus == MissionStatus.Proposal);
    }

}