using EmployeesManagement.Business.Models;

namespace EmployeesManagement.Business.Interfaces;

public interface IPersonService
{
    Task<IEnumerable<Person>> GetPersonsAsync(string surname);
    Task AddPersonAsync(Person model);
    Task UpdatePersonAsync(Person model);
    Task DeletePersonAsync(Person model);
}