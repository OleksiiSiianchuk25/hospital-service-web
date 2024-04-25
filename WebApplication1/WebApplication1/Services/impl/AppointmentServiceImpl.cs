using EF.context;
using EF.DTO.Appointment;
using Microsoft.EntityFrameworkCore;

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
        public AppointmentServiceImpl(NeondbContext context)
        {
            this.context = context;
            this.userService = new UserServiceImpl(context);
        }
        /// <inheritdoc/>
        public Appointment FindById(long id)
        {
            Appointment appointment = this.context.Appointments
        .Include(app => app.DoctorRefNavigation)
        .Include(app => app.PatientRefNavigation)
        .FirstOrDefault(app => app.AppointmentId == id);
            if (appointment == null)
            {
                throw new ApplicationException("Appointment with id: " + id + " does not exist!");
            }

            return appointment;
        }

        /// <inheritdoc/>
        public void AddNew(AppointmentDTO appointment)
        {
            Appointment newAppointment = new Appointment();
            newAppointment.Status = "активний";
            newAppointment.DoctorRefNavigation = this.userService.FindById(appointment.DoctorRef);
            newAppointment.PatientRefNavigation = this.userService.FindById(appointment.PatientRef);
            newAppointment.Message = appointment.Message;
            newAppointment.DateAndTime = appointment.DateAndTime;
            this.context.Appointments.Add(newAppointment);
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteById(long id)
        {
            Appointment appointmentToDelete = this.FindById(id);
            this.context.Appointments.Remove(appointmentToDelete);
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public void ArchiveById(long id)
        {
            Appointment appointmentToArchive = this.FindById(id);
            appointmentToArchive.Status = "архівований";
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Update(AppointmentDTO appointmentDTO)
        {
            Appointment appointment = this.FindById(appointmentDTO.AppointmentId);
            appointment.DoctorRefNavigation = this.userService.FindById(appointmentDTO.DoctorRef);
            appointment.PatientRefNavigation = this.userService.FindById(appointmentDTO.PatientRef);
            appointment.Message = appointmentDTO.Message;
            appointment.DateAndTime = appointmentDTO.DateAndTime;
            this.context.SaveChanges(true);
        }

        /// <inheritdoc/>
        public List<Appointment> GetAppointments()
        {
            return this.context.Appointments
                .Include(appointment => appointment.DoctorRefNavigation)
                .Include(appointment => appointment.PatientRefNavigation)
                .ToList();
        }

        /// <inheritdoc/>
        public List<Appointment> GetArchiveAppointmentsByUserId(long id)
        {
            return this.context.Appointments
                .Include(appointment => appointment.DoctorRefNavigation)
                .Include(appointment => appointment.PatientRefNavigation)
                .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id) && appointment.Status == "архівований")
                .ToList();
        }

        /// <inheritdoc/>
        public List<Appointment> GetActiveAppointmentsByUserId(long id)
        {
            return this.context.Appointments
                .Include(appointment => appointment.DoctorRefNavigation)
                .Include(appointment => appointment.PatientRefNavigation)
                .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id) && appointment.Status == "активний")
                .ToList();
        }

        /// <inheritdoc/>
        public List<Appointment> GetAppointmentsByUserId(long id)
        {
            return this.context.Appointments
                .Include(appointment => appointment.DoctorRefNavigation)
                .Include(appointment => appointment.PatientRefNavigation)
                .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id))
                .ToList();
        }

        /// <inheritdoc/>
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
            
            DateTime finish = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 17, 0, 0);

            List<DateTime> freeDates = new List<DateTime>();

            for (DateTime currentHour = start; currentHour <= finish; currentHour = currentHour.AddHours(1))
            {
                if (!this.IsHourOccupied(doctorId, currentHour))
                {
                    freeDates.Add(currentHour);
                }
            }

            return freeDates;
        }

        private bool IsHourOccupied(long doctorId, DateTime hour)
        {
            List<Appointment> appointments = this.GetActiveAppointmentsByUserId(doctorId);
            return appointments.Any(appointment => appointment.DoctorRef == doctorId && appointment.DateAndTime == hour);
        }

        /// <inheritdoc/>
        public long GetNumberOfAppointments()
        {
            return this.context.Appointments.Count();
        }
    }
}
