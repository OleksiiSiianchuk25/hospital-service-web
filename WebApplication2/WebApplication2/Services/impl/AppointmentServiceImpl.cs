using EF.context;
using EF.DTO.Appointment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.service.impl
{
    public class AppointmentServiceImpl : IAppointmentService
    {
        private readonly NeondbContext context;
        private readonly IUserService userService;

        public AppointmentServiceImpl(NeondbContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public Appointment FindById(long id)
        {
            Appointment appointment = context.Appointments
        .Include(app => app.DoctorRefNavigation)
        .Include(app => app.PatientRefNavigation)
        .FirstOrDefault(app => app.AppointmentId == id);
            if (appointment == null)
            {
                throw new ApplicationException("Appointment with id: " + id + " does not exist!");
            }
            return appointment;
        }
        public void AddNew(AppointmentDTO appointment)
        {
            Appointment newAppointment = new Appointment();
            newAppointment.Status = "активний";
            newAppointment.DoctorRefNavigation = userService.FindById(appointment.DoctorRef);
            newAppointment.PatientRefNavigation = userService.FindById(appointment.PatientRef);
            newAppointment.Message = appointment.Message;
            newAppointment.DateAndTime = appointment.DateAndTime;
            context.Appointments.Add(newAppointment);
            context.SaveChanges();
        }

        public void DeleteById(long id)
        {
            Appointment appointmentToDelete = FindById(id);
            context.Appointments.Remove(appointmentToDelete);
            context.SaveChanges();
        }
        public void ArchiveById(long id)
        {
            Appointment appointmentToArchive = FindById(id);
            appointmentToArchive.Status = "архівований";
            context.SaveChanges();
        }

        public void Update(AppointmentDTO appointmentDTO)
        {
            Appointment appointment = FindById(appointmentDTO.AppointmentId);
            appointment.DoctorRefNavigation = userService.FindById(appointmentDTO.DoctorRef);
            appointment.PatientRefNavigation = userService.FindById(appointmentDTO.PatientRef);
            appointment.Message = appointmentDTO.Message;
            appointment.DateAndTime = appointmentDTO.DateAndTime;
            context.SaveChanges(true);
           
        }
        public List<Appointment> GetAppointments()
        {
            return context.Appointments
                .Include(appointment => appointment.DoctorRefNavigation)
                .Include(appointment => appointment.PatientRefNavigation)
                .ToList();
        }

        public List<Appointment> GetArchiveAppointmentsByUserId(long id)
        {
            return context.Appointments
                .Include(appointment => appointment.DoctorRefNavigation)
                .Include(appointment => appointment.PatientRefNavigation)
                .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id) && appointment.Status == "архівований")
                .ToList();
        }

        public List<Appointment> GetActiveAppointmentsByUserId(long id)
        {
            return context.Appointments
                .Include(appointment => appointment.DoctorRefNavigation)
                .Include(appointment => appointment.PatientRefNavigation)
                .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id) && appointment.Status == "активний")
                .ToList();
        }

        public List<Appointment> GetAppointmentsByUserId(long id)
        {
            return context.Appointments
                .Include(appointment => appointment.DoctorRefNavigation)
                .Include(appointment => appointment.PatientRefNavigation)
                .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id))
                .ToList();
        }

       
        public List<DateTime> GetFreeHoursByDoctorId(long doctorId, DateTime currentDateTime)
        {
            DateTime start;
            if (currentDateTime.Day == DateTime.Now.Day && currentDateTime.Hour >= 9)
            {
                start = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour + 1, 0, 0);
            }
            else
            {
                start = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 9, 0, 0);
            }
            
            DateTime finish = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 17,0,0);

            List<DateTime> freeDates = new List<DateTime>();

            for (DateTime currentHour = start; currentHour <= finish; currentHour = currentHour.AddHours(1))
            {
                
                if (!IsHourOccupied(doctorId, currentHour))
                {
                    freeDates.Add(currentHour);
                }
            }

            return freeDates;

        }
        private bool IsHourOccupied(long doctorId, DateTime hour)
        {
            List<Appointment> appointments = GetActiveAppointmentsByUserId(doctorId);
            return appointments.Any(appointment => appointment.DoctorRef == doctorId && appointment.DateAndTime == hour);
        }

        public long GetNumberOfAppointments()
        {
            return context.Appointments.Count();
        }
    }
}
