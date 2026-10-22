using HospitalManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found. Configure it in appsettings.json.");

builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddSingleton<JsonDataService>();

var app = builder.Build();

// Create the database/tables if they do not exist and import the bundled JSON
// seed data only when the MySQL tables are empty.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<HospitalDbContext>();
        var environment = services.GetRequiredService<IWebHostEnvironment>();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context, environment);
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex,
            "Could not initialize the MySQL database. Check that MySQL Server is running and that the DefaultConnection settings in appsettings.json are correct.");
        throw;
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
