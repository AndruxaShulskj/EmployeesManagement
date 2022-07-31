using AutoMapper;
using EmployeesManagement.Business.Exceptions;
using EmployeesManagement.Business.Interfaces;
using EmployeesManagement.Business.Models;
using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.Business.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    public PersonService(
        IPersonRepository personRepository, 
        IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Person>> GetPersonsAsync(string surname)
    {
        var persons = await _personRepository.GetPersonsAsync(surname);

        return _mapper.Map<IEnumerable<Person>>(persons);
    }

    public async Task AddPersonAsync(Person model)
    {
        var person = _mapper.Map<PersonEntity>(model);
        
        if (await _personRepository.CheckExistingPersonAsync(person))
        {
            throw new PersonAlreadyExistException();
        }

        await _personRepository.AddPersonAsync(person);
    }
    
    public async Task UpdatePersonAsync(Person model)
    {
        var person = _mapper.Map<PersonEntity>(model);

        if (person.Id == 0)
        {
            throw new PersonNotExistException();
        }

        await _personRepository.UpdatePersonAsync(person);
    }

    public Task DeletePersonAsync(Person model)
    {
        var person = _mapper.Map<PersonEntity>(model);

        person.Deleted = true;

        return _personRepository.UpdatePersonAsync(person);
    }
}