using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitites
{
  /// <summary>
  /// Domain Model for storing the country details
  /// </summary>
  public class Country
  {
    [Key]
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    public virtual ICollection<Person>? Persons { get; set; }
  }
}