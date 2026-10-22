# Hospital Management System (ASP.NET Core MVC + MySQL)

This project uses ASP.NET Core MVC, Entity Framework Core 10, and Oracle's `MySql.EntityFrameworkCore` provider. It does **not** require XAMPP.

## Requirements

- .NET 10 SDK
- MySQL Server 8.0 or compatible, installed as a Windows service (for example, `MySQL800`)
- MySQL Workbench is optional; it is only a database GUI

## 1. Start MySQL Server

Open Windows Services (`Win + R` → `services.msc`) and make sure your MySQL service is **Running**. The default connection uses port `3306`.

## 2. Configure your password

Open `appsettings.json` and replace `CHANGE_ME` in the `DefaultConnection` string with the password for your local MySQL `root` account. Do not upload a real password to a public GitHub repository.

Example:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=127.0.0.1;port=3306;database=hospital_db_fixed;user=root;password=YOUR_LOCAL_PASSWORD;"
  }
}
```

If you use another host, port, username, or database name, update the connection string accordingly.

## 3. Run the project

Open this folder in VS Code. In Terminal → New Terminal, run:

```powershell
dotnet restore
dotnet build
dotnet run
```

Open the local URL printed in the terminal (the launch profile normally uses `http://localhost:5000`).

On first successful startup, `EnsureCreated()` creates the `hospital_db_fixed` database and the `patients`, `doctors`, and `appointments` tables if they do not already exist. If the bundled `Data/hospitalmanagement.json` contains records and the MySQL tables are empty, the app attempts to import those records. A backup JSON file is retained in `Data/`.

## Database tables

- `patients`
- `doctors`
- `appointments`

The MVC controllers use `HospitalDbContext` and EF Core for CRUD operations.

## Notes

- XAMPP is not required and no XAMPP paths are used by this project.
- This project uses `EnsureCreated()` for a small mini-project setup. `EnsureCreated()` does not apply or update EF Core migrations to an existing schema. If you later change entity models, use a migration-based workflow deliberately.
- `appsettings.json` contains a placeholder password. Set your own password locally before running.


If you previously ran an earlier version and it created only some tables, use the fresh database name `hospital_db_fixed` in `appsettings.json` as shown above. Do not delete your existing database. The `CreatedAt` field is populated by the C# model rather than a MySQL column default.
