namespace TargetsMvc.Dto
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
    public class TargetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public required string ImageLink { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public TargetStatus Status { get; set; }
    }
}
