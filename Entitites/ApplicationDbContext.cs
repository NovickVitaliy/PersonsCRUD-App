using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Entitites
{
  public class ApplicationDbContext : DbContext
  {

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Country>().ToTable("Countries");
      modelBuilder.Entity<Person>().ToTable("Persons");

      string countriesJson = File.ReadAllText("Countries.json");
      string personsJson = File.ReadAllText("Persons.json");

      var countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);

      var persons = JsonSerializer.Deserialize<List<Person>>(personsJson);

      modelBuilder.Entity<Country>().HasData(countries);
      modelBuilder.Entity<Person>().HasData(persons);


        //Fluent API
        modelBuilder.Entity<Person>().Property(temp => temp.TIN)
          .HasColumnName("TaxIdentificationNumber")
          .HasColumnType("varchar(8)")
          .HasDefaultValue("ABC12345");

        //modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN)
          //.IsUnique();

          modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

          //modelBuilder.Entity<Person>(entity =>
          //{
          //  entity.HasOne<Country>(c => c.Country)
          //    .WithMany(p => p.Persons)
          //    .HasForeignKey(p => p.CountryID);
          //});
    }

    public List<Person> sp_GetAllPersons()
    {
       return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
    }

    public int sp_InsertPerson(Person person)
    {
      SqlParameter[] sqlParameters = new SqlParameter[]
      {
        new SqlParameter("@Id", person.Id),
        new SqlParameter("@PersonName", person.PersonName),
        new SqlParameter("@Email", person.Email),
        new SqlParameter("@DateOfBirth", person.DateOfBirth),
        new SqlParameter("@Gender", person.Gender),
        new SqlParameter("@CountryID", person.CountryID),
        new SqlParameter("@Address", person.Address),
        new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
      };

      return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @Id, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters", sqlParameters);
    }
  }
}
