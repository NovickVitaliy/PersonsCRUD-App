using Entitites;
using Microsoft.EntityFrameworkCore;
using RepositoriesImplementations;
using RepositoryContracts;
using ServiceContractsLibrary;
using ServicesImplementationsLibrary;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;

namespace Contacts_manager
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      //builder.Host.ConfigureLogging(loggingProvider =>
      //{
      //  loggingProvider.ClearProviders();
      //  loggingProvider.AddConsole();
      //  loggingProvider.AddDebug();
      //  loggingProvider.AddEventLog();
      //});

      builder.Host.UseSerilog(
        (HostBuilderContext context,
         IServiceProvider serviceProvider,
         LoggerConfiguration loggerConfiguration) =>
      {
        loggerConfiguration.ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(serviceProvider);
      });

      builder.Services.AddControllersWithViews();
      builder.Services.AddScoped<IPersonService, PersonService>();
      builder.Services.AddScoped<ICountriesService, CountriesService>();
      builder.Services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
      });

      builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
      builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

      builder.Services.AddHttpLogging(option =>
      {
        option.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
      });

      var app = builder.Build();

      if (app.Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpLogging();

      //app.Logger.LogDebug("message");
      //app.Logger.LogInformation("message");
      //app.Logger.LogWarning("message");
      //app.Logger.LogError("message");
      //app.Logger.LogCritical("message");

      if (builder.Environment.IsEnvironment("Test") == false)
        Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

      app.UseStaticFiles();
      app.UseRouting();
      app.MapControllers();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller}/{action}"
        );
      });

      app.Run();
    }

  }
}