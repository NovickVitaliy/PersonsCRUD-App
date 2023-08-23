using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitites;

namespace ServiceContractsLibrary.DTO
{
  /// <summary>
  /// DTO class that is used as return type for most of CountryService methods
  /// </summary>
  public class CountryResponse
  {
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
      if (obj == null) return false;

      if(obj.GetType() != typeof(CountryResponse)) return false;

      return this.CountryId == (obj as CountryResponse).CountryId && this.CountryName == (obj as CountryResponse).CountryName;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }

  public static class CountryResponseExtension
  {
    public static CountryResponse ToCountryResponse(this Country country)
    {
      return new CountryResponse()
      {
        CountryId = country.CountryId,
        CountryName = country.CountryName,
      };
    }
  }
}
