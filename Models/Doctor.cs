namespace HospitalManagementSystem.Models;

// Doctor extends User and adds medical-specific details.
// Email/Phone support better contact info in lists and details screens.
// Specialty is required by your new "add doctor" and doctor listings.
public class Doctor : User
{
    public string Specialty { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";

    public Doctor() { }

    public Doctor(int id, string name, string password, string specialty, string email = "", string phone = "")
        : base(id, name, password)
    {
        Specialty = specialty;
        Email = email;
        Phone = phone;
    }

    public override string ToString() =>
        $"Doctor({Id}) Dr. {Name} [{Specialty}] — Email:{Email} Phone:{Phone}";
}
