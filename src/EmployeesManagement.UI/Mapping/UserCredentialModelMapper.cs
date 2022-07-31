using AutoMapper;
using EmployeesManagement.Business.Models;
using EmployeesManagement.UI.Models;

namespace EmployeesManagement.UI.Mapping;

public class UserCredentialModelMapper : Profile
{
    public UserCredentialModelMapper()
    {
        CreateMap<UserCredentialModel, UserCredential>();
    }
}