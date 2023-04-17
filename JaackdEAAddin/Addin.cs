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
using EA;
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
  public class Addin : IAddin, IPostNewObjectService {
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
    private string baseJaackdFolder = String.Empty;


    //static Addin() {
    //  //Configure();
    //}
    private static Assembly TryLoadFromBaseDirectory(object sender, ResolveEventArgs e) {
      string baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      // This event is called for any assembly that fails to resolve.
      var name = new AssemblyName(e.Name);

      var assemblyPath =
          Path.Combine(baseDirectory, name.Name + ".dll");

      if (System.IO.File.Exists(assemblyPath)) {
        // If we find this missing assembly in the application's base directory,
        // we simply return it.
        return Assembly.Load(AssemblyName.GetAssemblyName(assemblyPath));
      } else {
        return null;
      }
    }

    private void Configure() {

#pragma warning disable CS8601 // Possible null reference assignment.
      baseJaackdFolder =  System.Environment.GetEnvironmentVariable("jaackd-ea");
#pragma warning restore CS8601 // Possible null reference assignment.


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
      AppDomain.CurrentDomain.AssemblyResolve += TryLoadFromBaseDirectory;
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


          string mdgFile = baseJaackdFolder + config.GetValue<string>("MDGFile");
          Log.Logger.Information("Addin::EA_OnInitializeTechnologies(repository): successfully retrieved MDGFile from the configuration service. MDGFile = " + mdgFile);

          IMDGService mdgService = new MDGService(
                  _host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<MDGService>>(),
                  mdgFile);

          services.AddSingleton<IMDGService>(mdgService);


          string mtsFile = baseJaackdFolder + config.GetValue<string>("MTSFile");
          IMTSService mtsService = new MTSService(
            _host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<MTSService>>(),
            mtsFile);

          services.AddSingleton<IMTSService>(mtsService);


          string patternFile = baseJaackdFolder + config.GetValue<string>("PatternFile");
          IPatternService patternService = new PatternService(
            _host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<PatternService>>(),
            patternFile);

          services.AddSingleton<IPatternService>(patternService);


          services.AddSingleton<IContextService, ContextService>();

          services.AddSingleton<IPreNewObjectService, PreNewObjectService>();

          services.AddSingleton<IPostNewObjectService, PostNewObjectService>();


          services.AddSingleton<IBackgroundProcessing, BackgroundProcessing>();

        })
        .UseSerilog()
        .Build();



      IBackgroundProcessing backgroundProcessing = _host.Services.GetRequiredService<IBackgroundProcessing>();
      backgroundProcessing.Run();
    }

    public virtual object EA_OnInitializeTechnologies(EA.Repository repository) {
      IMDGService mdgService = _host.Services.GetRequiredService<IMDGService>();
      return mdgService.GetMDGXML();
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


    /// <summary>
    /// EA_OnContextItemChanged notifies Add-Ins that a different item is now in context.
    /// This event occurs after a user has selected an item anywhere in the Enterprise Architect GUI. Add-Ins that require knowledge of the current item in context can subscribe to this broadcast function. If ot = otRepository, then this function behaves the same as EA_FileOpen.
    /// Also look at EA_OnContextItemDoubleClicked and EA_OnNotifyContextItemModified.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="GUID">Contains the GUID of the new context item. 
    /// This value corresponds to the following properties, depending on the value of the ot parameter:
    /// ot (ObjectType)	- GUID value
    /// otElement  		- Element.ElementGUID
    /// otPackage 		- Package.PackageGUID
    /// otDiagram		- Diagram.DiagramGUID
    /// otAttribute		- Attribute.AttributeGUID
    /// otMethod		- Method.MethodGUID
    /// otConnector		- Connector.ConnectorGUID
    /// otRepository	- NOT APPLICABLE, GUID is an empty string
    /// </param>
    /// <param name="ot">Specifies the type of the new context item.</param>
    public virtual void EA_OnContextItemChanged(EA.Repository Repository, string GUID, EA.ObjectType ot) {
      IContextService contextService = _host.Services.GetRequiredService<IContextService>();
      contextService.OnContextItemChanged(Repository, GUID, ot);
    }

    /// <summary>
    /// EA_OnContextItemDoubleClicked notifies Add-Ins that the user has double-clicked the item currently in context.
    /// This event occurs when a user has double-clicked (or pressed [Enter]) on the item in context, either in a diagram or in the Project Browser. Add-Ins to handle events can subscribe to this broadcast function.
    /// Also look at EA_OnContextItemChanged and EA_OnNotifyContextItemModified.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="GUID">Contains the GUID of the new context item. 
    /// This value corresponds to the following properties, depending on the value of the ot parameter:
    /// ot (ObjectType)	- GUID value
    /// otElement  		- Element.ElementGUID
    /// otPackage 		- Package.PackageGUID
    /// otDiagram		- Diagram.DiagramGUID
    /// otAttribute		- Attribute.AttributeGUID
    /// otMethod		- Method.MethodGUID
    /// otConnector		- Connector.ConnectorGUID
    /// </param>
    /// <param name="ot">Specifies the type of the new context item.</param>
    /// <returns>Return True to notify Enterprise Architect that the double-click event has been handled by an Add-In. Return False to enable Enterprise Architect to continue processing the event.</returns>
    public virtual bool EA_OnContextItemDoubleClicked(EA.Repository Repository, string GUID, EA.ObjectType ot) {
      IContextService contextService = _host.Services.GetRequiredService<IContextService>();
      return contextService.OnContextItemDoubleClicked(Repository, GUID, ot);
    }

    /// <summary>
    /// EA_OnNotifyContextItemModified notifies Add-Ins that the current context item has been modified.
    /// This event occurs when a user has modified the context item. Add-Ins that require knowledge of when an item has been modified can subscribe to this broadcast function.
    /// Also look at EA_OnContextItemChanged and EA_OnContextItemDoubleClicked.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="GUID">Contains the GUID of the new context item. 
    /// This value corresponds to the following properties, depending on the value of the ot parameter:
    /// ot (ObjectType)	- GUID value
    /// otElement  		- Element.ElementGUID
    /// otPackage 		- Package.PackageGUID
    /// otDiagram		- Diagram.DiagramGUID
    /// otAttribute		- Attribute.AttributeGUID
    /// otMethod		- Method.MethodGUID
    /// otConnector		- Connector.ConnectorGUID
    /// </param>
    /// <param name="ot">Specifies the type of the new context item.</param>
    public virtual void EA_OnNotifyContextItemModified(EA.Repository Repository, string GUID, EA.ObjectType ot) {

      IContextService contextService = _host.Services.GetRequiredService<IContextService>();
      contextService.OnNotifyContextItemModified(Repository, GUID, ot);
    }




    #region EA Pre-New-Object Events

    /// <summary>
    /// EA_OnPreNewElement notifies Add-Ins that a new element is about to be created on a diagram. It enables Add-Ins to permit or deny creation of the new element.
    /// This event occurs when a user drags a new element from the Toolbox or Resources window onto a diagram. 
    /// The notification is provided immediately before the element is created, so that the Add-In can disable addition of the element.
    /// Also look at EA_OnPostNewElement.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the element to be created:
    /// - Type: A string value corresponding to Element.Type
    /// - Stereotype: A string value corresponding to Element.Stereotype
    /// - ParentID: A long value corresponding to Element.ParentID
    /// - DiagramID: A long value corresponding to the ID of the diagram to which the element is being added. </param>
    /// <returns>Return True to enable addition of the new element to the model. Return False to disable addition of the new element.</returns>
    public virtual bool EA_OnPreNewElement(EA.Repository Repository, EA.EventProperties Info) {
      IPreNewObjectService preNewObjectService = _host.Services.GetRequiredService<IPreNewObjectService>();
      return preNewObjectService.OnPreNewElement(Repository, Info);
    }

    /// <summary>
    /// EA_OnPreNewConnector notifies Add-Ins that a new connector is about to be created on a diagram. It enables Add-Ins to permit or deny creation of a new connector.
    /// This event occurs when a user drags a new connector from the Toolbox or Resources window, onto a diagram. The notification is provided immediately before the connector is created, so that the Add-In can disable addition of the connector.
    /// Also look at EA_OnPostNewConnector.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the connector to be created:
    /// - Type: A string value corresponding to Connector.Type
    /// - Subtype: A string value corresponding to Connector.Subtype
    /// - Stereotype: A string value corresponding to Connector.Stereotype
    /// - ClientID: A long value corresponding to Connector.ClientID
    /// - SupplierID: A long value corresponding to Connector.SupplierID
    /// - DiagramID: A long value corresponding to Connector.DiagramID.
    /// </param>
    /// <returns>Return True to enable addition of the new connector to the model. Return False to disable addition of the new connector.</returns>
    public virtual bool EA_OnPreNewConnector(EA.Repository Repository, EA.EventProperties Info) {
      IPreNewObjectService preNewObjectService = _host.Services.GetRequiredService<IPreNewObjectService>();
      return preNewObjectService.OnPreNewConnector(Repository, Info);
    }

    /// <summary>
    /// EA_OnPreNewDiagram notifies Add-Ins that a new diagram is about to be created. It enables Add-Ins to permit or deny creation of the new diagram.
    /// The notification is provided immediately before the diagram is created, so that the Add-In can disable addition of the diagram.
    /// Also look at EA_OnPostNewDiagram.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the diagram to be created:
    /// - Type: A string value corresponding to Diagram.Type
    /// - ParentID: A long value corresponding to Diagram.ParentID
    /// - PackageID: A long value corresponding to Diagram.PackageID. </param>
    /// <returns>Return True to enable addition of the new diagram to the model. Return False to disable addition of the new diagram.</returns>
    public virtual bool EA_OnPreNewDiagram(EA.Repository Repository, EA.EventProperties Info) {
      IPreNewObjectService preNewObjectService = _host.Services.GetRequiredService<IPreNewObjectService>();
      return preNewObjectService.OnPreNewDiagram(Repository, Info);
    }

    /// <summary>
    /// EA_OnPreNewDiagramObject notifies Add-Ins that a new diagram object is about to be dropped on a diagram. It enables Add-Ins to permit or deny creation of the new object.
    /// This event occurs when a user drags an object from the Enterprise Architect Project Browser or Resources window onto a diagram. The notification is provided immediately before the object is created, so that the Add-In can disable addition of the object.
    /// Also look at EA_OnPostNewDiagramObject.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the object to be created:
    /// - Type: A string value corresponding to Object.Type
    /// - Stereotype: A string value corresponding to Object.Stereotype
    /// - ParentID: A long value corresponding to Object.ParentID
    /// - DiagramID: A long value corresponding to the ID of the diagram to which the object is being added. </param>
    /// <returns>Return True to enable addition of the object to the model. Return False to disable addition of the object.</returns>
    public virtual bool EA_OnPreNewDiagramObject(EA.Repository Repository, EA.EventProperties Info) {
      IPreNewObjectService preNewObjectService = _host.Services.GetRequiredService<IPreNewObjectService>();
      return preNewObjectService.OnPreNewDiagramObject(Repository, Info);
    }

    /// <summary>
    /// When a user drags any kind of element from the Project Browser onto a diagram, EA_OnPreDropFromTree notifies the Add-In that a new item is about to be dropped onto a diagram. The notification is provided immediately before the element is dropped, so that the Add-In can override the default action that would be taken for this drag.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the element to be created:
    /// · ID: A long value of the type being dropped
    /// · Type: A string value corresponding to type of element being dropped
    /// · DiagramID: A long value corresponding to the ID of the diagram to which the element is being added
    /// · PositionX: The X coordinate into which the element is being dropped
    /// · PositionY: The Y coordinate into which the element is being dropped
    /// · DroppedID: A long value corresponding to the ID of the element the item has been dropped onto</param>
    /// <returns>Returns True to allow the default behavior to be executed. Return False if you are overriding this behavior.</returns>
    public virtual bool EA_OnPreDropFromTree(EA.Repository Repository, EA.EventProperties Info) { return true; }

    /// <summary>
    /// EA_OnPreNewAttribute notifies Add-Ins that a new attribute is about to be created on an element. It enables Add-Ins to permit or deny creation of the new attribute.
    /// This event occurs when a user creates a new attribute on an element by either drag-dropping from the Project Browser, using the Attributes Properties dialog, or using the in-place editor on the diagram. The notification is provided immediately before the attribute is created, so that the Add-In can disable addition of the attribute.
    /// Also look at EA_OnPostNewAttribute.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the attribute to be created:
    /// - Type: A string value corresponding to Attribute.Type
    /// - Stereotype: A string value corresponding to Attribute.Stereotype
    /// - ParentID: A long value corresponding to Attribute.ParentID
    /// - ClassifierID: A long value corresponding to Attribute.ClassifierID. </param>
    /// <returns>Return True to enable addition of the new attribute to the model. Return False to disable addition of the new attribute.</returns>
    public virtual bool EA_OnPreNewAttribute(EA.Repository Repository, EA.EventProperties Info) {
      IPreNewObjectService preNewObjectService = _host.Services.GetRequiredService<IPreNewObjectService>();
      return preNewObjectService.EA_OnPreNewAttribute(Repository, Info);
    }

    /// <summary>
    /// EA_OnPreNewMethod notifies Add-Ins that a new method is about to be created on an element. It enables Add-Ins to permit or deny creation of the new method.
    /// This event occurs when a user creates a new method on an element by either drag-dropping from the Project Browser, using the method Properties dialog, or using the in-place editor on the diagram. The notification is provided immediately before the method is created, so that the Add-In can disable addition of the method.
    /// Also look at EA_OnPostNewMethod.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the method to be created:
    /// - ReturnType: A string value corresponding to Method.ReturnType
    /// - Stereotype: A string value corresponding to Method.Stereotype
    /// - ParentID: A long value corresponding to Method.ParentID
    /// - ClassifierID: A long value corresponding to Method.ClassifierID. </param>
    /// <returns>Return True to enable addition of the new method to the model. Return False to disable addition of the new method.</returns>
    public virtual bool EA_OnPreNewMethod(EA.Repository Repository, EA.EventProperties Info) {
      IPreNewObjectService preNewObjectService = _host.Services.GetRequiredService<IPreNewObjectService>();
      return preNewObjectService.OnPreNewMethod(Repository, Info);
    }

    /// <summary>
    /// EA_OnPreNewPackage notifies Add-Ins that a new package is about to be created in the model. It enables Add-Ins to permit or deny creation of the new package.
    /// This event occurs when a user drags a new package from the Toolbox or Resources window onto a diagram, or by selecting the New Package icon from the Project Browser. The notification is provided immediately before the package is created, so that the Add-In can disable addition of the package.
    /// Also look at EA_OnPostNewPackage.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the package to be created:
    /// Stereotype: A string value corresponding to Package.Stereotype
    /// ParentID: A long value corresponding to Package.ParentID
    /// DiagramID: A long value corresponding to the ID of the diagram to which the package is being added. </param>
    /// <returns>Return True to enable addition of the new package to the model. Return False to disable addition of the new package.</returns>
    public virtual bool EA_OnPreNewPackage(EA.Repository Repository, EA.EventProperties Info) {
      IPreNewObjectService preNewObjectService = _host.Services.GetRequiredService<IPreNewObjectService>();
      return preNewObjectService.OnPreNewPackage(Repository, Info);
    }

    /// <summary>
    /// EA_OnPreNewGlossaryTerm notifies Add-Ins that a new glossary term is about to be created. It enables Add-Ins to permit or deny creation of the new glossary term.
    /// The notification is provided immediately before the glossary term is created, so that the Add-In can disable addition of the element.
    /// Also look at EA_OnPostNewGlossaryTerm.
    /// </summary>
    /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
    /// Poll its members to retrieve model data and user interface status information.</param>
    /// <param name="Info">Contains the following EventProperty objects for the glossary term to be deleted:
    /// TermID: A long value corresponding to Term.TermID.</param>
    /// <returns>Return True to enable addition of the new glossary term to the model. Return False to disable addition of the new glossary term.</returns>
    public virtual bool EA_OnPreNewGlossaryTerm(EA.Repository Repository, EA.EventProperties Info) {
      IPreNewObjectService preNewObjectService = _host.Services.GetRequiredService<IPreNewObjectService>();
      return preNewObjectService.OnPreNewGlossaryTerm(Repository, Info);
    }

    #endregion EA Pre-New Events


    public string EA_OnRetrieveModelTemplate(EA.Repository repository, string location) {
      IPatternService patternService = _host.Services.GetRequiredService<IPatternService>();
      return patternService.GetPatternXML(repository, location);
    }



    ///
    /// EA calls this operation when it exists. Can be used to do some cleanup work.
    ///
    public void EA_Disconnect() {
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    public bool EA_OnPostNewConnector(Repository repository, EventProperties eventProperties) {
      IPostNewObjectService postNewObjectService = _host.Services.GetRequiredService<IPostNewObjectService>();
      return postNewObjectService.EA_OnPostNewConnector(repository, eventProperties);
    }

    public bool EA_OnPostNewElement(Repository repository, EventProperties eventProperties) {
      IPostNewObjectService postNewObjectService = _host.Services.GetRequiredService<IPostNewObjectService>();
      return postNewObjectService.EA_OnPostNewElement(repository, eventProperties);
    }

    public bool EA_OnPostNewDiagram(Repository repository, EventProperties eventProperties) {
      IPostNewObjectService postNewObjectService = _host.Services.GetRequiredService<IPostNewObjectService>();
      return postNewObjectService.EA_OnPostNewDiagram(repository, eventProperties);
    }

    public bool EA_OnPostNewDiagramObject(Repository repository, EventProperties eventProperties) {
      IPostNewObjectService postNewObjectService = _host.Services.GetRequiredService<IPostNewObjectService>();
      return postNewObjectService.EA_OnPostNewDiagramObject(repository, eventProperties);
    }

    public bool EA_OnPostNewAttribute(Repository repository, EventProperties eventProperties) {
      IPostNewObjectService postNewObjectService = _host.Services.GetRequiredService<IPostNewObjectService>();
      return postNewObjectService.EA_OnPostNewAttribute(repository, eventProperties);
    }

    public bool EA_OnPostNewMethod(Repository repository, EventProperties eventProperties) {
      IPostNewObjectService postNewObjectService = _host.Services.GetRequiredService<IPostNewObjectService>();
      return postNewObjectService.EA_OnPostNewMethod(repository, eventProperties);
    }

    public bool EA_OnPostNewPackage(Repository repository, EventProperties eventProperties) {
      IPostNewObjectService postNewObjectService = _host.Services.GetRequiredService<IPostNewObjectService>();
      return postNewObjectService.EA_OnPostNewPackage(repository, eventProperties);
    }

    public bool EA_OnPostNewGlossaryTerm(Repository repository, EventProperties eventProperties) {
      IPostNewObjectService postNewObjectService = _host.Services.GetRequiredService<IPostNewObjectService>();
      return postNewObjectService.EA_OnPostNewGlossaryTerm(repository, eventProperties);
    }
  }

}