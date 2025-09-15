using HospitalApp.Services;
using HospitalApp.Models;
using HospitalApp.Extensions; // we’ll add ConsoleExtensions next step

class Program
{
    static void Main()
    {
        var dataDir = "Data";
        AuthService.LoadAll(dataDir);

        // Sanity print — shows counts so you know files loaded.
        //check this
        Console.WriteLine($"Loaded: {AuthService.Admins.Count} admin(s), " +
                          $"{AuthService.Doctors.Count} doctor(s), " +
                          $"{AuthService.Patients.Count} patient(s).");
        Console.WriteLine("Try logging in (use Admin/admin, Chen/pass, or Amira/1234).");

        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;

        Console.Write("Password: ");
        var pw = Console.ReadLine(); // masked input comes next step

        var (role, user) = AuthService.TryLogin(id, pw ?? "");
        Console.WriteLine(role == AuthService.Role.None
            ? "Invalid credentials"
            : $"Logged in as {role}: {user}");

        Console.WriteLine("\nSmoke test done. Menus coming next.");
    }
}
