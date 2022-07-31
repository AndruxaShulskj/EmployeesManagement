using AutoMapper;
using EmployeesManagement.Business.Models;
using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.UI.Mapping;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<User, UserModel>();
    }
}