using EmployeesManagement.Business.Interfaces;
using EmployeesManagement.DataAccess;
using EmployeesManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Business.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public PersonRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public Task<List<PersonEntity>> GetPersonsAsync(string surname)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var query = context.Persons
            .AsNoTracking()
            .Where(x => !x.Deleted && !x.Service);

        if (!string.IsNullOrEmpty(surname))
        {
            query = query.Where(x => x.Surname.StartsWith(surname));
        }
        
        return query
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    public Task<bool> CheckExistingPersonAsync(PersonEntity person)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.Persons
            .AsNoTracking()
            .AnyAsync(x => 
                x.Name == person.Name && 
                x.Surname == person.Surname &&
                x.SecondName == person.SecondName);
    }

    public async Task UpdatePersonAsync(PersonEntity person)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        context.Set<PersonEntity>().Update(person);

        await context.SaveChangesAsync();
    }

    public async Task AddPersonAsync(PersonEntity person)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        
        await context.AddAsync(person);

        await context.SaveChangesAsync();
    }
}