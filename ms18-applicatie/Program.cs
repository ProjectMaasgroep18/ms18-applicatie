using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.Interfaces.Members;
using Maasgroep.SharedKernel.Interfaces.Orders;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Members;
using Maasgroep.Database.Orders;
using ms18_applicatie.Services;


var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<MaasgroepContext>();
builder.Services.AddTransient<IReceiptRepository, ReceiptRepository>();
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IMemberService, MemberService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// app.UseHttpsRedirection(); // TODO Reenable for production, only disabled for testing
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();