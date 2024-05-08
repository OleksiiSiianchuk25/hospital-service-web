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

namespace WebApplication1.Controllers
{

    [Route("")]
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
        [Route("/add-new-doctor")]
        public IActionResult AddNewDoctor(UserDTO model) // Параметр model буде автоматично заповнено з даних форми
        {
            _userService.RegisterDoctor(model);
            return Redirect("admin");
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
         
            return Redirect("admin");
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
            return View(); ;
        }
    }
}

public class Appoint{

    public string Name { get; set; }
    public string Date { get; set; }

    public Appoint(string name, string date)
    {

        this.Name = name;
        this.Date = date;

    }
}

public class DoctorViewModel
{
    private EF.User user;

    public List<Appoint> Appointments { get; set; }

    public DoctorViewModel(List<Appoint> appointments, EF.User user)
    {
        Appointments = appointments;
        this.user = user;
    }
}
