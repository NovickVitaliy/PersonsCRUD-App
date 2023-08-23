using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitites;
using ServiceContractsLibrary.Enums;

namespace ServiceContractsLibrary.DTO
{
  /// <summary>
  /// Represents DTO class that is used as return type of most method of Person Service
  /// </summary>
  public class PersonResponse
  {
    public Guid? Id { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryID { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public double? Age { get; set; }

    public override bool Equals(object? obj)
    {
      if (obj == null) return false;
      if(obj.GetType() != typeof(PersonResponse)) return false;

      PersonResponse personResponse = (PersonResponse)obj;

      return this.Id == personResponse.Id &&
             this.PersonName == personResponse.PersonName &&
             this.Email == personResponse.Email &&
             this.DateOfBirth == personResponse.DateOfBirth &&
             this.Gender == personResponse.Gender &&
             this.CountryID == personResponse.CountryID &&
             this.Address == personResponse.Address &&
             this.ReceiveNewsLetters == personResponse.ReceiveNewsLetters;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override string ToString()
    {
      return $"Name: {PersonName}\n" +
             $"Id: {Id}\n" +
             $"Email: {Email}\n" +
             $"Date of birth: {DateOfBirth}\n" +
             $"Gender: {Gender}\n" +
             $"CountryId: {CountryID}\n" +
             $"Country: {Country}\n" +
             $"Address:{Address}\n" +
             $"ReceiveNewsLetter: {ReceiveNewsLetters}\n" +
             $"Age: {Age}\n\n";
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
      return new PersonUpdateRequest()
      {
        PersonId = Id,
        Address = Address,
        CountryID = CountryID,
        DateOfBirth = DateOfBirth,
        Email = Email,
        Gender = Enum.Parse<GenderOptions>(Gender, true),
        PersonName = PersonName,
        ReceiveNewsLetters = ReceiveNewsLetters
      };
    }
  }

  public static class PersonExtension
  {
    public static PersonResponse ToPersonResponse(this Person person)
    {
      return new PersonResponse()
      {
         Id = person.Id,
         PersonName = person.PersonName,
         Address = person.Address,
         Email = person.Email,
         DateOfBirth = person.DateOfBirth,
         Gender = person.Gender,
         CountryID = person.CountryID,
         ReceiveNewsLetters = person.ReceiveNewsLetters,
         Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
         Country = person.Country?.CountryName
      };
    }
  }
}
