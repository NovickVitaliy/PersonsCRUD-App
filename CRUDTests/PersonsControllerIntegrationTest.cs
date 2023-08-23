using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using FluentAssertions;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;

namespace CRUDTests
{
  public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
  {
    private readonly HttpClient _client;
    public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
      _client = factory.CreateClient();
    }

    #region Index

    [Fact]
    public async void Index_ToReturnView()
    {
      //Arrange
      HttpResponseMessage response = await _client.GetAsync("/Persons/Index");

      //Assert
      response.Should().BeSuccessful(); //2xx

      var responseBody = await response.Content.ReadAsStringAsync();

      HtmlDocument html = new HtmlDocument();
      html.LoadHtml(responseBody);

      var document = html.DocumentNode;

      document.QuerySelectorAll("table.persons").Should().NotBeNull();

    }

    #endregion
  }
}
