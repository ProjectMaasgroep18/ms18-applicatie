namespace ms18_applicatie.Models.team_a
{
    public class CalenderEvent
    {
        public DateTime StarDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

    }
}
