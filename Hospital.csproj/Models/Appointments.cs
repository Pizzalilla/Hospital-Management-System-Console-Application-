namespace HospitalApp.Models;

public class Appointment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string Notes { get; set; } = "";

    public Appointment() { } // For JSON

    public Appointment(int id, int doctorId, int patientId, string notes)
    {
        Id = id;
        DoctorId = doctorId;
        PatientId = patientId;
        Notes = notes;
    }

    //make this one more organized/look better. With Doctor, Appointment etc.
    public override string ToString() =>
        $"Appt({Id}) D:{DoctorId} ↔ P:{PatientId} - {Notes}";
}
