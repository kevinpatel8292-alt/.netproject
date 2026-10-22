using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models;

public class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    [DataType(DataType.Date)]
    public DateTime AppointmentDate { get; set; } = DateTime.Today;

    [StringLength(20)]
    public string Status { get; set; } = "Scheduled";

    [StringLength(300)]
    public string Notes { get; set; } = "";
}
