using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;
//using Microsoft.VisualBasic;

namespace JaackdEAAddin {


  [ComVisible(true)]
  [Guid("B4CC0052-01CE-406F-9769-E8D9D4A544AB")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAddin {
    String EA_Connect(EA.Repository Repository);
    object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName);
    void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked);
    void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName);
    void EA_Disconnect();
  }


  [ComVisible(true)]
  [Guid("0806C872-F80D-48DC-AD48-2408556757DF")]
  public class Addin : IAddin {
    private const string ADDIN_NAME = "JaackdEAAddin";
    private string logFileName = "";
    private static IHost _host = _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(builder => {
          builder.Sources.Clear();
          builder.AddConfiguration(Utilities.BuildConfiguration(new ConfigurationBuilder()).Build());
        })
        .UseSerilog()
        .Build();

    private static IConfigurationBuilder? builder;
    private static IConfigurationRoot? config;


    //static Addin() {
    //  //Configure();
    //}

    private void Configure() {

      builder = Utilities.BuildConfiguration(new ConfigurationBuilder());
      config = builder.Build();

      logFileName = config.GetValue<string>("LogFileBaseName") + "." + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.GetCultureInfo("en-US")) + ".log";

      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .Enrich.FromLogContext()
        .WriteTo.File(logFileName)
        .CreateLogger();

      Log.Logger.Information("Addin::Configure(): Logging Initialized: logFileName = " + logFileName);
    }


#pragma warning disable CS8601 // Possible null reference assignment. We know this can't be null since we're using the location the Assembly was launched from.
    private string _assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
#pragma warning restore CS8601 // Possible null reference assignment.

    public String AbsoluteFromAddinRelativePath(string relativePath) {
      return _assemblyFolder + relativePath;
    }

    ///
    /// Called Before EA starts and the addin must respond in order to work with EA
    ///
    /// <param name="Repository" />the EA repository
    /// a string
    public String EA_Connect(EA.Repository repository) {
      Configure();
      AddEAServices(repository);

      return ADDIN_NAME;
    }

    private void AddEAServices(EA.Repository repository) {

      _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(builder => {
          builder.Sources.Clear();
          builder.AddConfiguration(Utilities.BuildConfiguration(new ConfigurationBuilder()).Build());
        })
        .ConfigureServices((context, services) => {

          IEAService eaService = new EAService(
            _host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<EAService>>(),
            _host.Services.GetRequiredService<IConfiguration>(),
            repository);

          services.AddSingleton(typeof(IEAService), eaService);

          services.AddSingleton<IMenuService, MenuService>();

          services.AddSingleton<IBackgroundProcessing, BackgroundProcessing>();

        })
        .UseSerilog()
        .Build();



      IBackgroundProcessing backgroundProcessing = _host.Services.GetRequiredService<IBackgroundProcessing>();
      backgroundProcessing.Run();
    }

    public virtual object EA_OnInitializeTechnologies(EA.Repository repository) {

      IConfiguration config = _host.Services.GetRequiredService<IConfiguration>();
      Log.Logger.Information("Addin::EA_OnInitializeTechnologies(repository): successfully retrieved configuration service.");

      string mdgFile = config.GetValue<string>("MDGFile");
      Log.Logger.Information("Addin::EA_OnInitializeTechnologies(repository): successfully retrieved MDGFile from the configuration service. MDGFile = " + mdgFile);

      string xmlString = System.IO.File.ReadAllText(mdgFile);
      return xmlString;
    }

    public void EA_FileOpen(EA.Repository repository) {

    }

    ///
    /// Called when user Clicks Add-Ins Menu item from within EA.
    /// Populates the Menu with our desired selections.
    /// Location can be "TreeView" "MainMenu" or "Diagram".
    ///
    /// <param name="Repository" />the repository
    /// <param name="Location" />the location of the menu
    /// <param name="MenuName" />the name of the menu
    ///
    public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName) {

      IMenuService menuService = _host.Services.GetRequiredService<IMenuService>();
      return menuService.GetMenuItems(Repository, Location, MenuName);
    }

    ///
    /// Called once Menu has been opened to see what menu items should active.
    ///
    /// <param name="Repository" />the repository
    /// <param name="Location" />the location of the menu
    /// <param name="MenuName" />the name of the menu
    /// <param name="ItemName" />the name of the menu item
    /// <param name="IsEnabled" />boolean indicating whethe the menu item is enabled
    /// <param name="IsChecked" />boolean indicating whether the menu is checked
    public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked) {

      IMenuService menuService = _host.Services.GetRequiredService<IMenuService>();
      menuService.GetMenuState(Repository, Location, MenuName, ItemName, ref IsEnabled, ref IsChecked);
    }

    ///
    /// Called when user makes a selection in the menu.
    /// This is your main exit point to the rest of your Add-in
    ///
    /// <param name="Repository" />the repository
    /// <param name="Location" />the location of the menu
    /// <param name="MenuName" />the name of the menu
    /// <param name="ItemName" />the name of the selected menu item
    public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName) {

      IMenuService menuService = _host.Services.GetRequiredService<IMenuService>();
      menuService.MenuClick(Repository, Location, MenuName, ItemName);
    }

    ///
    /// EA calls this operation when it exists. Can be used to do some cleanup work.
    ///
    public void EA_Disconnect() {
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

  }

}