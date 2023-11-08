using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JaackdEAAddin {
  internal class MenuService : IMenuService {

    // define menu constants
    const string menuHeader = "-&JaackdTools";
    const string menuHello = "Say Hello";
    const string menuGoodbye = "Say &Goodbye";
    const string menuOpenMTS = "&Open MTS";
    const string menuShowMain = "&Show Main Window";
    const string menuHideMain = "&Hide Main Window";
    const string menuConvertElementsFromStereotype = "&Convert Elements from Stereotype";
    const string menuConvertConnectorsFromStereotype = "Convert Connectors from Stereotype";
    const string menuConvertSelectedElementsStereotypes = "Convert Selected Elements Stereotypes";
    const string menuTestJaackdTools = "&Test Jaackd Tools";

    // remember if we have to say hello or goodbye
    private bool shouldWeSayHello = true;

    private JaackdMainControl mainControl;
    private bool mainWindowOpen = false;
    private JaackdMainForm overlay;

    private readonly ILogger<MenuService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEAService _eaService;
    private readonly int _highlighBorderColor = 0;

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
          string[] subMenus = { menuHello, menuGoodbye, menuOpenMTS, menuShowMain, menuHideMain, menuConvertElementsFromStereotype, menuConvertConnectorsFromStereotype, menuConvertSelectedElementsStereotypes, menuTestJaackdTools };
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
            IsEnabled = !mainWindowOpen;
            break;
          // define the state of the goodbye menu option
          case menuHideMain:
            IsEnabled = mainWindowOpen;
            break;
          case menuOpenMTS:
            IsEnabled = true;
            break;
          case menuConvertElementsFromStereotype:
            IsEnabled = true;
            break;
          case menuConvertConnectorsFromStereotype:
            IsEnabled = true;
            break;
          case menuConvertSelectedElementsStereotypes:
            IsEnabled = true;
            break;
          case menuTestJaackdTools:
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
        case menuShowMain:
          this.ShowMain();
          break;
        case menuHideMain:
          this.HideMain();
          break;
        case menuConvertElementsFromStereotype:
          this.ConvertElementsFromStereotype();
          break;
        case menuConvertConnectorsFromStereotype:
          this.ConvertConnectorsFromStereotype();
          break;
        case menuConvertSelectedElementsStereotypes:
          this.ConvertSelectedElementsStereotypes();
          break;
        case menuTestJaackdTools:
          this.TestJaackdTools(Repository);
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

    private void ShowMain() {
      if (mainControl is null) {
        mainControl = (JaackdMainControl)_eaService.GetRepository().AddWindow("Jaackd Main Control", "JaackdEAAddin.JaackdMainControl");
      }

      mainControl.Show();


      if (_eaService.GetRepository().ShowAddinWindow("Jaackd Main Window")) {
        mainWindowOpen = true;
      }

    }

    private void HideMain() {
      _eaService.GetRepository().HideAddinWindow();
      //mainControl.Visible = false;
      mainWindowOpen = false;



      //mainForm.Hide();
      //mainWindowOpen = false;
    }

    private void ConvertElementsFromStereotype() {
      string formName = "Convert Elements from Stereotype";

      Dictionary<string, Type> dataElements = new Dictionary<string, Type>();
      dataElements.Add("fromStereotype", typeof(string));
      dataElements.Add("toStereotype", typeof(string));

      Dictionary<string, Tuple<Type, string>> input = Utilities.GetInput(formName, dataElements);

      if (input.Count > 0) {
        IEnumerable<EA.Element> convertedElements = _eaService.ConvertElementsFromStereotype(input["fromStereotype"].Item2, input["toStereotype"].Item2);

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Updated Elements:");
        foreach (EA.Element element in convertedElements) {
          builder.AppendLine("The stereotype for " + element.Name + " was changed to " + element.Stereotype);
        }

        _logger.LogInformation(builder.ToString());
      }
    }

    private void ConvertConnectorsFromStereotype() {
      string formName = "Convert Connectors from Stereotype";

      Dictionary<string, Type> dataElements = new Dictionary<string, Type>();
      dataElements.Add("fromStereotype", typeof(string));
      dataElements.Add("toStereotype", typeof(string));

      Dictionary<string, Tuple<Type, string>> input = Utilities.GetInput(formName, dataElements);

      if (input.Count > 0) {
        IEnumerable<EA.Connector> convertedConnectors = _eaService.ConvertConnectorsFromStereotype(input["fromStereotype"].Item2, input["toStereotype"].Item2);

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Updated Connectors:");
        foreach (EA.Connector connector in convertedConnectors) {
          builder.AppendLine("connector.StereoType = " + connector.Stereotype);
        }

        _logger.LogInformation(builder.ToString());
      }
    }

    private void ConvertSelectedElementsStereotypes() {
      string formName = "Convert Selected Elements Stereotypes";

      Dictionary<string, Type> dataElements = new Dictionary<string, Type>();
      dataElements.Add("toStereotype", typeof(string));

      EA.EASelection selection = _eaService.GetRepository().CurrentSelection;
      EA.Collection elements = _eaService.GetRepository().CurrentSelection.List;

      Dictionary<string, Tuple<Type, string>> input = Utilities.GetInput(formName, dataElements);

      if (input.Count > 0) {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Updated Elements:");

        for (short i = 0; i < elements.Count; i++) {
          EA.EAContext item = (EA.EAContext)elements.GetAt(i);
          

          switch (item.ContextType) {

            case EA.ObjectType.otPackage:
              EA.Element packageElement = _eaService.GetRepository().GetElementByGuid(item.ElementGUID);

              _eaService.UpdateElementStereotype(packageElement, input["toStereotype"].Item2);
              builder.AppendLine("The stereotype for " + packageElement.Name + " was changed to " + packageElement.Stereotype);
              break;

          }
        }

        _logger.LogInformation(builder.ToString());
      }
    }

    private void TestJaackdTools(EA.Repository repository) {
        _logger.LogInformation("TestJaackdTools: Start");
      //LaunchOverlay(repository);

      // Figure out what diagram this is running from
      EA.Diagram diagram = repository.GetCurrentDiagram();


      // Process the nodes
      bool firstIteration = true;
      EA.Element contextObject = null;
      foreach (EA.DiagramObject diagramObject in diagram.DiagramObjects) {
        if (firstIteration) { // The first objeect is the context object so we have a special case to handle it
          firstIteration = false;
          contextObject = repository.GetElementByID(diagramObject.ElementID);
          EA.Element contextElement = repository.GetElementByID(diagramObject.ElementID);
          _logger.LogInformation("Diagram Object Name: " + contextElement.Name + " is the context element");
          continue;
        }
        bool isStartNode = true;
        EA.Element element = repository.GetElementByID(diagramObject.ElementID);
        foreach (EA.Connector connector in element.Connectors) {
          if (connector.SupplierID == element.ElementID && connector.MetaType == "ControlFlow") {
            isStartNode = false;
            ProcessSimulation(repository, diagramObject);
            break;
          }
        }
        _logger.LogInformation("Diagram Object Name: " + element.Name + (isStartNode ? " is a start node" : " is not a start node"));
      }
    }

    private void ProcessSimulation(EA.Repository repository, EA.DiagramObject diagramObject) {
      // Store the color of the diagram object
      int originalBorderColor = diagramObject.BorderColor;
      // Set the color to the highlight color
      diagramObject.BorderColor = _highlighBorderColor;

      // Get the element from the diagramObject
      EA.Element element = repository.GetElementByID(diagramObject.ElementID);

      // Get the outbound control flows
      foreach (EA.Connector connector in element.Connectors) {
        if (connector.SupplierID == element.ElementID && connector.MetaType == "ControlFlow") {
          // Get the element at the opposite end of the control flow
          EA.Element nextElement = repository.GetElementByID(connector.ClientID);

          // Get the inbound object flows

          // Select case for each element type

          //   Atomic Action
          //   Parse the JavaScript
          //   Get the locals
          //   Get the model element references
        }
      }

      // Restore the color of the diagram object
      diagramObject.BorderColor = originalBorderColor;
    }

    private void LaunchOverlay(EA.Repository repository) {

      if(overlay is null) {
        overlay = new JaackdMainForm();
        overlay.Text = "Overlay";
        System.Drawing.Size size = new System.Drawing.Size(SystemInformation.VirtualScreen.Width * 2, SystemInformation.VirtualScreen.Height * 2);
        overlay.Size = size;
        overlay.Top = -size.Height / 4;
        overlay.Left = -size.Width / 4;
      }

      overlay.Show();

    }


    private void HideOverlay(EA.Repository repository) {
      overlay.Hide();
    }

  }
}
