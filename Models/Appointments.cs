namespace HospitalManagementSystem.Models;

// Simple link between one Doctor and one Patient with a short note.
// Kept small because the spec doesn't ask for times/dates; easy to extend later if needed.
public class Appointment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string Notes { get; set; } = "";

    public Appointment() { }

    // Parameterized constructor makes it easy to create new appointments from menus.
    public Appointment(int id, int doctorId, int patientId, string notes)
    {
        Id = id;
        DoctorId = doctorId;
        PatientId = patientId;
        Notes = notes;
    }

    public override string ToString() =>
        $"Appt({Id}) D:{DoctorId} ↔ P:{PatientId} - {Notes}";
}
