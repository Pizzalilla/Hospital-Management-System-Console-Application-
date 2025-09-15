namespace HospitalManagementSystem.Models;

// Patient extends User and tracks Age, contact info, and assigned Doctor.
// DoctorId == -1 means "not registered" which we hide in the UI per your requirement.
public class Patient : User
{
    public int Age { get; set; }
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public int DoctorId { get; set; } = -1;

    public Patient() { }

    public Patient(int id, string name, string password, int age, int doctorId = -1, string email = "", string phone = "")
        : base(id, name, password)
    {
        Age = age;
        DoctorId = doctorId;
        Email = email;
        Phone = phone;
    }

    public override string ToString()
        => DoctorId == -1
           ? $"Patient({Id}) {Name}, {Age} yrs — Email:{Email} Phone:{Phone}"
           : $"Patient({Id}) {Name}, {Age} yrs, DoctorId={DoctorId} — Email:{Email} Phone:{Phone}";
}
