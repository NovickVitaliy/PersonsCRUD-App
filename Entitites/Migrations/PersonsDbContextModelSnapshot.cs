﻿// <auto-generated />
using System;
using Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entitites.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class PersonsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entitites.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("f93da20f-3670-436a-b67d-512dfc3e39b9"),
                            CountryName = "Ukraine"
                        },
                        new
                        {
                            CountryId = new Guid("6eae705e-ae07-45ad-85e4-a2fd2edd7db5"),
                            CountryName = "Russia"
                        });
                });

            modelBuilder.Entity("Entitites.Person", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("CountryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("ReceiveNewsLetters")
                        .HasColumnType("bit");

                    b.Property<string>("TIN")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(8)")
                        .HasDefaultValue("ABC12345")
                        .HasColumnName("TaxIdentificationNumber");

                    b.HasKey("Id");

                    b.ToTable("Persons", null, t =>
                        {
                            t.HasCheckConstraint("CHK_TIN", "len([TIN]) = 8");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("eeeb9dcb-a17c-4034-962a-9973872134d8"),
                            Address = "dasdas",
                            CountryID = new Guid("6eae705e-ae07-45ad-85e4-a2fd2edd7db5"),
                            DateOfBirth = new DateTime(2005, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "novickvitaliy@gmail.com",
                            Gender = "Male",
                            PersonName = "Vitalick",
                            ReceiveNewsLetters = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
