using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace JaackdEAAddin
{
    internal class ContextService : IContextService
    {

        private readonly ILogger<MenuService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEAService _eaService;


        public ContextService(ILogger<MenuService> logger, IConfiguration configuration, IEAService eaService)
        {
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
        public void OnContextItemChanged(EA.Repository Repository, string GUID, EA.ObjectType objectType)
        {
            //MessageBox.Show("OnContextItemChanged");
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
        public virtual bool OnContextItemDoubleClicked(EA.Repository Repository, string GUID, EA.ObjectType ot) {

            //MessageBox.Show("OnContextItemDoubleClicked");
            return false; 
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
        public virtual void OnNotifyContextItemModified(EA.Repository Repository, string GUID, EA.ObjectType ot) {

            //MessageBox.Show("OnContextItemModified");
        }


    }
}
