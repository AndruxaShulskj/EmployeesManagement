using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.Business.Interfaces;

public interface IUserRepository
{
    Task<UserEntity> GetUserByLoginAsync(string login);
}