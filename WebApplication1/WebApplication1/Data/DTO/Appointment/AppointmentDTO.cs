namespace EF.DTO.Appointment
{
    public class AppointmentDTO
    {
        public long AppointmentId { get; set; }

        public DateTime DateAndTime { get; set; }

        public string? Message { get; set; } = null!;

        public string Status { get; set; } = null!;

        public long PatientRef { get; set; }

        public long DoctorRef { get; set; }

        public AppointmentDTO()
        {
        }

        public AppointmentDTO(long appointmentId, DateTime dateAndTime, string? message, long patientRef, long doctorRef)
        {
            this.AppointmentId = appointmentId;
            this.DateAndTime = dateAndTime;
            this.Message = message;
            this.PatientRef = patientRef;
            this.DoctorRef = doctorRef;
        }

        public AppointmentDTO(DateTime dateAndTime, string? message, long patientRef, long doctorRef)
        {
            this.DateAndTime = dateAndTime;
            this.Message = message;
            this.PatientRef = patientRef;
            this.DoctorRef = doctorRef;
        }
        public String print()
        {
            return (DoctorRef + " " + PatientRef + " " + DateAndTime);
        }
    }
}
