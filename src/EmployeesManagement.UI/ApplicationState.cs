namespace EmployeesManagement.UI;

public class ApplicationState
{
    public Func<UserModel, Task> UserSuccessfulAuthorizedFn { get; set; }
}