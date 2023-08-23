using Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContractsLibrary;
using ServiceContractsLibrary.DTO;
using ServiceContractsLibrary.Enums;

namespace Contacts_manager.Controllers
{
  [Route("[controller]")]
  public class PersonsController : Controller
  {
    private readonly IPersonService _personService;
    private readonly ICountriesService _countriesService;
    private readonly ILogger<PersonsController> _logger;
    public PersonsController(IPersonService personService, ICountriesService countriesService, ILogger<PersonsController> logger)
    {
      _personService = personService;
      _countriesService = countriesService;
      _logger = logger;
    }


    [Route("[action]")]
    [Route("/")]
    public async  Task<IActionResult> Index(string searchBy, string searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
    {
      _logger.LogInformation("Index action method of PersonsController");

      _logger.LogDebug($"searchBy:{searchBy}\n" +
        $"searchString:{searchString}\n" +
        $"sortBy:{sortBy}\n" +
        $"sortOrder:{sortOrder}");

      //Searching
      ViewBag.SearchFields = new Dictionary<string, string>()
      {
        {nameof(Person.PersonName), "Person Name"},
        {nameof(Person.Address), "Address"},
        {nameof(Person.Gender), "Gender"},
        {nameof(Person.Email), "Email"},
        {nameof(Person.CountryID), "CountryID"},
        {nameof(Person.DateOfBirth), "Date of birth"},
      };



      List<PersonResponse> persons = await _personService.GetFilteredList(searchBy, searchString);
      ViewBag.CurrentSearchBy = searchBy;
      ViewBag.CurrentSearchString = searchString;

      //Sorting
      var sortedPersons = await _personService.GetSortedPersons(persons, sortBy, sortOrder);

      ViewBag.CurrentSortBy = sortBy;
      ViewBag.CurrentSortOrder = sortOrder.ToString();

      return View(sortedPersons); //Views/Persons/Index.cshtml
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Create()
    {
      var countries = await _countriesService.GetAllCountries();

      ViewBag.Countries = countries.Select(country => new SelectListItem(){Text = country.CountryName, Value = country.CountryId.ToString()});

      return View();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
    {
      if (!ModelState.IsValid)
      {
        var countries = await _countriesService.GetAllCountries();

        ViewBag.Countries = countries.Select(country => new SelectListItem() { Text = country.CountryName, Value = country.CountryId.ToString() });

        ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        return View(personAddRequest);
      }

      await _personService.AddPerson(personAddRequest);

      return RedirectToAction("Index", "Persons");
    }

    [HttpGet]
    [Route("[action]/{personID}")]
    public async Task<IActionResult> Edit(Guid personID)
    {
      var person = await _personService.GetPersonById(personID);
      var countries = await _countriesService.GetAllCountries();

      ViewBag.Countries = countries.Select(country => new SelectListItem() { Text = country.CountryName, Value = country.CountryId.ToString() });

      if (person == null)
      {
        return RedirectToAction("Index");
      }
      return View(person.ToPersonUpdateRequest());
    }

    [HttpPost]
    [Route("[action]/{personID}")]
    public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
    {
      var personResponse = await _personService.GetPersonById(personUpdateRequest.PersonId);

      if (personResponse == null)
      {
        return RedirectToAction("Index");
      }

      if (ModelState.IsValid)
      {
        PersonResponse updatedPersonResponse = await _personService.UpdatePerson(personUpdateRequest);


        return RedirectToAction("Index");
      }

      var countries = await _countriesService.GetAllCountries();

      ViewBag.Countries = countries.Select(country => new SelectListItem() { Text = country.CountryName, Value = country.CountryId.ToString() });
      ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

      return View(personResponse.ToPersonUpdateRequest());
    }

    [HttpGet]
    [Route("[action]/{personID}")]
    public async Task<IActionResult> Delete(Guid personID)
    {
      PersonResponse? personResponse = await _personService.GetPersonById(personID);

      if (personResponse == null)
        return RedirectToAction("Index");

      return View(personResponse);
    }

    [HttpPost]
    [Route("[action]/{personID}")]
    public async Task<IActionResult> Delete(PersonUpdateRequest person)
    {
      var personResponse = await _personService.GetPersonById(person.PersonId);

      if(personResponse == null)
        return RedirectToAction("Index");

      await _personService.DeletePerson(personResponse.Id);

      return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> PersondPDF()
    {
      List<PersonResponse> persons = await _personService.GetAllPersons();

      //Return view as PDF
      return new ViewAsPdf("PersonsPDF", persons, ViewData)
      {
        PageMargins = new Margins()
        {
          Top = 20,
          Right = 20,
          Bottom = 20,
          Left = 20
        },
        PageOrientation = Orientation.Landscape
      };
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> PersoncCsv()
    {
      MemoryStream memoryStream = await _personService.GetPersonCsv();

      return File(memoryStream, "application/octet-stream", "persons.csv");
    }
  }
}
