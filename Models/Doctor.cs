using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models;

public class Doctor
{
    public int DoctorId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    [Required, StringLength(100)]
    public string Specialization { get; set; } = "";

    [StringLength(20)]
    public string Phone { get; set; } = "";
}
