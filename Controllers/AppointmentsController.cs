using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Controllers;

public class AppointmentsController : Controller
{
    private readonly HospitalDbContext _context;

    public AppointmentsController(HospitalDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return View(appointments);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Patients = await _context.Patients.OrderBy(p => p.Name).ToListAsync();
        ViewBag.Doctors = await _context.Doctors.OrderBy(d => d.Name).ToListAsync();
        return View(new Appointment());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        if (!await _context.Patients.AnyAsync(p => p.PatientId == appointment.PatientId))
            ModelState.AddModelError(nameof(appointment.PatientId), "Please select a valid patient.");

        if (!await _context.Doctors.AnyAsync(d => d.DoctorId == appointment.DoctorId))
            ModelState.AddModelError(nameof(appointment.DoctorId), "Please select a valid doctor.");

        if (!ModelState.IsValid)
        {
            ViewBag.Patients = await _context.Patients.OrderBy(p => p.Name).ToListAsync();
            ViewBag.Doctors = await _context.Doctors.OrderBy(d => d.Name).ToListAsync();
            return View(appointment);
        }

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);

        if (appointment == null) return NotFound();

        return View(appointment);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
