using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitites;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContractsLibrary;
using ServiceContractsLibrary.DTO;
using ServiceContractsLibrary.Enums;
using ServicesImplementationsLibrary;
using Xunit.Abstractions;
using Xunit.Sdk;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Linq.Expressions;

namespace CRUDTests
{
  public class PersonServiceTest
  {
    private readonly IPersonService _personService;
    private readonly IFixture _fixture;
    private readonly IPersonsRepository _personsRepository;
    private readonly Mock<IPersonsRepository> _personRepositoryMock;
    public PersonServiceTest()
    {
      _fixture = new Fixture();
      _personRepositoryMock = new Mock<IPersonsRepository>();
      _personsRepository = _personRepositoryMock.Object;

      _personService = new PersonService(_personsRepository);

    }

    #region AddPerson

    [Fact]
    public async Task AddPerson_NullPersonAddRequest_ToBeArgumentNullException()
    {
      PersonAddRequest personAddRequest = null;

      Func<Task> action = async () =>
      {
        await _personService.AddPerson(personAddRequest);
      };

      await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddPerson_NullPersonName_ToBeArgumentException()
    {
      PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
        .With(p => p.PersonName, null as string)
        .Create();

      Person person = personAddRequest.ToPerson();

      _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
        .ReturnsAsync(person);

      Func<Task> action = async () =>
       {
         await _personService.AddPerson(personAddRequest);
       };

      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
    {
      PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
        .With(temp => temp.Email, "someone@example.com")
        .Create();

      Person person = personAddRequest.ToPerson();
      PersonResponse personReponseExpected = person.ToPersonResponse();

      //if we supply any argument value to that add person it should return the same return value
      _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
        .ReturnsAsync(person);

      PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

      personReponseExpected.Id = personResponse.Id;

      personResponse.Id.Should().NotBe(Guid.Empty);

      personResponse.Should().Be(personReponseExpected);
    }

    #endregion

    #region GetPerson


    [Fact]
    public async Task GetPerson_NullPersonId_ToBeNull()
    {
      Guid? id = null;

      PersonResponse? personResponse = await _personService.GetPersonById(id);

      personResponse.Should().BeNull();
    }


    [Fact]
    public async Task GetPerson_ValidPersonId_ToBeSuccessful()
    {
      Person person = _fixture.Build<Person>()
        .With(p => p.Email, "pidor@gmail.com")
        .With(p => p.Country, null as Country)
        .Create();

      PersonResponse personExpected = person.ToPersonResponse();

      _personRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
        .ReturnsAsync(person);

      var personActual = await _personService.GetPersonById(person.Id);

      personActual.Should().Be(personExpected);
    }

    #endregion

    #region  GetAllPersons

    //
    [Fact]
    public async Task GetAllPersons_EmptyList()
    {
      var persons = new List<Person>();
      _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

      List<PersonResponse> listFromService = await _personService.GetAllPersons();

      listFromService.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
    {
      List<Person> persons = new List<Person>()
      {
        _fixture.Build<Person>().With(item => item.Email, "lol@gmail.com").With(temp => temp.Country, null as Country).Create(),
        _fixture.Build<Person>().With(item => item.Email, "lol@gmail.com").With(temp => temp.Country, null as Country).Create(),
        _fixture.Build<Person>().With(item => item.Email, "lol@gmail.com").With(temp => temp.Country, null as Country).Create(),
      };

      List<PersonResponse> personResponseFromAdd = persons.Select(person => person.ToPersonResponse()).ToList();

      _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

      var listOfPersonsFromGet = await _personService.GetAllPersons();

      listOfPersonsFromGet.Should().BeEquivalentTo(personResponseFromAdd);
    }

    #endregion

    #region GetFilteredPersonі

    [Fact]
    public async Task GetFilteredPerson_EmptySearchText()
    {
      List<Person> persons = new List<Person>()
      {
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(temp => temp.Country, null as Country)
        .Create(),
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(temp => temp.Country, null as Country)
        .Create(),
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(temp => temp.Country, null as Country)
        .Create(),
      };

      List<PersonResponse> personResponseListExpected = persons.Select(p => p.ToPersonResponse()).ToList();

      _personRepositoryMock.Setup(temp => temp.GetFilteredPerson(It.IsAny<Expression<Func<Person, bool>>>()))
        .ReturnsAsync(persons);

      var personResponsesActual = await _personService.GetFilteredList(nameof(Person.PersonName), "");
      personResponsesActual.Should().BeEquivalentTo(personResponseListExpected);
    }

    [Fact]
    public async Task GetFilteredPerson_SearchByPersonName()
    {
      List<Person> persons = new List<Person>()
      {
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(item => item.Country, null as Country)
        .Create(),
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(item => item.Country, null as Country)
        .Create(),
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(item => item.Country, null as Country)
        .Create(),
      };

      List<PersonResponse> personResponsesExpected = persons.Select(p => p.ToPersonResponse()).ToList();

      _personRepositoryMock.Setup(item => item.GetFilteredPerson(It.IsAny<Expression<Func<Person, bool>>>()))
        .ReturnsAsync(persons);


      List<PersonResponse> personResponsesActual = await _personService.GetFilteredList(nameof(Person.PersonName), "ma");

      personResponsesActual.Should().BeEquivalentTo(personResponsesExpected);
    }

    #endregion

    #region GetSortedList
    //When we sort based on a person name in descending order it should return the persons in descending order
    [Fact]
    public async Task GetSortedPersons()
    {
      List<Person> persons = new List<Person>()
      {
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(temp => temp.Country, null as Country)
        .Create(),
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(temp => temp.Country, null as Country)
        .Create(),
        _fixture.Build<Person>()
        .With(item => item.Email, "lol@gmail.com")
        .With(temp => temp.Country, null as Country)
        .Create(),
      };

      List<PersonResponse> personResponseListExpected = persons.Select(p => p.ToPersonResponse()).ToList();

      _personRepositoryMock.Setup(temp => temp.GetAllPersons())
        .ReturnsAsync(persons);

      var allPersons = await _personService.GetAllPersons();

      List<PersonResponse> personListFromSort = await _personService.GetSortedPersons(await _personService.GetAllPersons(), nameof(Person.PersonName), SortOrderOptions.DESC);

      personListFromSort.Should().BeInDescendingOrder(temp => temp.PersonName);

    }

    #endregion

    #region UpdatePerson

    //When supply a null value it has to throw ArgumentNullException
    [Fact]
    public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
    {
      PersonUpdateRequest? personUpdateRequest = null;

      Func<Task> action = async () =>
      {
        await _personService.UpdatePerson(personUpdateRequest);
      };

      await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
    {
      PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
      {
        PersonId = Guid.NewGuid()
      };

      //await Assert.ThrowsAsync<ArgumentException>(async () =>
      //{
      //  await _personService.UpdatePerson(personUpdateRequest);
      //});

      Func<Task> action = async () =>
      {
        await _personService.UpdatePerson(personUpdateRequest);
      };

      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdatePerson_NullPersonName_ToBeArgumentException()
    {
      Person person = _fixture.Build<Person>()
        .With(temp => temp.Gender, "Male")
        .With(temp => temp.PersonName, null as string)
        .With(item => item.Email, "some@gmail.com")
        .With(item => item.Country, null as Country)
        .Create();

      PersonResponse personResponseExpected = person.ToPersonResponse();

      PersonUpdateRequest? personUpdateRequest = personResponseExpected.ToPersonUpdateRequest();

      Func<Task> action = (async () =>
      {
        await _personService.UpdatePerson(personUpdateRequest);
      });

      await action.Should().ThrowAsync<ArgumentException>();

    }

    [Fact]
    public async Task UpdatePerson_PersonFullDetailsUpdation()
    {
      Person person = _fixture.Build<Person>()
        .With(temp => temp.Gender, "Male")
        .With(temp => temp.PersonName, "Vitalick")
        .With(item => item.Email, "some@gmail.com")
        .With(item => item.Country, null as Country)
        .Create();

      PersonResponse personResponseExpected = person.ToPersonResponse();

      PersonUpdateRequest personUpdateRequest = personResponseExpected.ToPersonUpdateRequest();

      _personRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
        .ReturnsAsync(person);

      _personRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
        .ReturnsAsync(person);

      PersonResponse updatedPerson = await _personService.UpdatePerson(personUpdateRequest);


      updatedPerson.Should().Be(personResponseExpected);
    }
    #endregion

    #region DeletePerson

    [Fact]
    public async Task DeletePerson_NullID_ToBeExpected()
    {
      Guid? id = null;

      await Assert.ThrowsAsync<ArgumentNullException>(async () => await _personService.DeletePerson(id));
    }

    [Fact]
    public async Task DeletePerson_SuccessDelete()
    {
      Person person = _fixture.Build<Person>()
        .With(temp => temp.Email, "some@gmail.com")
        .With(temp => temp.Gender, "Male")
        .With(temp => temp.Country, null as Country)
        .Create();

      PersonResponse personResponseFromAdd = person.ToPersonResponse();

      _personRepositoryMock.Setup(temp => temp.DeletePersonByPersonId(It.IsAny<Guid>()))
        .ReturnsAsync(true);

      _personRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
        .ReturnsAsync(person);

      bool isDeleted = await _personService.DeletePerson(personResponseFromAdd.Id);

      isDeleted.Should().BeTrue();
    }

    #endregion

  }
}
