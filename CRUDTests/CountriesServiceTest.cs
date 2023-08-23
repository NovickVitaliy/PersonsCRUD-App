using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitites;
using Microsoft.EntityFrameworkCore;
using ServiceContractsLibrary.DTO;
using ServiceContractsLibrary;
using ServicesImplementationsLibrary;
using EntityFrameworkCoreMock;
using Moq;
using FluentAssertions;
using RepositoryContracts;
using AutoFixture;

namespace CRUDTests
{
  public class CountriesServiceTest
  {
    private readonly ICountriesService _countryService;
    private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
    private readonly ICountriesRepository _countriesRepository;
    private readonly IFixture _fixture;

    public CountriesServiceTest()
    {
      _fixture = new Fixture();
      _countriesRepositoryMock = new Mock<ICountriesRepository>();
      _countriesRepository = _countriesRepositoryMock.Object;


      _countryService = new CountriesService(_countriesRepository);
    }


    #region AddCountry

    //When you supply null it should throw ArgumentNullException
    [Fact]
    public async Task AddCountry_NullCountry_ToBeArgumentNullException()
    {
      //Arrange
      CountryAddRequest? countryAddRequest = null;

      //Assert
      await Assert.ThrowsAsync<ArgumentNullException>(async () =>
      {
        //Act
        await _countryService.AddCountry(countryAddRequest);
      });

    }
    //When the country name is null it should throw ArgumentException
    [Fact]
    public async Task AddCountry_NullCountryName_ToBeArgumentException()
    {
      CountryAddRequest? countryAddRequest = new CountryAddRequest()
      {
        CountryName = null
      };

      await Assert.ThrowsAsync<ArgumentException>(async () =>
      {
        await _countryService.AddCountry(countryAddRequest);
      });
    }

    //When the country name is duplicate it should throw ArgumentException

    [Fact]
    public async Task AddCountry_DuplicateCountryName_ToBeArgumentException()
    {
      CountryAddRequest countryAddRequest = _fixture.Build<CountryAddRequest>()
        .With(c => c.CountryName, "USA")
        .Create();

      Country country = countryAddRequest.ToCountry();

      _countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
        .ReturnsAsync(country);

      await Assert.ThrowsAsync<ArgumentException>(async () =>
      {
        await _countryService.AddCountry(countryAddRequest);
        await _countryService.AddCountry(countryAddRequest);
      });
    }

    //When you supply proper country name it should insert the same into the list of countries
    [Fact]
    public async Task AddCountry_ProperCountryDetails_ToBeSuccessful()
    {
      CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

      Country country = countryAddRequest.ToCountry();

      //_countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
      //  .ReturnsAsync(country);

      _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>()))
        .ReturnsAsync(country);

      var countryResponseActual = await _countryService.AddCountry(countryAddRequest);

      countryResponseActual.CountryId.Should().NotBeEmpty();
    }

    #endregion

    #region GetAllCountries

    [Fact]
    //The list of countries should be empty by default
    public async Task GetAllCountries_EmptyList()
    {
      //Act
      List<Country> countries = new List<Country>();

      _countriesRepositoryMock.Setup(item => item.GetAllCountry())
        .ReturnsAsync(countries);

      var actualCountriesFromResponseList = await _countryService.GetAllCountries();

      //Assert
      Assert.Empty(actualCountriesFromResponseList);
    }

    [Fact]
    public async Task GetAllCountries_AddFewCountries()
    {
      //Arrange
      List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>()
      {
        new CountryAddRequest() {CountryName = "USA"},
        new CountryAddRequest() {CountryName = "Japan"},
        new CountryAddRequest() {CountryName = "Ukraine"}
      };

      var countries = countryAddRequests.Select(c => c.ToCountry()).ToList();

      var countriesResponseExpected = countries.Select(c => c.ToCountryResponse()).ToList();

      _countriesRepositoryMock.Setup(item => item.GetAllCountry())
        .ReturnsAsync(countries);

      var countriesResponseActual = await _countryService.GetAllCountries();

      countriesResponseActual.Should().BeEquivalentTo(countriesResponseExpected);

    }

    #endregion

    #region GetCountryByID

    [Fact]
    public async Task GetCountryByID_NullCountryID_ToBeNull()
    {
      Guid? id = null;
      var countryResponseFromService = await _countryService.GetCountryByCountryID(id);

      Assert.Null(countryResponseFromService);
    }

    [Fact]
    public async Task GetCountryByID_ValidCountryID()
    {
      CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

      Country country = countryAddRequest.ToCountry();

      _countriesRepositoryMock.Setup(item => item.GetCountryByCountryId(It.IsAny<Guid>()))
        .ReturnsAsync(country);

      var countryResponseActual = await _countryService.GetCountryByCountryID(country.CountryId);

      countryResponseActual.Should().NotBeNull();
    }

    #endregion
  }
}
