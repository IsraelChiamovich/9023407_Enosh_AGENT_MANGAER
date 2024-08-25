namespace TargetsMvc.Dto
{
    public enum MissionStatus
    {
        //הצעה
        Proposal,
        //מצוות למשימה
        assigned,
        //משימה הסתיימה
        ended
    }
    public class MissionDto
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public AgentDto Agent { get; set; }
        public TargetDto Target { get; set; }
        public double TimeLeft { get; set; }
        public double ActualTime { get; set; }
        public MissionStatus MissionStatus { get; set; }

    }
}
