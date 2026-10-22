using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Controllers;

public class DoctorsController : Controller
{
    private readonly HospitalDbContext _context;

    public DoctorsController(HospitalDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var doctors = await _context.Doctors
            .OrderBy(d => d.Name)
            .ToListAsync();
        return View(doctors);
    }

    public IActionResult Create() => View(new Doctor());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Doctor doctor)
    {
        if (!ModelState.IsValid) return View(doctor);

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        return doctor == null ? NotFound() : View(doctor);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Doctor doctor)
    {
        if (!ModelState.IsValid) return View(doctor);

        var existing = await _context.Doctors.FindAsync(doctor.DoctorId);
        if (existing == null) return NotFound();

        existing.Name = doctor.Name;
        existing.Specialization = doctor.Specialization;
        existing.Phone = doctor.Phone;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        return doctor == null ? NotFound() : View(doctor);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor != null)
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
