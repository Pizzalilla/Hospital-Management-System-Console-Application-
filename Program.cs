using HospitalManagementSystem.Extensions;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Menus;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem;

class Program
{
    static void Main()
    {
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        AuthService.LoadAll(dataDir);

        while (true)
        {
            ConsoleExtensions.HeadingBox("Hospital Management System", "Login");

            Console.Write("ID: ");
            if (!int.TryParse(Console.ReadLine(), out var id)) continue;

            Console.Write("Password: ");
            var pw = ConsoleExtensions.ReadPasswordMasked();

            var (role, user) = AuthService.TryLogin(id, pw);
            if (role == AuthService.Role.None)
            {
                Console.WriteLine("Invalid credentials.");
                ConsoleExtensions.Pause();
                continue;
            }

            switch (role)
            {
                case AuthService.Role.Admin:
                    new AdminMenu((Admin)user!).Show(dataDir);
                    break;
                case AuthService.Role.Doctor:
                    new DoctorMenu((Doctor)user!).Show(dataDir);
                    break;
                case AuthService.Role.Patient:
                    new PatientMenu((Patient)user!).Show(dataDir);
                    break;
            }
        }
    }
}
