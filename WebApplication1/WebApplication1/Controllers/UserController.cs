using EF;
using EF.service;
using EF.service.@interface;
using Microsoft.AspNetCore.Mvc;
using NLog;
using WebApplication1.Data;
using WebApplication1.Data.DTO.User;
using X.PagedList;

namespace WebApplication1.Controllers
{
    [Route("api")]

    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IAppointmentService appointmentService;
        private readonly Logger logger;

        public UserController(IUserService userService,IRoleService roleService, IAppointmentService appointmentService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.appointmentService = appointmentService;
            this.logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            this.logger.Info("Returning Login page");
            return this.View();
        }

        [HttpGet]
        [Route("registration")]
        public IActionResult Registration()
        {
            this.logger.Info("Returning Registration page");
            return this.View(new UserRegistrationDTO());
        }

        [HttpPost]
        [Route("registration")]
        public IActionResult Registration(UserRegistrationDTO user)
        {
            if (!this.ModelState.IsValid)
            {
                this.logger.Warn("User fields are invalid");

                return this.BadRequest(this.ModelState);
            }
            else
            {
                this.userService.RegisterPatient(user);
                this.logger.Info("Success registariton new Patient");
            }

            this.logger.Info("Returning Login page");
            return this.Redirect("/api/login");
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(
            [FromForm] string email,
            [FromForm] string password
            )
        {
            User user;
            try
            {
                user = this.userService.FindByEmail(email);
            }
            catch (Exception ex)
            {
                this.logger.Warn($"User with email:{email} not found");
                return this.View();
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!passwordMatch)
            {
                this.logger.Warn($"Incorrect password for user{email}");
                return this.View();
            }
            else
            {
                Context.UserId = user.UserId;
                this.logger.Info($"Setted new UserId:{user.UserId}");
                if (user.RoleRef == this.roleService.GetPatientRole().RoleId)
                {
                    this.logger.Info("Redirecting to patient page");
                    return this.Redirect("/patient");
                } 
                else if(user.RoleRef == this.roleService.GetDoctorRole().RoleId)
                {
                    this.logger.Info("Redirecting to doctor page");
                    return this.Redirect("/doctor");
                }
                else
                {
                    this.logger.Info("Redirecting to admin page");
                    return this.Redirect("/admin");
                }
            }
        }

        [HttpGet]
        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            this.logger.Info("Returning ForgotPassword page");
            return this.View();
        }

        [HttpPost]
        [Route("forgot-password")]
        public IActionResult ForgotPassword([FromForm] string email)
        {
            try
            {
                this.userService.ChangePasswordByEmail(email);
            }
            catch (Exception ex)
            {
                this.logger.Warn($"User with email:{email} not found");
                return this.View();
            }

            this.logger.Info($"Password for user:{email} was successfully changed");
            this.logger.Info("Returning Login page");

            return this.Redirect("/api/login");
        }

        [HttpPost]
        [HttpGet]
        [Route("doctors")]
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            this.ViewBag.CurrentSort = sortOrder;
            this.ViewBag.LastNameSortParm = string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            this.ViewBag.PhoneSortParm = sortOrder == "Phone" ? "phone_desc" : "Phone";
            this.ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewBag.CurrentFilter = searchString;

            var doctors = this.userService.GetDoctors();

            if (!string.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(d => d.LastName.Contains(searchString) ||
                d.FirstName.Contains(searchString) ||
                d.Email.Contains(searchString) ||
                d.Phone.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "lastname_desc":
                    doctors = doctors.OrderByDescending(d => d.LastName).ToList();
                    break;
                case "Phone":
                    doctors = doctors.OrderBy(d => d.Phone).ToList();
                    break;
                case "phone_desc":
                    doctors = doctors.OrderByDescending(d => d.Phone).ToList();
                    break;
                case "Email":
                    doctors = doctors.OrderBy(d => d.Email).ToList();
                    break;
                case "email_desc":
                    doctors = doctors.OrderByDescending(d => d.Email).ToList();
                    break;
                default:
                    doctors = doctors.OrderBy(d => d.LastName).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;
            return this.View(doctors.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [HttpGet]
        [Route("patients")]
        public IActionResult Patients(string sortOrder, string currentFilter, string searchString, int? page)
        {
            this.ViewBag.CurrentSort = sortOrder;
            this.ViewBag.LastNameSortParm = string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            this.ViewBag.PhoneSortParm = sortOrder == "Phone" ? "phone_desc" : "Phone";
            this.ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewBag.CurrentFilter = searchString;

            var patients = this.userService.GetPatients();

            if (!string.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(d => d.LastName.Contains(searchString) ||
                d.FirstName.Contains(searchString) ||
                d.Email.Contains(searchString) ||
                d.Phone.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "lastname_desc":
                    patients = patients.OrderByDescending(d => d.LastName).ToList();
                    break;
                case "Phone":
                    patients = patients.OrderBy(d => d.Phone).ToList();
                    break;
                case "phone_desc":
                    patients = patients.OrderByDescending(d => d.Phone).ToList();
                    break;
                case "Email":
                    patients = patients.OrderBy(d => d.Email).ToList();
                    break;
                case "email_desc":
                    patients = patients.OrderByDescending(d => d.Email).ToList();
                    break;
                default:
                    patients = patients.OrderBy(d => d.LastName).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;
            return this.View(patients.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [HttpGet]
        [Route("patient/{userId}")]
        public IActionResult PatientAppointments(int userId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            this.ViewBag.CurrentSort = sortOrder;
            this.ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            this.ViewBag.DoctorSortParm = sortOrder == "Doctor" ? "doctor_desc" : "Doctor";
            this.ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewBag.CurrentFilter = searchString;

            var appointments = this.appointmentService.GetAppointmentsByUserId(userId).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                appointments = appointments.Where(s => s.DoctorRefNavigation.LastName.Contains(searchString)
                                               || s.DoctorRefNavigation.FirstName.Contains(searchString)
                                               || s.DoctorRefNavigation.Patronymic.Contains(searchString)
                                               || s.Status.Contains(searchString)
                                               || s.Message.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    appointments = appointments.OrderByDescending(s => s.DateAndTime);
                    break;
                case "Doctor":
                    appointments = appointments.OrderBy(s => s.DoctorRefNavigation.LastName);
                    break;
                case "doctor_desc":
                    appointments = appointments.OrderByDescending(s => s.DoctorRefNavigation.LastName);
                    break;
                case "status":
                    appointments = appointments.OrderBy(s => s.Status);
                    break;
                case "status_desc":
                    appointments = appointments.OrderByDescending(s => s.Status);
                    break;
                default:
                    appointments = appointments.OrderBy(s => s.DateAndTime);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            var nearestAppointment = this.appointmentService.GetAppointmentsByUserId(userId)
                        .Where(a => a.DateAndTime > DateTime.Now)
                        .OrderBy(a => a.DateAndTime)
                        .FirstOrDefault();

            this.ViewBag.NearestAppointment = nearestAppointment;

            return this.View(appointments.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [HttpGet]
        [Route("doctor/{userId}")]
        public IActionResult DoctorAppointments(int userId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            this.ViewBag.CurrentSort = sortOrder;
            this.ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            this.ViewBag.PatientSortParm = sortOrder == "Patient" ? "patient_desc" : "Patient";
            this.ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewBag.CurrentFilter = searchString;

            var appointments = this.appointmentService.GetAppointmentsByUserId(userId).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                appointments = appointments.Where(s => s.DoctorRefNavigation.LastName.Contains(searchString)
                                               || s.DoctorRefNavigation.FirstName.Contains(searchString)
                                               || s.DoctorRefNavigation.Patronymic.Contains(searchString)
                                               || s.Status.Contains(searchString)
                                               || s.Message.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    appointments = appointments.OrderByDescending(s => s.DateAndTime);
                    break;
                case "Patient":
                    appointments = appointments.OrderBy(s => s.DoctorRefNavigation.LastName);
                    break;
                case "patient_desc":
                    appointments = appointments.OrderByDescending(s => s.DoctorRefNavigation.LastName);
                    break;
                case "status":
                    appointments = appointments.OrderBy(s => s.Status);
                    break;
                case "status_desc":
                    appointments = appointments.OrderByDescending(s => s.Status);
                    break;
                default:
                    appointments = appointments.OrderBy(s => s.DateAndTime);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            var nearestAppointment = this.appointmentService.GetAppointmentsByUserId(userId)
                        .Where(a => a.DateAndTime > DateTime.Now)
                        .OrderBy(a => a.DateAndTime)
                        .FirstOrDefault();

            this.ViewBag.NearestAppointment = nearestAppointment;

            return this.View(appointments.ToPagedList(pageNumber, pageSize));
        }
    }
}
