using Maasgroep.Filters;
using Maasgroep.Database;
using Maasgroep.Database.Interfaces;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Admin;
// using Maasgroep.Database.Orders;
// using Maasgroep.SharedKernel.Interfaces.Members;
// using Maasgroep.SharedKernel.Interfaces.Orders;
// using Maasgroep.Services;


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
// builder.Services.AddTransient<IOrderRepository, OrderRepository>();
// builder.Services.AddTransient<IMemberService, MemberService>();/////????

// Add services to the container.
builder.Services.AddControllersWithViews(options => {
    
    options.Filters.Add<MaasgroepExceptionFilter>();
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
app.UseSwagger();
app.UseSwaggerUI();


// app.UseHttpsRedirection(); // TODO Reenable for production, only disabled for testing
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();