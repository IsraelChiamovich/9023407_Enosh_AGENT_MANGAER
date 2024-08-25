using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TargetsRest.Data;
using TargetsRest.Models;
using TargetsRest.Utils;

namespace TargetsRest.Services
{
    public class MissionsService(ApplicationDbContext context) : IMissionsService
    {

        public async Task<List<MissionModel>> GetAllMission()
        {
            var missions = await context.Missions
                .Include(a => a.Agent)
                .Include(a => a.Target)
                .ToListAsync();

            return missions;
        }

        public async Task<MissionModel> GetMissionById(int id)
        {
            var mission = await context.Missions
                .Include(a => a.Agent)
                .Include(a => a.Target)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null) { throw new Exception("The mission does not found"); }

            return mission;
        }

        public async Task<(List<AgentModel> agents, List<TargetModel> targets)> FindAgentsAndTargetsToMissionAsync()
        {
            var agents = await context.Agents
                .Where(a => a.Status == AgentStatus.dormant)
                .ToListAsync();

            var targets = await context.Targets
                .Where(t => t.Status == TargetStatus.Live)
                .ToListAsync();

            var relevantMissions = new List<(AgentModel agent, TargetModel target)>();

            foreach (var agent in agents)
            {
                foreach (var target in targets)
                {
                    var distance = PossitionUtils.CalculateDistance(agent.x, agent.y, target.x, target.y);
                    if (distance < 200 && !await CheckIfMissionExists(agent, target))
                    {
                        relevantMissions.Add((agent, target));
                        await CreateMissionAsync(agent, target);
                    }
                }
            }

            var agentsInMissions = relevantMissions.Select(a => a.agent).Distinct().ToList();
            var targetsInMissions = relevantMissions.Select(a => a.target).Distinct().ToList();
            return (agentsInMissions, targetsInMissions);
        }

        public async Task<MissionModel?> CreateMissionAsync(AgentModel agent, TargetModel target)
        {
            var distance = PossitionUtils.CalculateDistance(agent.x, agent.y, target.x, target.y);
            var timeLeft = distance / 5.0;

            var mission = new MissionModel
            {
                AgentId = agent.Id,
                TargetId = target.Id,
                Agent = agent,
                Target = target,
                TimeLeft = timeLeft,
                ActualTime = DateTime.Now.TimeOfDay.TotalMinutes,
                MissionStatus = MissionStatus.Proposal,
            };

            await context.Missions.AddAsync(mission);
            await context.SaveChangesAsync();
            return mission;
        }

        public async Task AssignAgentToMissionAsync(int missionId, int agentId)
        {
            var mission = await GetMissionById(missionId);

            var agent = await context.Agents.FindAsync(agentId);
            if (agent == null) { throw new Exception("The agent does not found"); }

            var existingMissions = await context.Missions
                .Where(x => x.AgentId == agent.Id && x.MissionStatus == MissionStatus.assigned)
                .ToListAsync();

            if (existingMissions.Any()) { throw new Exception("The agent is already assigned to a mission"); }

            mission.MissionStatus = MissionStatus.assigned;
            agent.Status = AgentStatus.activity;

            await context.SaveChangesAsync();
        }

        public async Task UpdateAllMissionsAsync()
        {
            var missions = await GetAllMission();

            foreach (var mission in missions)
            {
                await UpdateMissionAsync(mission.Id);
            }
        }

        public async Task UpdateMissionAsync(int missionId)
        {
            var mission = await GetMissionById(missionId);

            if (mission == null) { throw new Exception("The mission does not found"); }

            await MoveAgentToTarget(mission);

            mission.ActualTime = DateTime.Now.TimeOfDay.TotalMinutes;

            if (IsAgentEliminetTarget(mission.Agent, mission.Target))
            {
                mission.MissionStatus = MissionStatus.ended;
                mission.Target.Status = TargetStatus.Eliminated;
                mission.Agent.Status = AgentStatus.dormant;
                mission.TimeLeft = 0;
            }
            else if (mission.TimeLeft <= 0)
            {
                mission.Agent.Status = AgentStatus.dormant;
                mission.Target.Status = TargetStatus.Live;
                context.Missions.Remove(mission);
            }

            await context.SaveChangesAsync();
        }


        public async Task UpdateMissionToAssingdAsync(int missionId)
        {
            var mission = await GetMissionById(missionId);

            if (mission == null)
            {
                throw new Exception("Mission not found");
            }

            mission.MissionStatus = MissionStatus.assigned;

            await context.SaveChangesAsync();
        }

        private async Task<bool> CheckIfMissionExists(AgentModel agent, TargetModel target)
        {
            return await context.Missions.AnyAsync(m => m.AgentId == agent.Id && m.TargetId == target.Id);
        }

        private bool IsAgentEliminetTarget(AgentModel agent, TargetModel target)
        {
            return agent.x == target.x && agent.y == target.y;
        }

        private async Task MoveAgentToTarget(MissionModel mission)
        {
            var agent = mission.Agent;
            var target = mission.Target;

            if (agent.x < target.x) agent.x += 1;
            else if (agent.x > target.x) agent.x -= 1;

            if (agent.y < target.y) agent.y += 1;
            else if (agent.y > target.y) agent.y -= 1;

            ValidatePosition(agent.x, agent.y);

            mission.TimeLeft = PossitionUtils.CalculateDistance(agent.x, agent.y, target.x, target.y) / 5.0;

            mission.ActualTime = DateTime.Now.TimeOfDay.TotalMinutes;

            await context.SaveChangesAsync();
        }


        private void ValidatePosition(int x, int y)
        {
            if (x < 1 || x > 999 || y < 1 || y > 999)
            {
                throw new Exception("Position is out of range");
            }
        }
    }
}
