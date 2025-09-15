namespace HospitalManagementSystem.Models;

// Admin is a specialized User.
// No extra fields for the assignment, but kept as a separate type for clarity and role menus.
public class Admin : User
{
    public Admin() { }
    public Admin(int id, string name, string password) : base(id, name, password) { }

    public override string ToString() => $"Admin({Id}) {Name}";
}
