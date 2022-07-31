using System.Reflection;
using EmployeesManagement.Business.Interfaces;
using EmployeesManagement.Business.Repositories;
using EmployeesManagement.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesManagement.Business;

public static class BusinessModule
{
    public static void RegisterBusiness(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPersonService, PersonService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}