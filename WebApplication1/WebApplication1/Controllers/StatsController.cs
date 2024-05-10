using EF.service.@interface;
using EF.service;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api")]
    public class StatsController : Controller
    {
        private readonly IRoleService roleService;
        private readonly IUserService userService;
        private readonly IAppointmentService appointmentService;

        public StatsController(
            IRoleService roleService,
            IUserService userService,
            IAppointmentService appointmentService)
        {
            this.roleService = roleService;
            this.userService = userService;
            this.appointmentService = appointmentService;
        }

        [HttpGet]
        [Route("stats")]
        public IActionResult DisplayStats()
        {
            var userCount = this.userService.GetPatients().Count();
            var appointmentCount = this.appointmentService.GetAppointments().Count();
            var doctorCount = this.userService.GetDoctors().Count;

            this.ViewBag.UserCount = userCount;
            this.ViewBag.AppointmentCount = appointmentCount;
            this.ViewBag.DoctorCount = doctorCount;

            return this.View();
        }
    }
}
