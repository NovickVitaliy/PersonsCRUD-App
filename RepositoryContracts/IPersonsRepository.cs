using Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
  /// <summary>
  /// Represent data access logic for managing a Person entity
  /// </summary>
  public interface IPersonsRepository
  {
    Task<Person> AddPerson(Person person);
    Task<List<Person>> GetAllPersons();
    Task<Person?> GetPersonByPersonId(Guid personId);
    Task<List<Person>> GetFilteredPerson(Expression<Func<Person, bool>> predicate);
    Task<bool> DeletePersonByPersonId(Guid personId);
    Task<Person> UpdatePerson(Person person);
  }
}
