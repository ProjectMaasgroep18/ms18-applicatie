using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Models.team_a;
using Microsoft.Extensions.Options;

namespace ms18_applicatie.Controllers.team_a
{
    [Route("Calendar")]
    public class CalendarController : Controller
    {
        private readonly CalendarSettings _calendarSettings;
        private readonly CalendarService _calendarService;
        private readonly ILogger<CalendarController> _logger;
        public CalendarController(ILogger<CalendarController> logger, CalendarService calendarService, IOptions<CalendarSettings> calendarSettings)
        {
            _logger = logger;
            _calendarService = calendarService;
            _calendarSettings = calendarSettings.Value;
        }

        [HttpDelete]
        [Route("Event")]
        public async Task<IActionResult> RemoveEvent(Calendars calendarName, string id)
        {
            try
            {
                var request = _calendarService.Events.Delete(GetCalendarId(calendarName), id);
                await request.ExecuteAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "delete Event call exception");
                return new BadRequestResult();
            }
        }

        [HttpPatch]
        [Route("Event")]
        public async Task<IActionResult> EditEvent(Calendars calendarName, CalendarEvent calendarEvent)
        {
            try
            {
                var request = _calendarService.Events.Get(GetCalendarId(calendarName), calendarEvent.Id);
                var googleCalendarEvent = await request.ExecuteAsync();
                if (googleCalendarEvent == null)
                    return new NotFoundResult();

                googleCalendarEvent = calendarEvent.ToGoogleEvent(googleCalendarEvent);

                var requestUpdate = _calendarService.Events.Patch(googleCalendarEvent, GetCalendarId(calendarName), calendarEvent.Id);
                await requestUpdate.ExecuteAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "patch Event call exception");
                return new BadRequestResult();
            }
        }

        [HttpPost]
        [Route("Event")]
        public async Task<IActionResult> AddEvent(Calendars calendarName, CalendarEvent calendarEvent)
        {
            try
            {
                var googleEvent = calendarEvent.ToGoogleEvent();
                EventsResource.InsertRequest request = _calendarService.Events.Insert(googleEvent, GetCalendarId(calendarName));
                Event createdEvent = await request.ExecuteAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "post Event call exception");
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("welpen")]
        public async Task<IActionResult> Welpen()
        {
            try
            {
                var events = await GetCalendar(Calendars.Welpen, false);
                return new OkObjectResult(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "welpen call exception");
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("matrozen")]
        public async Task<IActionResult> Matrozen()
        {
            try
            {
                var events = await GetCalendar(Calendars.Matrozen, false);
                return new OkObjectResult(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "matrozen call exception");
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("ZeeVerkenners")]
        public async Task<IActionResult> ZeeVerkenners()
        {
            try
            {
                var events = await GetCalendar(Calendars.ZeeVerkenners, false);
                return new OkObjectResult(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "zeeverkenners call exception");
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("stam")]
        public async Task<IActionResult> Stam()
        {
            try
            {

                var events = await GetCalendar(Calendars.Stam, false);
                return new OkObjectResult(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "stam call exception");
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("global")]
        public async Task<IActionResult> Global()
        {
            try
            {
                var events = await GetCalendar(Calendars.Global, false);
                return new OkObjectResult(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "global call exception");
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            try
            {
                var tasks = new List<Task>();

                var events = new List<CalendarEvent>();
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(Calendars.Matrozen))));
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(Calendars.Welpen))));
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(Calendars.ZeeVerkenners))));
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(Calendars.Stam))));
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(Calendars.Global))));

                Task t = Task.WhenAll(tasks);
                await t.WaitAsync(CancellationToken.None);

                return new OkObjectResult(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "all call exception");
                return new BadRequestResult();
            }
        }

        private async Task<List<CalendarEvent>> GetCalendar(Calendars calenderId, bool filterGlobal = true)
        {
            var calenderEvents = new List<CalendarEvent>();

            var request = _calendarService.Events.List(GetCalendarId(calenderId));
            var response = await request.ExecuteAsync();
            var listItems = response.Items.ToList();

            foreach (var eventsItem in listItems)
            {
                if (eventsItem == null)
                    continue;

                var calenderEvent = new CalendarEvent(eventsItem, calenderId);
                calenderEvents.Add(calenderEvent);
            }

            if (!filterGlobal)
            {
                request = _calendarService.Events.List(GetCalendarId(Calendars.Global));
                response = await request.ExecuteAsync();
                listItems.AddRange(response.Items);
            }

            return calenderEvents;
        }

        private string GetCalendarId(Calendars calendarName)
        {
            switch (calendarName)
            {
                case Calendars.Matrozen:
                    return _calendarSettings.MatrozenId;
                case Calendars.Stam:
                    return _calendarSettings.StamId;
                case Calendars.Welpen:
                    return _calendarSettings.WelpenId;
                case Calendars.ZeeVerkenners:
                    return _calendarSettings.ZeeverkennersId;
                case Calendars.Global:
                    return _calendarSettings.GlobalId;
            }
            return "";
        }

        public enum Calendars
        {
            Stam,
            Matrozen,
            Welpen,
            ZeeVerkenners,
            Global,
        }
    }
}
