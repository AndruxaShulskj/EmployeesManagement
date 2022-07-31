using System.Linq;
using System.Reflection;
using EmployeesManagement.Business;
using EmployeesManagement.Common;
using EmployeesManagement.Common.Configurations;
using EmployeesManagement.DataAccess;
using EmployeesManagement.UI.Interfaces;
using EmployeesManagement.UI.Mapping;
using EmployeesManagement.UI.Security;
using EmployeesManagement.UI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeesManagement.UI.IoC;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddSingleton<ApplicationState>();
        
        services.RegisterBusiness();
        services.RegisterCommon();
        
        services.AddSingleton<ICheckUserPermissionVerifier, CheckUserPermissionVerifier>();
        services.AddSingleton<IStyleThemeMapper, StyleThemeMapper>();
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
    
    public static IServiceCollection RegisterViews(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddSingleton<LoginView>();
        services.AddSingleton<MainView>();

        return services;
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();

        return services;
    }

    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        var connectionString = configuration.GetConnectionString(AppConstants.CONNECTION_NAME);
        services.AddDbContextFactory<ApplicationDbContext>(
            options => options.UseSqlite(connectionString,
                x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        return services;
    }
}