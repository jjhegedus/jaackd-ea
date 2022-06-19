using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace JaackdEAAddin {


  [ComVisible(true)]
  [Guid("B4CC0052-01CE-406F-9769-E8D9D4A544AB")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAddin {
    public String EA_Connect(EA.Repository Repository);
    public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName);
    public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked);
    public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName);
    public void EA_Disconnect();
    // String EA_Connect(EA.Repository Repository);
    // object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName);
    // void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked);
    // void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName);
  }


  [ComVisible(true)]
  [Guid("0806C872-F80D-48DC-AD48-2408556757DF")]
  public class Addin : IAddin {

    // define menu constants
    const string menuHeader = "-&MyAddin";
    const string menuHello = "&Say Hello";
    const string menuGoodbye = "&Say Goodbye";

    // remember if we have to say hello or goodbye
    private bool shouldWeSayHello = true;

    ///
    /// Called Before EA starts to check Add-In Exists
    /// Nothing is done here.
    /// This operation needs to exists for the addin to work
    ///
    /// <param name="Repository" />the EA repository
    /// a string
    public String EA_Connect(EA.Repository Repository) {

      var builder = new ConfigurationBuilder();
      Utilities.BuildConfiguration(builder);

      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Build())
        .Enrich.FromLogContext()
        .WriteTo.File("test.log")
        .CreateLogger();

      Log.Logger.Information("EA_Connect: Logging Initialized");
       
      var host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) => {
          services.AddTransient<IBackgroundService, BackgroundService>(); 
        })
        .UseSerilog()
        .Build();

      var backgroundService = ActivatorUtilities.CreateInstance<BackgroundService>(host.Services);
      backgroundService.Run();

      return "a string";
    }

    public void LoadModelConfiguration() {

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

      switch (MenuName) {
        // defines the top level menu option
        case "":
          return menuHeader;
        // defines the submenu options
        case menuHeader:
          string[] subMenus = { menuHello, menuGoodbye };
          return subMenus;
      }

      return "";
    }

    ///
    /// returns true if a project is currently opened
    ///
    /// <param name="Repository" />the repository
    /// true if a project is opened in EA
    bool IsProjectOpen(EA.Repository Repository) {
      try {
        EA.Collection c = Repository.Models;
        return true;
      } catch {
        return false;
      }
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
      if (IsProjectOpen(Repository)) {
        switch (ItemName) {
          // define the state of the hello menu option
          case menuHello:
            IsEnabled = shouldWeSayHello;
            break;
          // define the state of the goodbye menu option
          case menuGoodbye:
            IsEnabled = !shouldWeSayHello;
            break;
          // there shouldn't be any other, but just in case disable it.
          default:
            IsEnabled = false;
            break;
        }
      } else {
        // If no open project, disable all menu options
        IsEnabled = false;
      }
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
      switch (ItemName) {
        // user has clicked the menuHello menu option
        case menuHello:
          this.sayHello();
          break;
        // user has clicked the menuGoodbye menu option
        case menuGoodbye:
          this.sayGoodbye();
          break;
      }
    }

    ///
    /// Say Hello to the world
    ///
    private void sayHello() {
      MessageBox.Show("Hello World");
      this.shouldWeSayHello = false;
    }

    ///
    /// Say Goodbye to the world
    ///
    private void sayGoodbye() {
      MessageBox.Show("Goodbye World");
      this.shouldWeSayHello = true;
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