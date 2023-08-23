using Entitites;

namespace RepositoryContracts
{
  /// <summary>
  /// Represents data access logic for managing Country entity
  /// </summary>
  public interface ICountriesRepository
  {
    /// <summary>
    /// Add a new country to the data store
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    Task<Country> AddCountry(Country country);
    Task<List<Country>> GetAllCountry();
    Task<Country?> GetCountryByCountryId(Guid countryId);
    Task<Country?> GetCountryByCountryName(string countryName);

  }
}