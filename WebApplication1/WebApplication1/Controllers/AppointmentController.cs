using EF.service.@interface;
using EF.service;
using Microsoft.AspNetCore.Mvc;
using NLog;
using X.PagedList;
using WebApplication1.Data.models;

namespace WebApplication1.Controllers
{
    [Route("api")]

    public class AppointmentController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IAppointmentService appointmentService;
        private readonly Logger logger;

        public AppointmentController(IUserService userService, IRoleService roleService, IAppointmentService appointmentService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.appointmentService = appointmentService;
            this.logger = LogManager.GetCurrentClassLogger();
        }

        [HttpPost]
        [HttpGet]
        [Route("appointments")]
        public IActionResult Appointments(string sortOrder, string currentFilter, string searchString, int? page)
        {
            this.ViewBag.CurrentSort = sortOrder;
            this.ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            this.ViewBag.PatientSortParm = sortOrder == "Patient" ? "patient_desc" : "Patient";
            this.ViewBag.DoctorSortParm = sortOrder == "Doctor" ? "doctor_desc" : "Doctor";
            this.ViewBag.CurrentFilter = searchString;

            var appointments = this.appointmentService.GetAppointments();

            if (!string.IsNullOrEmpty(searchString))
            {
                appointments = appointments.Where(s =>
                    s.PatientRefNavigation.FirstName.Contains(searchString) ||
                    s.PatientRefNavigation.LastName.Contains(searchString) ||
                    s.DoctorRefNavigation.FirstName.Contains(searchString) ||
                    s.DoctorRefNavigation.LastName.Contains(searchString) ||
                    s.DateAndTime.ToString().Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "date_desc":
                    appointments = appointments.OrderByDescending(s => s.DateAndTime).ToList();
                    break;
                case "Patient":
                    appointments = appointments.OrderBy(s => s.PatientRefNavigation.LastName).ToList();
                    break;
                case "patient_desc":
                    appointments = appointments.OrderByDescending(s => s.PatientRefNavigation.LastName).ToList();
                    break;
                case "Doctor":
                    appointments = appointments.OrderBy(s => s.DoctorRefNavigation.LastName).ToList();
                    break;
                case "doctor_desc":
                    appointments = appointments.OrderByDescending(s => s.DoctorRefNavigation.LastName).ToList();
                    break;
                default:
                    appointments = appointments.OrderBy(s => s.DateAndTime).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            return this.View(new DoctorViewModel(null, null,appointments.ToPagedList(pageNumber, pageSize)));
        }
    }
}
