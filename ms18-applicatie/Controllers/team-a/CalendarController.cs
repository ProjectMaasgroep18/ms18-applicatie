using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Models.team_a;

namespace ms18_applicatie.Controllers.team_a
{
    [Route("Calendar")]
    public class CalendarController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CalendarSettings _calendarSettings;

        public CalendarController(ILogger<HomeController> logger, IConfiguration Configuration)
        {
            _logger = logger;

            _calendarSettings =
                Configuration.GetSection("Calendar").Get<CalendarSettings>();
        }

        [HttpGet]
        [Route("welpen")]
        public async Task<IActionResult> Welpen()
        {
            var events = await GetCalendar(Calenders.Welpen);
            return new OkObjectResult(events);
        }

        [HttpGet]
        [Route("matrozen")]
        public async Task<IActionResult> Matrozen()
        {
            var events = await GetCalendar(Calenders.Matrozen);
            return new OkObjectResult(events);
        }

        [HttpGet]
        [Route("ZeeVerkenners")]
        public async Task<IActionResult> ZeeVerkenners()
        {
            var events = await GetCalendar(Calenders.ZeeVerkenners);
            return new OkObjectResult(events);
        }

        [HttpGet]
        [Route("stam")]
        public async Task<IActionResult> Stam()
        {
            var events = await GetCalendar(Calenders.Stam);
            return new OkObjectResult(events);
        }

        private async Task<List<CalenderEvent>> GetCalendar(Calenders calenderName)
        {
            var calenderEvents = new List<CalenderEvent>();
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                ApiKey = _calendarSettings.ApiKey
            });

            var request = service.Events.List(GetCalenderId(calenderName));
            var response = await request.ExecuteAsync();
            foreach (var eventsItem in response.Items)
            {
                if (eventsItem == null)
                    continue;
                if (eventsItem.Start.DateTimeRaw == null || eventsItem.End.DateTimeRaw == null)
                    continue;
                var calenderEvent = new CalenderEvent();
                if (DateTime.TryParse(eventsItem.Start.DateTimeRaw, out var startDate))
                    calenderEvent.StarDateTime = startDate;
                if (DateTime.TryParse(eventsItem.End.DateTimeRaw, out var endDate))
                    calenderEvent.EndDateTime = endDate;
                calenderEvent.Title = eventsItem.Summary;
                calenderEvents.Add(calenderEvent);
            }
            return calenderEvents;
        }

        private string GetCalenderId(Calenders calenderName)
        {
            switch (calenderName)
            {
                case Calenders.Matrozen:
                    return _calendarSettings.MatrozenId;
                case Calenders.Stam:
                    return _calendarSettings.StamId;
                case Calenders.Welpen:
                    return _calendarSettings.WelpenId;
                case Calenders.ZeeVerkenners:
                    return _calendarSettings.ZeeverkennersId;
            }
            return "";
        }

        private enum Calenders
        {
            Stam,
            Matrozen,
            Welpen,
            ZeeVerkenners
        }
    }
}
