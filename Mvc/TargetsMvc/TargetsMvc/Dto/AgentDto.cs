using System.ComponentModel.DataAnnotations;

namespace TargetsMvc.Dto
{
    public enum AgentStatus
    {
        //רדום
        dormant,
        //בפעילות
        activity
    }
    public class AgentDto
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string ImageLink { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public AgentStatus Status { get; set; }
    }
}
