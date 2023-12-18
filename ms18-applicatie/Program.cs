using Maasgroep.Database;
using Maasgroep.Database.Interfaces;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Admin;
using Maasgroep.Database.Orders;
using Maasgroep.Middleware;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<MaasgroepContext>();

// Add HTTP context
builder.Services.AddHttpContextAccessor();

// Add repositories
builder.Services.AddTransient<ICostCentreRepository, CostCentreRepository>();
builder.Services.AddTransient<IReceiptRepository, ReceiptRepository>();
builder.Services.AddTransient<IReceiptApprovalRepository, ReceiptApprovalRepository>();
builder.Services.AddTransient<IReceiptPhotoRepository, ReceiptPhotoRepository>();
builder.Services.AddTransient<IReceiptStatusRepository, ReceiptStatusRepository>();
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<ITokenStoreRepository, TokenStoreRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IStockRepository, StockRepository>();

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

builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Make a call to User/Login, then paste your token here:",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });




var app = builder.Build();

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<TokenMiddleware>();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();