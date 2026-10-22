using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Controllers;

public class PatientsController : Controller
{
    private readonly HospitalDbContext _context;

    public PatientsController(HospitalDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var patients = await _context.Patients
            .OrderByDescending(p => p.PatientId)
            .ToListAsync();
        return View(patients);
    }

    public IActionResult Create() => View(new Patient());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Patient patient)
    {
        if (!ModelState.IsValid) return View(patient);

        patient.CreatedAt = DateTime.Now;
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        return patient == null ? NotFound() : View(patient);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Patient patient)
    {
        if (!ModelState.IsValid) return View(patient);

        var existing = await _context.Patients.FindAsync(patient.PatientId);
        if (existing == null) return NotFound();

        existing.Name = patient.Name;
        existing.Phone = patient.Phone;
        existing.Gender = patient.Gender;
        existing.Age = patient.Age;
        existing.Address = patient.Address;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        return patient == null ? NotFound() : View(patient);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient != null)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
