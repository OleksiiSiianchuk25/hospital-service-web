using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public AppointmentDTO() { }

        public AppointmentDTO(long appointmentId, DateTime dateAndTime, string? message, long patientRef, long doctorRef)
        {
            AppointmentId = appointmentId;
            DateAndTime = dateAndTime;
            Message = message;
            PatientRef = patientRef;
            DoctorRef = doctorRef;
        }

        public AppointmentDTO(DateTime dateAndTime, string? message, long patientRef, long doctorRef)
        {
            DateAndTime = dateAndTime;
            Message = message;
            PatientRef = patientRef;
            DoctorRef = doctorRef;
        }
    }


}
