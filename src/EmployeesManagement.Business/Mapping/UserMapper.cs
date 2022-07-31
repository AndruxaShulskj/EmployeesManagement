using AutoMapper;
using EmployeesManagement.Business.Models;
using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.Business.Mapping;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, User>();
    }
}