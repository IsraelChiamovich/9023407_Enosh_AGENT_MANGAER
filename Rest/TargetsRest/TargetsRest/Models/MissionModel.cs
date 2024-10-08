﻿namespace TargetsRest.Models
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
    public class MissionModel
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public AgentModel Agent { get; set; }
        public TargetModel Target { get; set; }
        public double TimeLeft { get; set; }
        public double ActualTime { get; set; }
        public MissionStatus MissionStatus { get; set; }

    }
}
