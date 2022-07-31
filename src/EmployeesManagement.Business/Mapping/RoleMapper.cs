using AutoMapper;
using EmployeesManagement.Business.Models;
using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.Business.Mapping;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<UserRoleEntity, Role>()
            .ForMember(x => x.Name, o => o.MapFrom(x => x.Role.Name));
    }
}