using HospitalManagementSystem.Extensions;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Menus;

// Menu for patient users — shows their details, doctor, and appointments.
public class PatientMenu
{
    private readonly Patient _me;
    public PatientMenu(Patient me) { _me = me; }

    public void Show(string dataDir)
    {
        while (true)
        {
            ConsoleExtensions.HeadingBox("Patient Menu", _me.Name.ToTitleCase());
            Console.WriteLine("1) My details");
            Console.WriteLine("2) My doctor");
            Console.WriteLine("3) My appointments");
            Console.WriteLine("4) Book appointment");
            Console.WriteLine("0) Logout");
            Console.Write("\nChoose: ");
            var c = Console.ReadLine();

            switch (c)
            {
                case "1": ShowMyDetails(); break;
                case "2": ShowMyDoctor(); break;
                case "3": ShowMyAppointments(dataDir); break;
                case "4": BookAppointment(dataDir); break;
                case "0": return;
                default: Console.WriteLine("Invalid option."); ConsoleExtensions.Pause(); break;
            }
        }
    }

    private void ShowMyDetails()
    {
        ConsoleExtensions.HeadingBox("My Details", _me.Name);
        Console.WriteLine($"ID: {_me.Id}\nName: {_me.Name}\nAge: {_me.Age}\nEmail: {_me.Email}\nPhone: {_me.Phone}");
        if (_me.DoctorId != -1)
        {
            var d = AuthService.Doctors.FirstOrDefault(x => x.Id == _me.DoctorId);
            if (d != null) Console.WriteLine($"Doctor: Dr. {d.Name} ({d.Specialty})");
        }
        ConsoleExtensions.Pause();
    }

    private void ShowMyDoctor()
    {
        if (_me.DoctorId == -1)
        {
            ConsoleExtensions.HeadingBox("My Doctor", "Not registered");
            ConsoleExtensions.Pause();
            return;
        }
        var d = AuthService.Doctors.FirstOrDefault(x => x.Id == _me.DoctorId);
        ConsoleExtensions.HeadingBox("My Doctor", d is null ? "(missing)" : $"Dr. {d.Name} — {d.Specialty}");
        if (d != null) Console.WriteLine($"Email: {d.Email}\nPhone: {d.Phone}");
        ConsoleExtensions.Pause();
    }

    private void ShowMyAppointments(string dataDir)
    {
        var appts = FileManager.Load<Appointment>(Path.Combine(dataDir, "appointments.json"))
                               .Where(a => a.PatientId == _me.Id).ToList();

        ConsoleExtensions.HeadingBox("My Appointments", _me.Name);
        ConsoleExtensions.PrintTable(
            "", "",                                  // ← no extra title/desc
            new[] { "Appt Id", "Doctor", "Notes" },
            appts.Select(a =>
            {
                var d = AuthService.Doctors.FirstOrDefault(x => x.Id == a.DoctorId);
                var doctorName = d is null ? a.DoctorId.ToString() : $"Dr. {d.Name} ({d.Specialty})";
                return new[] { a.Id.ToString(), doctorName, a.Notes };
            })
        );
        ConsoleExtensions.Pause();
    }


    private void BookAppointment(string dataDir)
    {
        if (_me.DoctorId == -1)
        {
            ConsoleExtensions.HeadingBox("Register Doctor", "Choose one");
            foreach (var doctor in AuthService.Doctors) Console.WriteLine(doctor);

            while (true)
            {
                var s = ConsoleExtensions.PromptOrBack("Enter Doctor ID");
                if (s is null) return;
                if (!int.TryParse(s, out var did)) { Console.WriteLine("Invalid ID. Try again."); continue; }

                var doctor = AuthService.Doctors.FirstOrDefault(x => x.Id == did);
                if (doctor is null) { Console.WriteLine("Doctor not found. Try again or press B to go back."); continue; }

                _me.DoctorId = doctor.Id;
                FileManager.Save(Path.Combine(dataDir, "patients.json"), AuthService.Patients);
                Console.WriteLine($"Registered to Dr. {doctor.Name}.");
                ConsoleExtensions.Pause();
                break;
            }
        }

        ConsoleExtensions.HeadingBox("Book Appointment", _me.Name);
        Console.Write("Notes (reason/subject): ");
        var notes = Console.ReadLine() ?? "";

        var apptsPath = Path.Combine(dataDir, "appointments.json");
        var appts = FileManager.Load<Appointment>(apptsPath);
        var nextId = appts.Count == 0 ? 30000 : appts.Max(a => a.Id) + 1;

        var appt = new Appointment(nextId, _me.DoctorId, _me.Id, notes);
        appts.Add(appt);
        FileManager.Save(apptsPath, appts);

        Console.WriteLine($"Appointment booked: {appt}");
        ConsoleExtensions.Pause();
    }

}
