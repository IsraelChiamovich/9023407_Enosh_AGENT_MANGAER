using System.ComponentModel.DataAnnotations;

namespace TargetsRest.Models
{
    public enum TargetStatus
    {
        //חי
        Live,
        //חוסל
        Eliminated,
        //מצוות למשימה
        associatedMission
    }
    public class TargetModel
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Role { get; set; }
        [Required]
        public required string ImageLink { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public TargetStatus Status { get; set; }
    }
}
