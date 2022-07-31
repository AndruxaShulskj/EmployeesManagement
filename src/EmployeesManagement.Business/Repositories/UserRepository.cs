using EmployeesManagement.Business.Interfaces;
using EmployeesManagement.DataAccess;
using EmployeesManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Business.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public UserRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<UserEntity> GetUserByLoginAsync(string login)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.Users
            .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
            .Include(x => x.Person)
            .FirstOrDefaultAsync(x => x.Login == login && !x.Disabled);
    }
}