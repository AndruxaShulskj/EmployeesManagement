using EmployeesManagement.Business.Models;

namespace EmployeesManagement.Business.Interfaces;

public interface IAuthenticationService
{
    Task<User> LoginAsync(UserCredential model);
}