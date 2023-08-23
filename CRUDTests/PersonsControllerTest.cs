using AutoFixture;
using Moq;
using ServiceContractsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Contacts_manager.Controllers;
using ServiceContractsLibrary.DTO;
using ServiceContractsLibrary.Enums;
using Microsoft.AspNetCore.Mvc;
using Entitites;

namespace CRUDTests
{
  public class PersonsControllerTest
  {
    private readonly IPersonService _personService;
    private readonly ICountriesService _countriesService;
    private readonly Mock<IPersonService> _personServiceMock;
    private readonly Mock<ICountriesService> _countriesServiceMock;
    private readonly IFixture _fixture;

    public PersonsControllerTest()
    {
      _fixture = new Fixture();

      _countriesServiceMock = new Mock<ICountriesService>();
      _countriesService = _countriesServiceMock.Object;

      _personServiceMock = new Mock<IPersonService>();
      _personService = _personServiceMock.Object;
    }

    #region Index

    [Fact]
    public async Task Index_ShouldReturnIndexViewWithPersonsList()
    {
      //Arrange

      List<PersonResponse> persons = _fixture.Create<List<PersonResponse>>();

      PersonsController personsController = new PersonsController(_personService, _countriesService);

      _personServiceMock.Setup(temp => temp.GetFilteredList(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(persons);

      _personServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
        .ReturnsAsync(persons);

      //Act
      IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

      //Assert
      ViewResult viewResult = Assert.IsType<ViewResult>(result);

      viewResult.ViewData.Model.Should()
        .BeOfType<List<PersonResponse>>();
      viewResult.ViewData.Model.Should().Be(persons);
    }


    #endregion

    #region Create

    [Fact]
    public async Task Create_IfModelError_ToReturnCreateView()
    {
      PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();

      PersonResponse personResponse = _fixture.Create<PersonResponse>();

      List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();


      _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);

      _personServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

      PersonsController personsController = new PersonsController(_personService, _countriesService);
      personsController.ModelState.AddModelError("PersonName", "Person Name cannot be blank");
      //Act
      IActionResult result = await personsController.Create(personAddRequest);

      //Assert
      ViewResult viewResult = Assert.IsType<ViewResult>(result);

      viewResult.ViewData.Model.Should()
        .BeAssignableTo<PersonAddRequest>();
      viewResult.ViewData.Model.Should().Be(personAddRequest);
    }

    [Fact]
    public async Task Create_IfNoModelError_ToReturnRedirectToIndex()
    {
      PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();

      PersonResponse personResponse = _fixture.Create<PersonResponse>();

      List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

      _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);

      _personServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

      PersonsController personsController = new PersonsController(_personService, _countriesService);

      //Act
      IActionResult result = await personsController.Create(personAddRequest);

      //Assert
      RedirectToActionResult redirectResult= Assert.IsType<RedirectToActionResult>(result);

      redirectResult.ActionName.Should().Be("Index");
    }
    #endregion
  }
}
