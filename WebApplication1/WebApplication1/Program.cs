using EF.context;
using EF.service;
using EF.service.impl;
using EF.service.@interface;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Logger is working! Hospital Doctor Dre.");
            logger.Warn("Logger WARN message!");
            logger.Error("logger EROR message!");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<NeondbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbConectionString"));
            });

            builder.Services.AddScoped<IRoleService, RoleServiceImpl>();
            builder.Services.AddScoped<IAppointmentService, AppointmentServiceImpl>();
            builder.Services.AddScoped<IUserService, UserServiceImpl>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
