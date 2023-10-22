using Microsoft.Extensions.DependencyInjection;
using ms18_applicatie.Controllers;
using ms18_applicatie.Database;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<MaasgroepContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "Receipt",
                pattern: "api/Receipt/{*receipt}",
                defaults: new { controller = "DeclarationApi", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();