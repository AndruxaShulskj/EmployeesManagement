using System.Reflection;
using EmployeesManagement.Common.Interfaces;
using EmployeesManagement.DataAccess;
using EmployeesManagement.DataAccess.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesManagement.Business.IoC;

public static class DatabaseMigration
{
    public static void AddDatabaseMigration(this IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetService<ApplicationDbContext>();
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
            
        var encryptor = serviceProvider.GetService<IEncryptor>();
        if (encryptor == null)
        {
            throw new ArgumentNullException(nameof(encryptor));
        }
            
        context.Database.Migrate();
            
        var path = Directory.GetParent(Assembly.GetEntryAssembly()?.Location)?.FullName;
            
        DbSeeder.Execute(context, path, encryptor);
    }
}