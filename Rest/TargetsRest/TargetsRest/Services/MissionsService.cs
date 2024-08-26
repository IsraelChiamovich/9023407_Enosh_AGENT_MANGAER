using Microsoft.EntityFrameworkCore;
using TargetsRest.Data;
using TargetsRest.Models;
using System.Globalization;

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

            if (mission == null)
            {
                throw new Exception("Mission not found");
            }

            return mission;
        }

        public async Task<List<MissionModel>> FindAgentsAndTargetsToMissionAsync()
        {
            var allMissions = await context.Missions.ToListAsync();

            var agents = await context.Agents
                .Where(a => a.Status == AgentStatus.dormant)
                .ToListAsync();

            var targets = await context.Targets
                .Where(t => t.Status == TargetStatus.Live)
                .ToListAsync();

            var existingMissions = await context.Missions
                .Where(m => m.MissionStatus != MissionStatus.ended)
                .ToListAsync();

            var missionsToAdd = agents.Zip(targets)
                .Select(x => (agent: x.First, target: x.Second, distance: CalculateDistance(x.First.x, x.First.y, x.Second.x, x.Second.y)))
                .Where(x => x.distance < 200)
                .Where(x => !existingMissions.Any(m => m.AgentId == x.agent.Id && m.TargetId == x.target.Id))
                .Select(x => new MissionModel()
                {
                    AgentId = x.agent.Id,
                    TargetId = x.target.Id,
                    Agent = x.agent,
                    Target = x.target,
                    TimeLeft = Math.Round(x.distance / 5.0, 2),
                    ActualTime = 0.0,
                    MissionStatus = MissionStatus.Proposal
                })
                .ToList();

            await context.Missions.AddRangeAsync(missionsToAdd);
            await context.SaveChangesAsync();

            return missionsToAdd;
        }

        public async Task<MissionModel?> CreateMissionAsync(AgentModel agent, TargetModel target)
        {
            var distance = CalculateDistance(agent.x, agent.y, target.x, target.y);
            var timeLeft = Math.Round(distance / 5.0, 2);

            if (await CheckIfMissionExists(agent, target))
            {
                throw new Exception("Mission with the same agent and target already exists.");
            }

            var mission = new MissionModel
            {
                AgentId = agent.Id,
                TargetId = target.Id,
                Agent = agent,
                Target = target,
                TimeLeft = timeLeft,
                ActualTime = 0.0,
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
            if (agent == null)
            {
                throw new Exception("Agent not found");
            }

            var existingMissions = await context.Missions
                .Where(x => x.AgentId == agent.Id && x.MissionStatus == MissionStatus.assigned)
                .ToListAsync();

            if (existingMissions.Any())
            {
                throw new Exception("Agent is already assigned to a mission");
            }

            mission.MissionStatus = MissionStatus.assigned;
            agent.Status = AgentStatus.activity;
            mission.ActualTime = DateTime.Now.TimeOfDay.TotalMinutes / 60.0;

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

            if (mission == null)
            {
                throw new Exception("Mission not found");
            }

            if (mission.MissionStatus == MissionStatus.assigned)
            {
                var currentTime = DateTime.Now.TimeOfDay.TotalMinutes / 60.0;
                mission.ActualTime = currentTime - mission.ActualTime;

                await MoveAgentToTarget(mission);

                if (IsAgentEliminetTarget(mission.Agent, mission.Target))
                {
                    mission.MissionStatus = MissionStatus.ended;
                    mission.Target.Status = TargetStatus.Eliminated;
                    mission.Agent.Status = AgentStatus.dormant;
                    mission.TimeLeft = 0;
                    var currentTime2 = DateTime.Now.TimeOfDay.TotalMinutes / 60.0;
                    mission.ActualTime = currentTime2 - mission.ActualTime;
                }
                else if (mission.TimeLeft <= 0)
                {
                    mission.Agent.Status = AgentStatus.dormant;
                    mission.Target.Status = TargetStatus.Live;
                    context.Missions.Remove(mission);
                    mission.ActualTime = 0.0; 
                }

                await context.SaveChangesAsync();
            }
            else
            {
                mission.ActualTime = 0.0; 
            }
        }

        public async Task UpdateMissionToAssingdAsync(int missionId)
        {
            var mission = await GetMissionById(missionId);

            if (mission == null)
            {
                throw new Exception("Mission not found");
            }

            mission.MissionStatus = MissionStatus.assigned;
            mission.ActualTime = DateTime.Now.TimeOfDay.TotalMinutes / 60.0; 

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

            mission.TimeLeft = Math.Round(CalculateDistance(agent.x, agent.y, target.x, target.y) / 5.0, 2);

            await context.SaveChangesAsync();
        }

        private void ValidatePosition(int x, int y)
        {
            if (x < 1 || x > 999 || y < 1 || y > 999)
            {
                throw new Exception("Position is out of range");
            }
        }

        private double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
