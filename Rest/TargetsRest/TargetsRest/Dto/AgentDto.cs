using TargetsRest.Models;

namespace TargetsRest.Dto
{
    public class AgentDto
    {
        public string NickName { get; set; }
        public string Photo_url { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public DirectionDto Direction { get; set; }
    }
}
