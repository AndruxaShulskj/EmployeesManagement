using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.Business.Interfaces;

public interface IPersonRepository
{
    Task<List<PersonEntity>> GetPersonsAsync(string surname);
    Task<bool> CheckExistingPersonAsync(PersonEntity person);
    Task UpdatePersonAsync(PersonEntity person);
    Task AddPersonAsync(PersonEntity person);
}