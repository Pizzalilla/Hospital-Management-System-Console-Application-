namespace HospitalManagementSystem.Models;

// Base user type shared by Admin/Doctor/Patient.
// Keeps common fields (Id/Name/Password) in one place to avoid duplication
// and to clearly demonstrate inheritance in the assignment.
public abstract class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";

    // Needed by JSON deserializer to create an empty instance before setting properties.
    protected User() { }

    // Handy when we create patients/admins/doctors 
    protected User(int id, string name, string password)
    {
        Id = id;
        Name = name;
        Password = password;
    }

    // Makes console output readable and consistent for all user types.
    public override string ToString() => $"{GetType().Name}({Id}) - {Name}";
}
