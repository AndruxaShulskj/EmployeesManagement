using EmployeesManagement.Common.Interfaces;
using EmployeesManagement.Common.Security;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesManagement.Common;

public static class CommonModule
{
    public static void RegisterCommon(this IServiceCollection services)
    {
        //Security
        services.AddScoped<IEncryptor, Encryptor>();
    }
}