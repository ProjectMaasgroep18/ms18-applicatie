using Maasgroep.Filters;
using Maasgroep.Database;
using Maasgroep.Database.Interfaces;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Admin;
using Maasgroep.Database.Orders;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<MaasgroepContext>();

// Add repositories
builder.Services.AddTransient<ICostCentreRepository, CostCentreRepository>();
builder.Services.AddTransient<IReceiptRepository, ReceiptRepository>();
builder.Services.AddTransient<IReceiptApprovalRepository, ReceiptApprovalRepository>();
builder.Services.AddTransient<IReceiptPhotoRepository, ReceiptPhotoRepository>();
builder.Services.AddTransient<IReceiptStatusRepository, ReceiptStatusRepository>();
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews(options => {
    
    // Filter special MaasgroepExceptions that are thrown in our controllers
    options.Filters.Add<MaasgroepExceptionFilter>();
    
}).ConfigureApiBehaviorOptions(options =>
{
    // Model validation error when posting/putting data
    options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(new {
        error = 400,
        message = "Ongeldige gegevens opgegeven",
    });
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();