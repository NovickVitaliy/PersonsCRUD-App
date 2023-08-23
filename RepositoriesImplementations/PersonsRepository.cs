using Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesImplementations
{
  public class PersonsRepository : IPersonsRepository
  {
    private readonly ApplicationDbContext _db;
    private readonly ILogger<PersonsRepository> _logger;

    public PersonsRepository(ApplicationDbContext db, ILogger<PersonsRepository> logger)
    {
      _db = db;
      _logger = logger;
    }
    public async Task<Person> AddPerson(Person person)
    {
      _db.Persons.Add(person);
      await _db.SaveChangesAsync();
      return person;
    }

    public async Task<bool> DeletePersonByPersonId(Guid personId)
    {
      _db.Persons.RemoveRange(_db.Persons.Where(i => i.Id == personId));
      int rowsDeleted = await _db.SaveChangesAsync();
      return rowsDeleted > 0;
    }

    public async Task<List<Person>> GetAllPersons()
    {
      _logger.LogInformation("GetAllPersons of PersonRepository");
      return await _db.Persons.Include("Country").ToListAsync();
    }

    public async Task<List<Person>> GetFilteredPerson(Expression<Func<Person, bool>> predicate)
    {
      _logger.LogInformation("GetFilteredPersons of PersonRepository");
      return await _db.Persons.Include("Country").Where(predicate).ToListAsync();
    }

    public async Task<Person?> GetPersonByPersonId(Guid personId)
    {
      return await _db.Persons.Include("Country").FirstOrDefaultAsync(i => i.Id == personId);
    }

    public async Task<Person> UpdatePerson(Person person)
    {
      Person? matchingPerson = await _db.Persons.FirstAsync(item => item.Id == person.Id);

      if (matchingPerson == null)
        return person;
      matchingPerson.PersonName = person.PersonName;
      matchingPerson.Address = person.Address;
      matchingPerson.Gender = person.Gender;
      matchingPerson.CountryID = person.CountryID;
      matchingPerson.Email = person.Email;
      matchingPerson.DateOfBirth = person.DateOfBirth;
      matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

      int rowsUpdated = await _db.SaveChangesAsync();

      return matchingPerson;
    }
  }
}
