using System.Linq;
using EmployeesManagement.UI.Interfaces;

namespace EmployeesManagement.UI.Security;

public class CheckUserPermissionVerifier : ICheckUserPermissionVerifier
{
    public bool CanAdd(UserModel model)
    {
        return model.Roles.Any(x => x.Name == "admin");
    }
    
    public bool CanEdit(UserModel model)
    {
        return model.Roles.Any(x => x.Name == "admin");
    }

    public bool CanDelete(UserModel model)
    {
        return model.Roles.Any(x => x.Name == "admin");
    }
}