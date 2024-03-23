using Datalayer.Interfaces;
using Datalayer.Models;
using Core.Services;
using Datalayer.Models.Users;
using Datalayer;
using Microsoft.EntityFrameworkCore;
using Datalayer.Models.Repositories;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Relocation_and_booking_services.Start;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}
)
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
        options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
        //options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        options.ClaimActions.MapAll();
    });
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFurnitureService, FurnitureService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IRentingService, RentingService>();
builder.Services.AddScoped<ITransportService, TransportService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIndustryUserService, IndustryUserService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddHttpClient();

builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddScoped<BookingRepo>();
builder.Services.AddScoped<EmailRepo>();
builder.Services.AddScoped<FurnitureRepo>();
builder.Services.AddScoped<IndustryUserRepo>();
builder.Services.AddScoped<JobRepo>();
builder.Services.AddScoped<RentingRepo>();
builder.Services.AddScoped<SchoolRepo>();
builder.Services.AddScoped<SchoolUserRepo>();
builder.Services.AddScoped<TransportRepo>();
builder.Services.AddScoped<UserRepo>();


builder.Services.AddDbContext<RelocationDbContext>(
       options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"), b => b.MigrationsAssembly("Relocation and booking services")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    var requestPath = context.Request.Path.Value;

    if (string.Equals(requestPath, "/", StringComparison.OrdinalIgnoreCase)
        || string.Equals(requestPath, "/index.html", StringComparison.OrdinalIgnoreCase))
    {
        var tokenSet = antiforgery.GetAndStoreTokens(context);
        context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!,
            new CookieOptions { HttpOnly = false });
    }

    return next(context);
});

app.Run();

