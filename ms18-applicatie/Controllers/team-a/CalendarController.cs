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

        public CalendarController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;

            _calendarSettings =
                configuration.GetSection("Calendar").Get<CalendarSettings>();
        }

        [HttpGet]
        [Route("welpen")]
        public async Task<IActionResult> Welpen()
        {
            var events = await GetCalendar(GetCalenderId(Calenders.Welpen), false);
            return new OkObjectResult(events);
        }

        [HttpGet]
        [Route("matrozen")]
        public async Task<IActionResult> Matrozen()
        {
            var events = await GetCalendar(GetCalenderId(Calenders.Matrozen), false);
            return new OkObjectResult(events);
        }

        [HttpGet]
        [Route("ZeeVerkenners")]
        public async Task<IActionResult> ZeeVerkenners()
        {
            var events = await GetCalendar(GetCalenderId(Calenders.ZeeVerkenners), false);
            return new OkObjectResult(events);
        }

        [HttpGet]
        [Route("stam")]
        public async Task<IActionResult> Stam()
        {
            var events = await GetCalendar(GetCalenderId(Calenders.Stam), false);
            return new OkObjectResult(events);
        }

        [HttpGet]
        [Route("global")]
        public async Task<IActionResult> Global()
        {
            var events = await GetCalendar(GetCalenderId(Calenders.Stam), false);
            return new OkObjectResult(events);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            var events = new List<CalenderEvent>();
            events.AddRange(await GetCalendar(GetCalenderId(Calenders.Matrozen)));
            events.AddRange(await GetCalendar(GetCalenderId(Calenders.Welpen)));
            events.AddRange(await GetCalendar(GetCalenderId(Calenders.ZeeVerkenners)));
            events.AddRange(await GetCalendar(GetCalenderId(Calenders.Stam)));
            events.AddRange(await GetCalendar(GetCalenderId(Calenders.Global)));
            return new OkObjectResult(events);
        }

        private async Task<List<CalenderEvent>> GetCalendar(string calenderId, bool filterGlobal = true)
        {
            var calenderEvents = new List<CalenderEvent>();
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                ApiKey = _calendarSettings.ApiKey
            });

            var request = service.Events.List(calenderId);
            var response = await request.ExecuteAsync();
            var listItems = response.Items.ToList();

            if (!filterGlobal)
            {
                request = service.Events.List(calenderId);
                response = await request.ExecuteAsync();
                listItems.AddRange(response.Items);
            }

            foreach (var eventsItem in listItems)
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
                case Calenders.Global:
                    return _calendarSettings.GlobalId;
            }
            return "";
        }

        private enum Calenders
        {
            Stam,
            Matrozen,
            Welpen,
            ZeeVerkenners,
            Global,
        }
    }
}
