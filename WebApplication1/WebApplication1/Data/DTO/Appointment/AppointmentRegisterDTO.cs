using EF;
using Microsoft.VisualBasic;

namespace WebApplication1.Data.DTO.Appointment
{
    public class AppointmentRegisterDTO
    {
        public long DoctorRef { get; set; }
        public long PatientRef { get; set; }
        public String Date { get; set; }
        public String Time {  get; set; }
        

        public String print()
        {
            return (DoctorRef + " " + PatientRef + " " + Date + " " + Time);
        }

        public AppointmentRegisterDTO(long doctorRef, long patientRef, String date, String time)
        {
            this.DoctorRef = doctorRef;
            this.PatientRef = patientRef;
            this.Date = date;
            this.Time = time;
        }

        public AppointmentRegisterDTO()
        {
        }
    }
}
