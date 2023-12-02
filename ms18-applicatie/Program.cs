using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.Database.Receipts;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<MaasgroepContext>();
builder.Services.AddTransient<IReceiptRepository, ReceiptRepository>();

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