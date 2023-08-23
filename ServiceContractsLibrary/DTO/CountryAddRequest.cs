using System;
using System.Collections.Generic;
using Entitites;

namespace ServiceContractsLibrary.DTO
{
  /// <summary>
  /// DTO class for adding a new country
  /// </summary>
  public class CountryAddRequest
  {
    public string? CountryName { get; set; }

    public Country ToCountry()
    {
      return new Country()
      {
        CountryName = this.CountryName,
      };
    }
  }
}
