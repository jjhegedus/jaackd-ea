using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace JaackdEAAddin {
  internal class MenuService : IMenuService {

    // define menu constants
    const string menuHeader = "-&JaackdTools";
    const string menuHello = "&Say Hello";
    const string menuGoodbye = "&Say Goodbye";
    const string menuOpenMTS = "&Open MTS";
    const string menuShowMain = "&Show Main Window";
    const string menuHideMain = "&Hide Main Window";

    // remember if we have to say hello or goodbye
    private bool shouldWeSayHello = true;
    private bool mainWindowOpen = true;

    private readonly ILogger<MenuService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEAService _eaService;

    

    public MenuService(ILogger<MenuService> logger, IConfiguration configuration, IEAService eaService) {
      _logger = logger;
      _configuration = configuration;
      _eaService = eaService;
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
    public object GetMenuItems(EA.Repository Repository, string Location, string MenuName) {

      switch (MenuName) {
        // defines the top level menu option
        case "":
          return menuHeader;
        // defines the submenu options
        case menuHeader:
          string[] subMenus = { menuHello, menuGoodbye, menuOpenMTS };
          return subMenus;
      }

      return "";
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
    public void GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked) {

      if (Utilities.IsProjectOpen(Repository)) {
        switch (ItemName) {
          // define the state of the hello menu option
          case menuHello:
            IsEnabled = shouldWeSayHello;
            break;
          // define the state of the goodbye menu option
          case menuGoodbye:
            IsEnabled = !shouldWeSayHello;
            break;
          // show the main window
          case menuShowMain:
            IsEnabled = !shouldWeSayHello;
            break;
          // define the state of the goodbye menu option
          case menuHideMain:
            IsEnabled = !shouldWeSayHello;
            break;
          case menuOpenMTS:
            IsEnabled = true;
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
    public void MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName) {
      switch (ItemName) {
        // user has clicked the menuHello menu option
        case menuHello:
          this.sayHello();
          break;
        // user has clicked the menuGoodbye menu option
        case menuGoodbye:
          this.sayGoodbye();
          break;
        case menuOpenMTS:
          this.OpenMTSFile();
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



    private void OpenMTSFile() {
      string filterString = "MTS Files (*.mts)|*.mts|";
      int defaultFilterIndex = 1;
      string initialDir = "";
      string fileName = "";
      int OF_FILEMUSTEXIST = 0x1000;

      EA.Project project = _eaService.GetRepository().GetProjectInterface();

      var filePath = "";
      filePath = project.GetFileNameDialog(fileName, filterString, defaultFilterIndex,
                                           OF_FILEMUSTEXIST, initialDir, 0);
      _logger.LogInformation("MTS File Path = " + filePath);
    }


  }
}
