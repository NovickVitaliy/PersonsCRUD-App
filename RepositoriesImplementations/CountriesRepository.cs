using Entitites;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace RepositoriesImplementations
{
  public class CountriesRepository : ICountriesRepository
  {
    private readonly ApplicationDbContext _db;

    public CountriesRepository(ApplicationDbContext db)
    {
      _db = db;
    }

    public async Task<Country> AddCountry(Country country)
    {
      _db.Countries.Add(country);
      await _db.SaveChangesAsync();

      return country;
    }

    public async Task<List<Country>> GetAllCountry()
    {
      return await _db.Countries.ToListAsync();
    }

    public async Task<Country?> GetCountryByCountryId(Guid countryId)
    {
      return await _db.Countries.FirstOrDefaultAsync(country => country.CountryId == countryId);
    }

    public async Task<Country?> GetCountryByCountryName(string countryName)
    {
      return await _db.Countries.FirstOrDefaultAsync(country => country.CountryName == countryName);
    }
  }
}