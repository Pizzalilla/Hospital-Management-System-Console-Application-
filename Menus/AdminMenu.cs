using HospitalManagementSystem.Extensions;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Menus;

// Menu for admin users — lets them view and manage doctors and patients.
public class AdminMenu
{
    private readonly Admin _me;
    public AdminMenu(Admin me) { _me = me; }

    public void Show(string dataDir)
    {
        while (true)
        {
            ConsoleExtensions.HeadingBox("Admin Menu", $"Welcome, {_me.Name.ToTitleCase()}");
            Console.WriteLine("1) List all doctors");
            Console.WriteLine("2) Check doctor details (by ID)");
            Console.WriteLine("3) List all patients");
            Console.WriteLine("4) Check patient details (by ID)");
            Console.WriteLine("5) Add doctor");
            Console.WriteLine("6) Add patient");
            Console.WriteLine("0) Logout");
            Console.Write("\nChoose: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ListAllDoctors(); break;
                case "2": CheckDoctorById(); break;
                case "3": ListAllPatients(); break;
                case "4": CheckPatientById(); break;
                case "5": AddDoctor(dataDir); break;
                case "6": AddPatient(dataDir); break;
                case "0": return; // logout
                default: Console.WriteLine("Invalid option."); ConsoleExtensions.Pause(); break;
            }
        }
    }

    private void ListAllDoctors()
    {
        ConsoleExtensions.HeadingBox("Hospital Management System", "All Doctors");
        ConsoleExtensions.PrintTable(
            "", "",                                  // ? no extra title/desc
            new[] { "ID", "Name", "Specialty", "Email", "Phone" },
            AuthService.Doctors.Select(d => new[] { d.Id.ToString(), $"Dr. {d.Name}", d.Specialty, d.Email, d.Phone })
        );
        ConsoleExtensions.Pause();
    }

    private void ListAllPatients()
    {
        ConsoleExtensions.HeadingBox("Hospital Management System", "All Patients");
        ConsoleExtensions.PrintTable(
            "", "",                                  // ? no extra title/desc
            new[] { "Patient", "Doctor", "Email", "Phone", "Age" },
            AuthService.Patients.Select(p =>
            {
                var doc = p.DoctorId == -1 ? "" : AuthService.Doctors.FirstOrDefault(x => x.Id == p.DoctorId)?.Name ?? "";
                return new[]
                {
                $"{p.Name} ({p.Id})",
                string.IsNullOrWhiteSpace(doc) ? "" : $"Dr. {doc}",
                p.Email,
                p.Phone,
                p.Age.ToString()
                };
            })
        );
        ConsoleExtensions.Pause();
    }


    private void CheckDoctorById()
    {
        ConsoleExtensions.HeadingBox("Hospital Management System", "Check Doctor");
        while (true)
        {
            var id = ConsoleExtensions.PromptIntOrBack("Enter Doctor ID");
            if (id is null) return;
            if (id == int.MinValue) { Console.WriteLine("Invalid ID. Try again."); continue; }

            var d = AuthService.Doctors.FirstOrDefault(x => x.Id == id);
            if (d is null) { Console.WriteLine("Not found. Try again or press B to go back."); continue; }

            ConsoleExtensions.HeadingBox("Doctor Details", $"Dr. {d.Name}");
            Console.WriteLine($"ID: {d.Id}\nSpecialty: {d.Specialty}\nEmail: {d.Email}\nPhone: {d.Phone}");
            ConsoleExtensions.Pause();
            break;
        }
    }

    private void CheckPatientById()
    {
        ConsoleExtensions.HeadingBox("Hospital Management System", "Check Patient");
        while (true)
        {
            var id = ConsoleExtensions.PromptIntOrBack("Enter Patient ID");
            if (id is null) return;
            if (id == int.MinValue) { Console.WriteLine("Invalid ID. Try again."); continue; }

            var p = AuthService.Patients.FirstOrDefault(x => x.Id == id);
            if (p is null) { Console.WriteLine("Not found. Try again or press B to go back."); continue; }

            var doc = p.DoctorId == -1 ? "(not registered)" : $"Dr. {AuthService.Doctors.FirstOrDefault(d => d.Id == p.DoctorId)?.Name}";
            ConsoleExtensions.HeadingBox("Patient Details", p.Name);
            Console.WriteLine($"ID: {p.Id}\nAge: {p.Age}\nDoctor: {doc}\nEmail: {p.Email}\nPhone: {p.Phone}");
            ConsoleExtensions.Pause();
            break;
        }
    }


    private void AddDoctor(string dataDir)
    {
        ConsoleExtensions.HeadingBox("Hospital Management System", "Add Doctor");
        var id = PromptUniqueId("Doctor");
        Console.Write("Name: "); var name = Console.ReadLine() ?? "";
        Console.Write("Password: "); var pw = Console.ReadLine() ?? "";
        Console.Write("Specialty: "); var spec = Console.ReadLine() ?? "";
        Console.Write("Email: "); var email = Console.ReadLine() ?? "";
        Console.Write("Phone: "); var phone = Console.ReadLine() ?? "";

        AuthService.Doctors.Add(new Doctor(id, name, pw, spec, email, phone));
        FileManager.Save(Path.Combine(dataDir, "doctors.json"), AuthService.Doctors);
        Console.WriteLine("Doctor added.");
        ConsoleExtensions.Pause();
    }

    private void AddPatient(string dataDir)
    {
        ConsoleExtensions.HeadingBox("Hospital Management System", "Add Patient");
        var id = PromptUniqueId("Patient");
        Console.Write("Name: "); var name = Console.ReadLine() ?? "";
        Console.Write("Password: "); var pw = Console.ReadLine() ?? "";
        Console.Write("Age: "); var ok = int.TryParse(Console.ReadLine(), out var age);
        Console.Write("Email: "); var email = Console.ReadLine() ?? "";
        Console.Write("Phone: "); var phone = Console.ReadLine() ?? "";
        if (!ok) age = 0;

        AuthService.Patients.Add(new Patient(id, name, pw, age, -1, email, phone));
        FileManager.Save(Path.Combine(dataDir, "patients.json"), AuthService.Patients);
        Console.WriteLine("Patient added.");
        ConsoleExtensions.Pause();
    }

    // Makes sure ID is unique across admins, doctors, and patients.
    private static int PromptUniqueId(string who)
    {
        while (true)
        {
            Console.Write($"{who} ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Invalid."); continue; }
            var exists = AuthService.Admins.Any(x => x.Id == id)
                      || AuthService.Doctors.Any(x => x.Id == id)
                      || AuthService.Patients.Any(x => x.Id == id);
            if (exists) { Console.WriteLine("ID already in use. Try again."); continue; }
            return id;
        }
    }
}
