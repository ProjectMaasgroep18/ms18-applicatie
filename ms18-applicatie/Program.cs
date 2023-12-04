using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.OpenApi.Models;
using ms18_applicatie.Models.team_a;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(s =>
{

    s.SwaggerDoc("v1", new OpenApiInfo { Title = "m18-applicatie", Version = "v1" });
    s.CustomOperationIds(e => e.ActionDescriptor.RouteValues["action"]);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });
});

builder.Services.Configure<CalendarSettings>(builder.Configuration.GetSection(nameof(CalendarSettings)));

builder.Services.AddSingleton((x) =>
{
    var settings = builder.Configuration
        .GetSection(nameof(CalendarSettings))
        .Get<CalendarSettings>();

    string[] scopes = { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents };
    using var stream =
           new FileStream(settings.FilePath, FileMode.Open, FileAccess.Read);
    var credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
    

    //Create the Calendar service.
    return new CalendarService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = "project c",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("../swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseSwagger();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("MyAllowSpecificOrigins");

app.UseAuthorization();
app.MapControllers();

app.Run();