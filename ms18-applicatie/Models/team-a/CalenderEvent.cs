using Google.Apis.Calendar.v3.Data;

namespace ms18_applicatie.Models.team_a
{
    public class CalenderEvent
    {
        public CalenderEvent(Event googleEvent)
        {
            Id = googleEvent.Id;
            Description = googleEvent.Description;
            Location = googleEvent.Location;
            if (googleEvent.Start.Date != null && googleEvent.End.Date != null)
            {
                StarDateTime = DateTime.Parse(googleEvent.Start.Date);
                EndDateTime = DateTime.Parse(googleEvent.End.Date);
            }
            else if (googleEvent.Start.DateTimeRaw == null || googleEvent.End.DateTimeRaw == null)
                return;
            else
            {
                if (DateTime.TryParse(googleEvent.Start.DateTimeRaw, out var startDate))
                    StarDateTime = startDate;
                if (DateTime.TryParse(googleEvent.End.DateTimeRaw, out var endDate))
                    EndDateTime = endDate;
            }
            Title = googleEvent.Summary;
        }
        public CalenderEvent()
        {

        }
        public DateTime StarDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;


        public Event ToGoogleEvent(Event? googleEvent = null)
        {
            googleEvent ??= new Event();

            googleEvent.Summary = Title;
            googleEvent.Description = Description;
            googleEvent.Start = new();
            googleEvent.End = new();
            googleEvent.Start.DateTimeDateTimeOffset = StarDateTime;
            googleEvent.End!.DateTimeDateTimeOffset = EndDateTime;
            googleEvent.Location = Location;

            return googleEvent;
        }
    }
}
