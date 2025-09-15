using HospitalManagementSystem.Extensions;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Menus;

// Menu for doctor users — shows patients and appointments.
public class DoctorMenu
{
    private readonly Doctor _me;
    public DoctorMenu(Doctor me) { _me = me; }

    public void Show(string dataDir)
    {
        while (true)
        {
            ConsoleExtensions.HeadingBox("Doctor Menu", $"Dr. {_me.Name.ToTitleCase()}");
            Console.WriteLine("1) My details");
            Console.WriteLine("2) My patients");
            Console.WriteLine("3) My appointments");
            Console.WriteLine("4) Check patient by ID");
            Console.WriteLine("5) Appointments with patient by ID");
            Console.WriteLine("0) Logout");
            Console.Write("\nChoose: ");
            var c = Console.ReadLine();

            switch (c)
            {
                case "1": ShowMyDetails(); break;
                case "2": ShowMyPatients(); break;
                case "3": ShowMyAppointments(dataDir); break;
                case "4": CheckPatientById(); break;
                case "5": AppointmentsWithPatientById(dataDir); break;
                case "0": return;
                default: Console.WriteLine("Invalid option."); ConsoleExtensions.Pause(); break;
            }
        }
    }

    private void ShowMyDetails()
    {
        ConsoleExtensions.HeadingBox("Doctor Details", $"Dr. {_me.Name} — {_me.Specialty}");
        Console.WriteLine($"ID: {_me.Id}\nEmail: {_me.Email}\nPhone: {_me.Phone}");
        ConsoleExtensions.Pause();
    }

    private void ShowMyPatients()
    {
        var mine = AuthService.Patients.Where(p => p.DoctorId == _me.Id).ToList();
        ConsoleExtensions.HeadingBox("My Patients", $"Dr. {_me.Name}");
        ConsoleExtensions.PrintTable(
            "", "",                                  // ← no extra title/desc
            new[] { "Patient", "Email", "Phone", "Age" },
            mine.Select(p => new[] { $"{p.Name} ({p.Id})", p.Email, p.Phone, p.Age.ToString() })
        );
        ConsoleExtensions.Pause();
    }

    private void ShowMyAppointments(string dataDir)
    {
        var appts = FileManager.Load<Appointment>(Path.Combine(dataDir, "appointments.json"))
                               .Where(a => a.DoctorId == _me.Id).ToList();

        ConsoleExtensions.HeadingBox("My Appointments", $"Dr. {_me.Name}");
        ConsoleExtensions.PrintTable(
            "", "",                                  // ← no extra title/desc
            new[] { "Appt Id", "Patient", "Notes" },
            appts.Select(a =>
            {
                var p = AuthService.Patients.FirstOrDefault(x => x.Id == a.PatientId);
                var patientName = p is null ? a.PatientId.ToString() : $"{p.Name} ({p.Id})";
                return new[] { a.Id.ToString(), patientName, a.Notes };
            })
        );
        ConsoleExtensions.Pause();
    }

    private void CheckPatientById()
    {
        ConsoleExtensions.HeadingBox("Check Patient", $"Dr. {_me.Name}");
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

    private void AppointmentsWithPatientById(string dataDir)
    {
        ConsoleExtensions.HeadingBox("Appointments with Patient", $"Dr. {_me.Name}");
        while (true)
        {
            var pid = ConsoleExtensions.PromptIntOrBack("Enter Patient ID");
            if (pid is null) return;
            if (pid == int.MinValue) { Console.WriteLine("Invalid ID. Try again."); continue; }

            var appts = FileManager.Load<Appointment>(Path.Combine(dataDir, "appointments.json"))
                                   .Where(a => a.DoctorId == _me.Id && a.PatientId == pid)
                                   .ToList();

            ConsoleExtensions.PrintTable(
                "", "",                              // ← no extra title/desc
                new[] { "Appt Id", "Notes" },
                appts.Select(a => new[] { a.Id.ToString(), a.Notes })
            );
            ConsoleExtensions.Pause();
            break;
        }
    }
}
