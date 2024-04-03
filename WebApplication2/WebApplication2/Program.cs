using EF.context;
using EF.service;
using EF.service.impl;
using EF.service.@interface;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<NeondbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbConectionString"));
            });

            builder.Services.AddScoped<IRoleService, RoleServiceImpl>();
            builder.Services.AddScoped<IAppointmentService, AppointmentServiceImpl>();
            builder.Services.AddScoped<IUserService, UserServiceImpl>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}