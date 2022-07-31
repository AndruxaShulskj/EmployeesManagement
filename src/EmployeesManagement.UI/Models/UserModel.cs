using System.Collections.Generic;

namespace EmployeesManagement.UI.Models;

public class UserModel
{
    public string Login { get; set; }
    public IEnumerable<RoleModel> Roles { get; set; }
    public PersonModel Person { get; set; }
}