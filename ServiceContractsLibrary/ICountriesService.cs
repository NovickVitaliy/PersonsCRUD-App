using ServiceContractsLibrary.DTO;

namespace ServiceContractsLibrary
{
  /// <summary>
  /// Represents business logic for manipulatind Country entity
  /// </summary>
  public interface ICountriesService
  {
    /// <summary>
    /// Adds a country object to the list of countries
    /// </summary>
    /// <param name="countryAddRequest">Country object to add</param>
    /// <returns>Returns the country object after adding it(including newly generated country id)</returns>
    Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
    /// <summary>
    /// returns all countries from the list
    /// </summary>
    /// <returns>All countries form the list as list of CountryResponse</returns>
    Task<List<CountryResponse>> GetAllCountries();

    /// <summary>
    /// Returns country object based on given country id
    /// </summary>
    /// <param name="countryID">CountryID to search</param>
    /// <returns>Matching country object</returns>
    Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);
  }
}