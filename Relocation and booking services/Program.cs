using Datalayer.Interfaces;
using Datalayer.Models;
using Core.Services;
using Datalayer.Models.Users;
using Datalayer;
using Microsoft.EntityFrameworkCore;
using Datalayer.Models.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBookingService,BookingService>();
builder.Services.AddScoped<IFurnitureService, FurnitureService>();      
builder.Services.AddScoped<IJobService, JobService>();   
builder.Services.AddScoped<IRentingService, RentingService>();  
builder.Services.AddScoped<ITransportService, TransportService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIndustryUserService, IndustryUserService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();

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
app.UseRouting();
app.MapControllers();
app.Run();
