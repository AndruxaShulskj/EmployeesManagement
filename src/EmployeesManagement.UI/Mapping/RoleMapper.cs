using AutoMapper;
using EmployeesManagement.Business.Models;

namespace EmployeesManagement.UI.Mapping;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleModel>();
    }
}