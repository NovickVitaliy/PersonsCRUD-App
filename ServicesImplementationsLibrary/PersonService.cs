using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContractsLibrary;
using ServiceContractsLibrary.DTO;
using ServiceContractsLibrary.Enums;
using ServicesImplementationsLibrary.Helpers;
using Xunit.Abstractions;

namespace ServicesImplementationsLibrary
{
  public class PersonService : IPersonService
  {
    private readonly IPersonsRepository _personsRepository;
    private readonly ILogger<PersonService> _logger;

    public PersonService(IPersonsRepository personsRepository, ILogger<PersonService> logger)
    {
      _personsRepository = personsRepository;
      _logger = logger;
    }



    public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
    {
      if (personAddRequest == null)
        throw new ArgumentNullException(nameof(personAddRequest));
      if (string.IsNullOrEmpty(personAddRequest.PersonName))
        throw new ArgumentException();

      //Model validations

      ValidationHelper.ModelValidation(personAddRequest);

      Person person = personAddRequest.ToPerson();

      person.Id = Guid.NewGuid();

      //_personsDbContext.sp_InsertPerson(person);

      await _personsRepository.AddPerson(person);

      return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetAllPersons()
    {
      _logger.LogInformation("GetAllPersons of PersonService");

      var persons = await _personsRepository.GetAllPersons();

      return persons.Select(p => p.ToPersonResponse()).ToList();
    }

    public async Task<PersonResponse?> GetPersonById(Guid? personId)
    {
      if (personId == null) return null;

      Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);

      if (person == null) return null;

      return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredList(string searchBy, string? searchString)
    {
      _logger.LogInformation("GetFilteredList of PersonService");

      List<Person> persons = searchBy switch

      {
        nameof(PersonResponse.PersonName) =>
          await _personsRepository.GetFilteredPerson(temp => temp.PersonName.Contains(searchString)),
        nameof(PersonResponse.Email) =>
          await _personsRepository.GetFilteredPerson(temp => temp.Email.Contains(searchString)),
        nameof(PersonResponse.DateOfBirth) =>
           await _personsRepository.GetFilteredPerson(temp => temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),
        nameof(PersonResponse.Gender) =>
           await _personsRepository.GetFilteredPerson(temp => temp.Gender.Contains(searchString)),
        nameof(PersonResponse.CountryID) =>
           await _personsRepository.GetFilteredPerson(temp => temp.Country.CountryName.Contains(searchString)),
        nameof(PersonResponse.Address) =>
           await _personsRepository.GetFilteredPerson(temp => temp.Address.Contains(searchString)),
        _ => await _personsRepository.GetAllPersons()
      };

      return persons.Select(item => item.ToPersonResponse()).ToList();
    }

    public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
      _logger.LogInformation("GetSortedPersons of PersonService");

      if (string.IsNullOrEmpty(sortBy))
        return allPersons;

      List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
      {
        (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
        (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
        (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),
        (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender).ToList(),
        (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender).ToList(),
        (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country).ToList(),
        (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country).ToList(),
        (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address).ToList(),
        (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address).ToList(),
        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
      };

      return sortedPersons;
    }

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
      if (personUpdateRequest == null)
        throw new ArgumentNullException(nameof(personUpdateRequest));

      ValidationHelper.ModelValidation(personUpdateRequest);

      Person? personToUpdate = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId.Value);

      if (personToUpdate == null)
        throw new ArgumentException("Given person does not exist");

      personToUpdate.PersonName = personUpdateRequest.PersonName;
      personToUpdate.Email = personUpdateRequest.Email;
      personToUpdate.Gender = personUpdateRequest.Gender.ToString();
      personToUpdate.CountryID = personUpdateRequest.CountryID;
      personToUpdate.DateOfBirth = personUpdateRequest.DateOfBirth;
      personToUpdate.Address = personUpdateRequest.Address;
      personToUpdate.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

      await _personsRepository.UpdatePerson(personToUpdate);

      return personToUpdate.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? personID)
    {
      if (personID == null)
        throw new ArgumentNullException(nameof(personID));

      Person? personToDelete = await _personsRepository.GetPersonByPersonId(personID.Value);

      if (personToDelete == null)
        return false;

      await _personsRepository.DeletePersonByPersonId(personID.Value);

      return true;
    }

    public async Task<MemoryStream> GetPersonCsv()
    {
      MemoryStream memoryStream = new MemoryStream();
      StreamWriter streamWriter = new StreamWriter(memoryStream);

      CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

      csvWriter.WriteHeader<PersonResponse>();
      csvWriter.NextRecord();

      List<PersonResponse> persons = await GetAllPersons();

      await csvWriter.WriteRecordsAsync(persons);

      memoryStream.Position = 0;

      return memoryStream;
    }
  }
}
