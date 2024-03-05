using Datalayer.Interfaces;
using Datalayer.Models;
using Core.Services;
using Datalayer.Models.Users;
using Datalayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IBookingService,BookingService>();
builder.Services.AddSingleton<IFurnitureService, FurnitureService>();      
builder.Services.AddSingleton<IJobService, JobService>();   
builder.Services.AddSingleton<IRentingService, RentingService>();  
builder.Services.AddSingleton<ITransportService, TransportService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IIndustryUserService, IndustryUserService>();
builder.Services.AddSingleton<ISchoolService, SchoolService>();
builder.Services.AddSingleton<UnitOfWork>();
string test = builder.Configuration.GetConnectionString("ConnectionString");

builder.Services.AddDbContext<RelocationDbContext>(
       options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"), b => b.MigrationsAssembly("Relocation and booking services")));

var app = builder.Build();



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
