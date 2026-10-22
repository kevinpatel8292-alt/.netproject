using System.Text.Json;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data;

public class HospitalData
{
    public List<Patient> Patients { get; set; } = new();
    public List<Doctor> Doctors { get; set; } = new();
    public List<Appointment> Appointments { get; set; } = new();
}

public class JsonDataService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    private readonly object _lock = new();

    public JsonDataService(IWebHostEnvironment environment)
    {
        var dataFolder = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(dataFolder);
        _filePath = Path.Combine(dataFolder, "hospitalmanagement.json");
    }

    public HospitalData Load()
    {
        lock (_lock)
        {
            if (!File.Exists(_filePath))
            {
                var data = new HospitalData();
                Save(data);
                return data;
            }

            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new HospitalData();

            return JsonSerializer.Deserialize<HospitalData>(json, _options) ?? new HospitalData();
        }
    }

    public void Save(HospitalData data)
    {
        lock (_lock)
        {
            var json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(_filePath, json);
        }
    }

    public int GetNextPatientId(HospitalData data) => data.Patients.Count == 0 ? 1 : data.Patients.Max(x => x.PatientId) + 1;
    public int GetNextDoctorId(HospitalData data) => data.Doctors.Count == 0 ? 1 : data.Doctors.Max(x => x.DoctorId) + 1;
    public int GetNextAppointmentId(HospitalData data) => data.Appointments.Count == 0 ? 1 : data.Appointments.Max(x => x.AppointmentId) + 1;
}
