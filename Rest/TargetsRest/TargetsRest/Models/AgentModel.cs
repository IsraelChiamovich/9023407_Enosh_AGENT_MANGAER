using System.ComponentModel.DataAnnotations;

namespace TargetsRest.Models
{
    public enum AgentStatus
    {
        //רדום
        dormant,
        //בפעילות
        activity
    }
    public class AgentModel
    {
        public int Id { get; set; }
        [Required]
        public required string NickName { get; set; }
        [Required]
        public required string ImageLink { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public AgentStatus Status { get; set; }
    }
}
