namespace WebApplication1.Data.models
{
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
}
