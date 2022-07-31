using EmployeesManagement.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.DataAccess.Seeds;

public static class DbSeeder
{
    public static void Execute(DbContext applicationContext, string path, IEncryptor encryptor)
    {
        using var transaction = applicationContext.Database.BeginTransaction();

        var persons = DbSeedGetter.GetPersons(path).ToList();
        CheckAndFullIsEmpty(applicationContext, persons);

        var roles = DbSeedGetter.GetRoles(path).ToList();
        CheckAndFullIsEmpty(applicationContext, roles);

        var users = DbSeedGetter.GetUsers(path).ToList();
        CheckAndFullIsEmpty(applicationContext,
            users.Select(
                user =>
                {
                    user.Hash = encryptor.GetHash($"{user.Login}2022+", user.Salt);

                    return user;
                }));

        var usersRoles = DbSeedGetter.GetUserRoles(path).ToList();
        CheckAndFullIsEmpty(applicationContext, usersRoles);

        applicationContext.SaveChanges();
        transaction.Commit();
    }

    private static void CheckAndFullIsEmpty<T>(DbContext applicationContext, IEnumerable<T> items)
        where T : class
    {
        if (!applicationContext.Set<T>().Any())
        {
            applicationContext.Set<T>().AddRangeAsync(items);
        }
    }
}