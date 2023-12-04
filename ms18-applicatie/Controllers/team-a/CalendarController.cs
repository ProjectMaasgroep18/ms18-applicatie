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
        public async Task<IActionResult> RemoveEvent(Calenders calenderName, string id)
        {
            try
            {

                var request = _calendarService.Events.Delete(GetCalenderId(calenderName), id);
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
        public async Task<IActionResult> EditEvent(Calenders calenderName, CalenderEvent calendarEvent)
        {
            try
            {

                var request = _calendarService.Events.Get(GetCalenderId(calenderName), calendarEvent.Id);
                var googleCalendarEvent = await request.ExecuteAsync();
                if (googleCalendarEvent == null)
                    return new NotFoundResult();

                googleCalendarEvent = calendarEvent.ToGoogleEvent(googleCalendarEvent);

                var requestUpdate = _calendarService.Events.Patch(googleCalendarEvent, GetCalenderId(calenderName), calendarEvent.Id);
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
        public async Task<IActionResult> AddEvent(Calenders calenderName, CalenderEvent calendarEvent)
        {
            try
            {

                var googleEvent = calendarEvent.ToGoogleEvent();

                EventsResource.InsertRequest request = _calendarService.Events.Insert(googleEvent, GetCalenderId(calenderName));
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

                var events = await GetCalendar(GetCalenderId(Calenders.Welpen), false);
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
                var events = await GetCalendar(GetCalenderId(Calenders.Matrozen), false);
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
                var events = await GetCalendar(GetCalenderId(Calenders.ZeeVerkenners), false);
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

                var events = await GetCalendar(GetCalenderId(Calenders.Stam), false);
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

                var events = await GetCalendar(GetCalenderId(Calenders.Global), false);
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

                var events = new List<CalenderEvent>();
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(GetCalenderId(Calenders.Matrozen)))));
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(GetCalenderId(Calenders.Welpen)))));
                tasks.Add(Task.Run(async () =>
                    events.AddRange(await GetCalendar(GetCalenderId(Calenders.ZeeVerkenners)))));
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(GetCalenderId(Calenders.Stam)))));
                tasks.Add(Task.Run(async () => events.AddRange(await GetCalendar(GetCalenderId(Calenders.Global)))));

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

        private async Task<List<CalenderEvent>> GetCalendar(string calenderId, bool filterGlobal = true)
        {
            var calenderEvents = new List<CalenderEvent>();

            var request = _calendarService.Events.List(calenderId);
            var response = await request.ExecuteAsync();
            var listItems = response.Items.ToList();

            if (!filterGlobal)
            {
                request = _calendarService.Events.List(GetCalenderId(Calenders.Global));
                response = await request.ExecuteAsync();
                listItems.AddRange(response.Items);
            }

            foreach (var eventsItem in listItems)
            {
                if (eventsItem == null)
                    continue;


                var calenderEvent = new CalenderEvent(eventsItem);
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

        public enum Calenders
        {
            Stam,
            Matrozen,
            Welpen,
            ZeeVerkenners,
            Global,
        }
    }
}
