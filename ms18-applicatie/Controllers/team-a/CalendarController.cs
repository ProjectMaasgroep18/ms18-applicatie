using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Models.team_a;
using Google.Apis.Auth.OAuth2;

namespace ms18_applicatie.Controllers.team_a
{
    [Route("Calendar")]
    public class CalendarController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CalendarSettings _calendarSettings;
        private readonly CalendarService _calendarService;
        public CalendarController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;

            _calendarSettings =
                configuration.GetSection("Calendar").Get<CalendarSettings>();

            string[] scopes = new string[] { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents };
            GoogleCredential credential;
            using (var stream = new FileStream("./Controllers/team-a/service_accountInfo.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(scopes);
            }

            //Create the Calendar service.
            _calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "project c",
            });
        }

        [HttpDelete]
        [Route("Event")]
        public async Task<IActionResult> RemoveEvent(Calenders calenderName, string id)
        {
            var request = _calendarService.Events.Delete(GetCalenderId(calenderName), id);
            await request.ExecuteAsync();
            return new OkResult();
        }

        [HttpPatch]
        [Route("Event")]
        public async Task<IActionResult> EditEvent(Calenders calenderName, CalenderEvent calendarEvent)
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

        [HttpPost]
        [Route("Event")]
        public async Task<IActionResult> AddEvent(Calenders calenderName, CalenderEvent calendarEvent)
        {
            var googleEvent = calendarEvent.ToGoogleEvent();

            EventsResource.InsertRequest request = _calendarService.Events.Insert(googleEvent, GetCalenderId(calenderName));
            Event createdEvent = await request.ExecuteAsync();
            return new OkResult();
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
            var events = await GetCalendar(GetCalenderId(Calenders.ZeeVerkenners), true);
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
