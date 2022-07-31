namespace EmployeesManagement.UI.Interfaces;

public interface ICheckUserPermissionVerifier
{
    bool CanAdd(UserModel model);
    bool CanEdit(UserModel model);
    bool CanDelete(UserModel model);
}