using Microsoft.EntityFrameworkCore;
using Ms18.Application.Interface.TeamD;
using Ms18.Application.Repository.TeamD;
using Ms18.Database;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
ConfigureApp(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<MaasgroepContext>(options =>
        options.UseNpgsql(connectionString));

    services.AddControllersWithViews();
    services.AddSwaggerGen();

    services.AddScoped<IFolderRepository, FolderRepository>();
    services.AddScoped<IPhotoRepository, PhotoRepository>();
}

void ConfigureApp(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    // app.UseHttpsRedirection(); // TODO: Reenable for production
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllers();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}