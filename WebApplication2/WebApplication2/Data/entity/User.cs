using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EF;

public partial class User
{
    public long UserId { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Type { get; set; }

    public long RoleRef { get; set; }

    [JsonIgnore]
    public virtual ICollection<Appointment> AppointmentDoctorRefNavigations { get; set; } = new List<Appointment>();

    [JsonIgnore]
    public virtual ICollection<Appointment> AppointmentPatientRefNavigations { get; set; } = new List<Appointment>();

    public virtual Role RoleRefNavigation { get; set; } = null!;
}
