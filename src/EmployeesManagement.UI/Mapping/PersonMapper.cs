using AutoMapper;
using EmployeesManagement.Business.Models;

namespace EmployeesManagement.UI.Mapping;

public class PersonMapper : Profile
{
    public PersonMapper()
    {
        CreateMap<Person, PersonModel>();
        CreateMap<PersonModel, Person>();
    }
}