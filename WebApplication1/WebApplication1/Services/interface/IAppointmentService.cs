using EF.DTO.Appointment;

namespace EF.service
{
    public interface IAppointmentService
    {
        Appointment FindById(long id);

        void AddNew(AppointmentDTO appointment);

        void Update(AppointmentDTO AppointmentDTO);

        void ArchiveById(long id);

        void DeleteById(long appointment);

        List<Appointment> GetAppointments();

        List<Appointment> GetArchiveAppointmentsByUserId(long id);

        List<Appointment> GetActiveAppointmentsByUserId(long id);

        List<Appointment> GetAppointmentsByUserId(long id);

        List<DateTime> GetFreeHoursByDoctorId(long doctorId, DateTime currentDateTime);

        long GetNumberOfAppointments();
    }
}
