using TargetsRest.Models;

namespace TargetsRest.Dto
{
    public class TargetDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Photo_url { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
