using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models;

public class Patient
{
    public int PatientId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    [Required, StringLength(20)]
    public string Phone { get; set; } = "";

    [StringLength(10)]
    public string Gender { get; set; } = "";

    public int Age { get; set; }

    [StringLength(200)]
    public string Address { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
