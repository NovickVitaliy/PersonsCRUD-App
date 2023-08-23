using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entitites.Migrations
{
  /// <inheritdoc />
  public partial class GetPersons_StoredProcedures : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      string sp_GetAllPerson = @"
            CREATE PROCEDURE [dbo].[GetAllPersons]
            AS BEGIN
              SELECT Id, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters FROM [dbo].[Persons]
            END
            ";

      migrationBuilder.Sql(sp_GetAllPerson);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      string sp_GetAllPerson = @"
            DROP PROCEDURE [dbo].[GetAllPersons]
            ";

      migrationBuilder.Sql(sp_GetAllPerson);
    }
  }
}
