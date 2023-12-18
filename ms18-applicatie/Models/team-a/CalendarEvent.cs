using Google.Apis.Calendar.v3.Data;
using ms18_applicatie.Controllers.team_a;

namespace ms18_applicatie.Models.team_a
{
    public class CalendarEvent
    {
        public CalendarEvent(Event googleEvent, CalendarController.Calendars calendarId)
        {
            Id = googleEvent.Id;
            Description = googleEvent.Description;
            Location = googleEvent.Location;
            CalendarId = calendarId;
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
        public CalendarEvent()
        {

        }
        public DateTime StarDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = null;
        public string Id { get; set; } = string.Empty;
        public string? Location { get; set; } = null;
        public CalendarController.Calendars CalendarId { get; set; }


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
