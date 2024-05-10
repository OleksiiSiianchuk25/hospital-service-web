using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EF.service.@interface;
using EF.service;
using EF.DTO;
using EF.DTO.User;
using System.Security.Cryptography.X509Certificates;
using EF;
using NLog;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using WebApplication1.Data.models;
using WebApplication1.Data.DTO.User;
using EF.DTO.Appointment;
using WebApplication1.Data.DTO.Appointment;
using Microsoft.VisualBasic;
using System.Data;

namespace WebApplication1.Controllers
{

    [Route("api")]
    public class AdminController : Controller
    {
        private IUserService _userService;
        private IAppointmentService _appointmentService;


        public AdminController(IUserService userService, IAppointmentService appointmentService)
        {
            _userService = userService;
            _appointmentService = appointmentService;
        }
        // Цей атрибут вказує, що метод відповідає на GET-запити

        [HttpPost] // Цей атрибут вказує, що метод відповідає на POST-запити
        [Route("/add-new-appointment")]
        public IActionResult AddNewAppointment(AppointmentRegisterDTO model) // Параметр model буде автоматично заповнено з даних форми
        {
            
            AppointmentDTO appointmentDTO = new AppointmentDTO();
            appointmentDTO.DoctorRef = model.DoctorRef;
            appointmentDTO.PatientRef = model.PatientRef;
            DateTime date = DateTime.Parse(model.Date);
            DateTime time = DateTime.Parse(model.Time);

            // Встановлюємо дату для часу, якщо вона не задана
            if (time.Date == DateTime.MinValue.Date)
            {
                time = date.Date + time.TimeOfDay;
            }

            // Отримуємо повний об'єкт DateTime
            DateTime dateTime = date.Date + time.TimeOfDay;

            appointmentDTO.DateAndTime = dateTime;

            _appointmentService.AddNew(appointmentDTO);
            return Redirect("/api/appointments");
        }


        [HttpPost] // Цей атрибут вказує, що метод відповідає на POST-запити
        [Route("/add-new-appointment-patient")]
        public IActionResult AddNewAppointmentPatient(AppointmentRegisterDTO model) // Параметр model буде автоматично заповнено з даних форми
        {

            AppointmentDTO appointmentDTO = new AppointmentDTO();
            appointmentDTO.DoctorRef = model.DoctorRef;
            appointmentDTO.PatientRef = model.PatientRef;
            DateTime date = DateTime.Parse(model.Date);
            DateTime time = DateTime.Parse(model.Time);

            // Встановлюємо дату для часу, якщо вона не задана
            if (time.Date == DateTime.MinValue.Date)
            {
                time = date.Date + time.TimeOfDay;
            }

            // Отримуємо повний об'єкт DateTime
            DateTime dateTime = date.Date + time.TimeOfDay;

            appointmentDTO.DateAndTime = dateTime;

            _appointmentService.AddNew(appointmentDTO);
            return Redirect("/api/patient/19");
        }


        [HttpPost] // Цей атрибут вказує, що метод відповідає на POST-запити
        [Route("/save-edit-appointment")]
        public IActionResult SaveEditAppointment(AppointmentRegisterDTO model) // Параметр model буде автоматично заповнено з даних форми
        {

            AppointmentDTO appointmentDTO = new AppointmentDTO();
            appointmentDTO.DoctorRef = model.DoctorRef;
            appointmentDTO.PatientRef = model.PatientRef;
            DateTime date = DateTime.Parse(model.Date);
            DateTime time = DateTime.Parse(model.Time);

            // Встановлюємо дату для часу, якщо вона не задана
            if (time.Date == DateTime.MinValue.Date)
            {
                time = date.Date + time.TimeOfDay;
            }

            // Отримуємо повний об'єкт DateTime
            DateTime dateTime = date.Date + time.TimeOfDay;

            appointmentDTO.DateAndTime = dateTime;

            _appointmentService.AddNew(appointmentDTO);
            return Redirect("/api/appointments");
        }

        [HttpGet]
        [Route("/edit-appointment/{id}")]
        public IActionResult EditAppointment(int id) // Параметр model буде автоматично заповнено з даних форми
        {
            EF.Appointment model = _appointmentService.FindById(id);
            return Json(model);

        }


        [HttpPost] // Цей атрибут вказує, що метод відповідає на POST-запити
        [Route("/add-new-doctor")]
        public IActionResult AddNewDoctor(UserDTO model) // Параметр model буде автоматично заповнено з даних форми
        {
            
            _userService.RegisterDoctor(model);
            return Redirect("/api/doctors");
        }

        [HttpPost] // Цей атрибут вказує, що метод відповідає на POST-запити
        [Route("/save-edit-doctor")]
        public IActionResult SaveEditDoctor(UpdateUserDTO model) // Параметр model буде автоматично заповнено з даних форми
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            else
            {
                _userService.EditUser(model);
            }

            return Redirect("api/doctors");
        }

        [HttpGet]
        [Route("/edit-doctor/{id}")]
        public IActionResult EditDoctor(int id) // Параметр model буде автоматично заповнено з даних форми
        {
            EF.User model = _userService.FindById(id);
            return Json(model);

        }

        [HttpGet]
        [Route("/history-doctor/{id}")]
        public IActionResult GetDoctorHistory(int id)
        {
            var docApp = _appointmentService.GetArchiveAppointmentsByUserId(id);
            var historyData = new List<Appoint>(); // Передбачаючи, що тип Appoint відповідає типу записів історії

            foreach (var appointment in docApp)
            {
                var patient = _userService.FindById(appointment.PatientRef);
                var data = appointment.DateAndTime;
                historyData.Add(new Appoint(patient.FirstName + " " + patient.LastName, data.ToString("dd/MM/yyyy HH:mm")));
            }

            var model = new DoctorViewModel(historyData, _userService.FindById(id));

            return Json(model);
        }

        [HttpGet]
        [Route("admin")]
        public IActionResult AdminPage()
        {
            return View();
        }

        [HttpPost] // Цей атрибут вказує, що метод відповідає на POST-запити
        [Route("/add-new-patient")]
        public IActionResult AddNewPatient(UserRegistrationDTO model) // Параметр model буде автоматично заповнено з даних форми
        {
            _userService.RegisterPatient(model);
            return Redirect("api/patients");
        }

        [HttpPost] // Цей атрибут вказує, що метод відповідає на POST-запити
        [Route("/save-edit-patient")]
        public IActionResult SaveEditPatient(UpdateUserDTO model) // Параметр model буде автоматично заповнено з даних форми
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            else
            {
                _userService.EditUser(model);
            }

            return Redirect("/api/patients");
        }

        [HttpGet]
        [Route("/edit-patient/{id}")]
        public IActionResult EditPatient(int id) // Параметр model буде автоматично заповнено з даних форми
        {
            EF.User model = _userService.FindById(id);
            return Json(model);

        }

        [HttpGet]
        [Route("/history-patient/{id}")]
        public IActionResult GetPatientHistory(int id)
        {
            var docApp = _appointmentService.GetArchiveAppointmentsByUserId(id);
            var historyData = new List<Appoint>(); // Передбачаючи, що тип Appoint відповідає типу записів історії

            foreach (var appointment in docApp)
            {
                var doctor = _userService.FindById(appointment.DoctorRef);
                var data = appointment.DateAndTime;
                historyData.Add(new Appoint(doctor.FirstName + " " + doctor.LastName + " " + doctor.Type, data.ToString("dd/MM/yyyy HH:mm")));
            }

            var model = new DoctorViewModel(historyData, _userService.FindById(id));

            return Json(model);
        }

        [HttpGet]
        [Route("/get-users")]
        public IActionResult GetUsers()
        {

            var model = new DoctorViewModel(_userService.GetDoctors(), _userService.GetPatients());

            return Json(model);
        }

        [HttpGet]
        [Route("/get-dates")]
        public IActionResult GetDates()
        {

            List<DateTime> dates = new List<DateTime>();
            DateTime currentDate = DateTime.Now;
            if (currentDate.Hour > 17)
            {
                currentDate = currentDate.AddDays(1);
            }
            for (int i = 0; i < 7; i++)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    dates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(1);
            }
            return Json(dates);

        }

        [HttpGet]
        [Route("/get-free-hours")]
        public IActionResult GetHours()
        {
            string selectedDoctor = HttpContext.Request.Query["doctor"];
            string selectedDate = HttpContext.Request.Query["date"];
            string doctor = HttpContext.Request.Query["doctor"];
            string date = HttpContext.Request.Query["date"];
            Console.WriteLine("OBEMAAA" + doctor + " " + date);
            DateTime date2 = DateTime.Parse(date.Substring(0, 23));
            return Json(_appointmentService.GetFreeHoursByDoctorId(long.Parse(doctor), date2));
        }



        [HttpGet]
        [Route("patient")]
        public IActionResult PatientPage()
        {
            return View();
        }
    }
}

