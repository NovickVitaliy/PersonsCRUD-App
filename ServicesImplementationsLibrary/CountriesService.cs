using Entitites;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContractsLibrary;
using ServiceContractsLibrary.DTO;

namespace ServicesImplementationsLibrary
{
  public class CountriesService : ICountriesService
  {
    private readonly List<Country> _countries;
    private readonly ICountriesRepository _countriesRepository;
    public CountriesService(ICountriesRepository countriesRepository)
    {
      _countriesRepository = countriesRepository;
    }

    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    {
      if (countryAddRequest == null)
        throw new ArgumentNullException(nameof(countryAddRequest));
      if (countryAddRequest.CountryName == null)
        throw new ArgumentException();



      if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
      {
        throw new ArgumentException("Given country name already exists");
      }

      Country country = countryAddRequest.ToCountry();

      country.CountryId = Guid.NewGuid();

      await _countriesRepository.AddCountry(country);
      return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
      return (await _countriesRepository.GetAllCountry()).Select(item => item.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
    {
      if (countryID == null)
        return null;

      Country? country = await _countriesRepository.GetCountryByCountryId(countryID.Value);

      if (country == null)
        return null;

      return country.ToCountryResponse();
    }
  }
}