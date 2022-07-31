namespace EmployeesManagement.Business.Models;

public class User
{
    public string Login { get; set; }
    public IEnumerable<Role> Roles { get; set; }
    public Person Person { get; set; }
}