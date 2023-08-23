using Entitites;
using ServiceContractsLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContractsLibrary.DTO
{
  /// <summary>
  /// Represents the DTO class that contains the person details to be updated
  /// </summary>
  public class PersonUpdateRequest
  {
    [Required(ErrorMessage = "Person id should be supplied")]
    public Guid? PersonId { get; set; }

    [Required(ErrorMessage = "Person name cannot be blank")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "Email cannot be blank")]
    [EmailAddress]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public GenderOptions? Gender { get; set; }
    public Guid? CountryID { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }

    /// <summary>
    /// Convert the current object of PersonAddRequest to Person type
    /// </summary>
    /// <returns></returns>
    public Person ToPerson()
    {
      return new Person()
      {
        PersonName = PersonName,
        Email = Email,
        DateOfBirth = DateOfBirth,
        Gender = Gender.ToString(),
        CountryID = CountryID,
        Address = Address,
        ReceiveNewsLetters = ReceiveNewsLetters,
        Id = PersonId
      };
    }
  }
}
