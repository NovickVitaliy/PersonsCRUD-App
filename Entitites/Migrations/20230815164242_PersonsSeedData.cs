using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entitites.Migrations
{
    /// <inheritdoc />
    public partial class PersonsSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[] { new Guid("eeeb9dcb-a17c-4034-962a-9973872134d8"), "dasdas", new Guid("6eae705e-ae07-45ad-85e4-a2fd2edd7db5"), new DateTime(2005, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "novickvitaliy@gmail.com", "Male", "Vitalick", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("eeeb9dcb-a17c-4034-962a-9973872134d8"));
        }
    }
}
