using HospitalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly HospitalDbContext _context;

    public HomeController(HospitalDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        ViewBag.Patients = await _context.Patients.CountAsync();
        ViewBag.Doctors = await _context.Doctors.CountAsync();
        ViewBag.Appointments = await _context.Appointments.CountAsync();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
