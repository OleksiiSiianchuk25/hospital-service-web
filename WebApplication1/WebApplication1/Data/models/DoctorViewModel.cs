using X.PagedList;

namespace WebApplication1.Data.models
{
    public class DoctorViewModel
    {
        public EF.User user;
        public List<Appoint> Appointments { get; set; }
        public IPagedList<EF.User> IUsers { get; set; }
        public IPagedList<EF.Appointment> IAppointment{ get; set; }
        public List<EF.User> Doctors { get; set; }
        public List<EF.User> Patients { get; set; }
        public DoctorViewModel(List<Appoint> appointments, EF.User user, IPagedList<EF.User> iUsers)
        {
            Appointments = appointments;
            this.user = user;
            IUsers = iUsers; 
        }

        public DoctorViewModel(List<Appoint> appointments, EF.User user, IPagedList<EF.Appointment> iP)
        {
            Appointments = appointments;
            this.user = user;
            IAppointment= iP;
        }

        public DoctorViewModel(List<Appoint> appointments, EF.User user)
        {
            Appointments = appointments;
            this.user = user;
        }
        public DoctorViewModel(List<EF.User> doctors, List<EF.User> patients)
        {
            Doctors = doctors;
            Patients = patients;
        }
    }
}
