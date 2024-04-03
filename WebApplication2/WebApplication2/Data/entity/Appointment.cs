using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EF;

public partial class Appointment

{
    public long AppointmentId { get; set; }

    public DateTime DateAndTime { get; set; }

    public string? Message { get; set; }
    public string Status { get; set; }

    public long PatientRef { get; set; }

    public long DoctorRef { get; set; }

    public virtual User DoctorRefNavigation { get; set; } = null!;

    public virtual User PatientRefNavigation { get; set; } = null!;

    public Appointment(DateTime dateAndTime, string? message, string status, long patientRef, long doctorRef)
    {
        DateAndTime = dateAndTime;
        Message = message;
        Status = status;
        PatientRef = patientRef;
        DoctorRef = doctorRef;
    }

    public Appointment()
    {
    }
}
