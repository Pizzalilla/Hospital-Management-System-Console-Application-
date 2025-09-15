using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services;

// Minimal "auth": loads users from JSON and checks ID+Password.
// No hashing/tokens per assignment scope; easy to swap later if needed.
public static class AuthService
{
    public enum Role { None, Patient, Doctor, Admin }

    public static List<Patient> Patients { get; private set; } = new();
    public static List<Doctor> Doctors { get; private set; } = new();
    public static List<Admin> Admins { get; private set; } = new();

    public static void LoadAll(string dataDir)
    {
        Directory.CreateDirectory(dataDir);

        var adminsPath = Path.Combine(dataDir, "admins.json");
        var doctorsPath = Path.Combine(dataDir, "doctors.json");
        var patientsPath = Path.Combine(dataDir, "patients.json");

        Admins = FileManager.Load<Admin>(adminsPath);
        Doctors = FileManager.Load<Doctor>(doctorsPath);
        Patients = FileManager.Load<Patient>(patientsPath);

        // Optional safety net: seed one of each if files are empty.
        if (Admins.Count == 0)
        {
            Admins.Add(new Admin(1, "Admin", "admin"));
            FileManager.Save(adminsPath, Admins);
        }
        if (Doctors.Count == 0)
        {
            Doctors.Add(new Doctor(10001, "Chen", "pass", "Cardiology", "chen@hospital.org", "0412000000"));
            FileManager.Save(doctorsPath, Doctors);
        }
        if (Patients.Count == 0)
        {
            Patients.Add(new Patient(20001, "Amira", "1234", 29, -1, "amira@example.com", "0413000000"));
            FileManager.Save(patientsPath, Patients);
        }
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
