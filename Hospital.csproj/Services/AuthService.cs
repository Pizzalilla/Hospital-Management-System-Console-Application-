using HospitalApp.Models;

namespace HospitalApp.Services;

/// <summary>
/// Minimal “auth”: load users from JSON and check ID+Password.
/// No tokens, no hashing — matches assignment needs only.
/// </summary>
public static class AuthService
{
    public enum Role { None, Patient, Doctor, Admin }

    public static List<Patient> Patients { get; private set; } = new();
    public static List<Doctor>  Doctors  { get; private set; } = new();
    public static List<Admin>   Admins   { get; private set; } = new();

    public static void LoadAll(string dataDir)
    {
        Patients = FileManager.Load<Patient>($"{dataDir}/patients.json");
        Doctors  = FileManager.Load<Doctor>($"{dataDir}/doctors.json");
        Admins   = FileManager.Load<Admin>($"{dataDir}/admins.json");
    }

    public static (Role role, object? user) TryLogin(int id, string password)
    {
        var p = Patients.FirstOrDefault(x => x.Id == id && x.Password == password);
        if (p != null) return (Role.Patient, p);

        var d = Doctors.FirstOrDefault(x => x.Id == id && x.Password == password);
        if (d != null) return (Role.Doctor, d);

        var a = Admins.FirstOrDefault(x => x.Id == id && x.Password == password);
        if (a != null) return (Role.Admin, a);

        return (Role.None, null);
    }
}
