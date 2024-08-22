/*using Microsoft.AspNetCore.Http.HttpResults;
using TargetsRest.Data;
using TargetsRest.Models;
using TargetsRest.Utils;

namespace TargetsRest.Services
{
    public class MissionsService(ApplicationDbContext context, ITargetService targetService, IAgentsService agentsService) : IMissionsService
    {
        public async Task<List<MissionModel>> CreateMissionsIfConditionsExist()
        {
            var a = await targetService.GetAllTargetsAsync();
            var b = await agentsService.GetAllAgentsAsync();
            var xT = await context.Targets.Select(x => x.x);
            var yT = await context.Targets.Select(x => x.y);
            var xA = await context.Agents.Select(x => x.x);
            var yA = await context.Agents.Select(x => x.y);
            var desh = CalculateDistance(int xA, int yA, int xT, int yT);
            selectAllTargets...where Distance <= 200
            context.AddRangeAsync(Mission.MissionStatus = Proposal)
                returen list MissionModel where MissionStatus = Proposal && TargetStatus.Live && AgentStatus.dormant
        }

        public Task<List<MissionModel>> GetAllMission()
        {

        }

        public Task<MissionModel> GetMissionById(int id)
        {

        }
    }
}
*/