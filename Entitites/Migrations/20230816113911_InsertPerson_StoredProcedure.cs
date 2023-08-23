using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entitites.Migrations
{
  /// <inheritdoc />
  public partial class InsertPerson_StoredProcedure : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      string insertPerson = @"
            CREATE PROCEDURE [dbo].[InsertPerson]
            (@Id uniqueidentifier, 
             @PersonName nvarchar(40),
             @Email nvarchar(40),
             @DateOfBirth datetime2(7),
             @Gender nvarchar(10),
             @CountryID uniqueidentifier,
             @Address nvarchar(200),
             @ReceiveNewsLetters bit)
            AS BEGIN
              INSERT INTO [dbo].[Persons](Id, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters) VALUES (@Id, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters)
            END
            ";

      migrationBuilder.Sql(insertPerson);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      string insertPerson = @"
            DROP PROCEDURE [dbo].[InsertPerson]
            ";

      migrationBuilder.Sql(insertPerson);
    }
  }
}
