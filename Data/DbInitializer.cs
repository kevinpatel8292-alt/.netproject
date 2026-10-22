using System.Text.Json;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data;

public static class DbInitializer
{
    public static void Initialize(HospitalDbContext context, IWebHostEnvironment environment)
    {
        // Program creates the database and tables before seed data is checked.

        // If data already exists in MySQL, keep existing records
        if (context.Patients.Any() || context.Doctors.Any() || context.Appointments.Any())
        {
            return;
        }

        // Migrate records from hospitalmanagement.json if available
        var jsonFilePath = Path.Combine(environment.ContentRootPath, "Data", "hospitalmanagement.json");
        if (!File.Exists(jsonFilePath)) return;

        try
        {
            // Create a safe backup before importing
            var backupPath = Path.Combine(environment.ContentRootPath, "Data", "hospitalmanagement.json.backup");
            if (!File.Exists(backupPath))
            {
                File.Copy(jsonFilePath, backupPath, overwrite: false);
            }

            var jsonContent = File.ReadAllText(jsonFilePath);
            if (string.IsNullOrWhiteSpace(jsonContent)) return;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<HospitalData>(jsonContent, options);
            if (data == null) return;

            if (data.Doctors.Count > 0)
            {
                foreach (var doctor in data.Doctors)
                {
                    context.Doctors.Add(new Doctor
                    {
                        Name = doctor.Name,
                        Specialization = doctor.Specialization,
                        Phone = doctor.Phone
                    });
                }
                context.SaveChanges();
            }

            if (data.Patients.Count > 0)
            {
                foreach (var patient in data.Patients)
                {
                    context.Patients.Add(new Patient
                    {
                        Name = patient.Name,
                        Phone = patient.Phone,
                        Gender = patient.Gender,
                        Age = patient.Age,
                        Address = patient.Address,
                        CreatedAt = patient.CreatedAt
                    });
                }
                context.SaveChanges();
            }

            if (data.Appointments.Count > 0)
            {
                var doctorsList = context.Doctors.ToList();
                var patientsList = context.Patients.ToList();

                foreach (var appointment in data.Appointments)
                {
                    var matchedDoctor = doctorsList.FirstOrDefault(d => d.DoctorId == appointment.DoctorId) ?? doctorsList.FirstOrDefault();
                    var matchedPatient = patientsList.FirstOrDefault(p => p.PatientId == appointment.PatientId) ?? patientsList.FirstOrDefault();

                    if (matchedDoctor != null && matchedPatient != null)
                    {
                        context.Appointments.Add(new Appointment
                        {
                            PatientId = matchedPatient.PatientId,
                            DoctorId = matchedDoctor.DoctorId,
                            AppointmentDate = appointment.AppointmentDate,
                            Status = string.IsNullOrWhiteSpace(appointment.Status) ? "Scheduled" : appointment.Status,
                            Notes = appointment.Notes ?? ""
                        });
                    }
                }
                context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DbInitializer] Notice: JSON data migration skipped: {ex.Message}");
        }
    }
}
