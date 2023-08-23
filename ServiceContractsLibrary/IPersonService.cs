using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitites;
using ServiceContractsLibrary.DTO;
using ServiceContractsLibrary.Enums;

namespace ServiceContractsLibrary
{
  public interface IPersonService
  {
    Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
    Task<List<PersonResponse>> GetAllPersons();
    Task<PersonResponse?> GetPersonById(Guid? personID);
    Task<List<PersonResponse>> GetFilteredList(string searchBy, string? searchString);
    Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
    Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    Task<bool> DeletePerson(Guid? personID);

    Task<MemoryStream> GetPersonCsv();
  }
}
