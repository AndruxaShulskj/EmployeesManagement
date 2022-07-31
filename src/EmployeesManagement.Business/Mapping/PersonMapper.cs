using AutoMapper;
using EmployeesManagement.Business.Models;
using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.Business.Mapping;

public class PersonMapper : Profile
{
    public PersonMapper()
    {
        CreateMap<PersonEntity, Person>();
        CreateMap<Person, PersonEntity>();
    }
}